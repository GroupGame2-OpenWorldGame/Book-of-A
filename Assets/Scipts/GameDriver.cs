using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum GameState{
	OverWorld,
	Dialogue
}

public class GameDriver : MonoBehaviour {

	[Header("Game State")]
	public GameState gameState = GameState.OverWorld;
	[Space(8)]

	[Header("Dialogue")]
	public GameObject dialogueBox;
	public GameObject optionsBox;
	public Text dialogueText;
	public Text speakerName;
	public Image playerImage;
	public Image npcImage;
	public float optionY = 136f;
	[Space(8)]

	[Header("Testing")]
	public string playerName = "Mel";
	public bool branchCondition = false;

	//TESTING VARIABLES
	public bool dialogueHit = false;

	public string[] flagNames;

	private Button[] optionButtons;
	private int numOptions;
	private int selectedButton = 0;

	private Dictionary<string, bool> flags;

	private NPCScript npcTarget;
	//private Dialogue currentDialogue;
	//private int currentLine = 0;
	private DialogueHead currentDialogue;
	private DialogueElement currentLine;
	private PlayerController player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();

		flags = new Dictionary<string, bool>();

		for( int i = 0; i < flagNames.Length; i++){
			flags.Add(flagNames[i], false);
		}

		optionButtons = optionsBox.GetComponentsInChildren<Button> ();
		CleanUpOptions ();
		dialogueBox.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public NPCScript NPCTarget{
		get{ 
			return npcTarget; 
		}
		set{
			npcTarget = value; 
		}
	}

	/*
	public int DialogueLength{
		get{ return currentDialogue.Length; }
		set { currentDialogue.Length = value; }
	}
	*/

	public void StartDialogue(){
		gameState = GameState.Dialogue;
		player.SetMovement (false);
		/*
		DeserializeXMLDialogue (npcTarget.dialogueXMLPath);
		if (gameState == GameState.Dialogue) {
			speakerName.text = currentDialogue.Speaker;
			dialogueText.text = currentDialogue.Speech [0];
			currentLine = 0;
			dialogueBox.SetActive (true);
		}
		*/
		if (npcTarget != null) {
			if (npcTarget.dialogueHead != null) {
				currentDialogue = npcTarget.dialogueHead;
				currentLine = currentDialogue.FindLineById (currentDialogue.FirstLineId);
				npcImage.sprite = npcTarget.portrait;
				dialogueBox.SetActive (true);
				HandleDialogue ();
			}
		}
	}

	public void HandleDialogue(){
		if (currentLine.GetType () == typeof(DialogueLine)) {
			DialogueLine line = (DialogueLine)currentLine;
			SetLine (line.Speaker, line.Text);
			return;
		} else if (currentLine.GetType () == typeof(DialogueIfBranch)) {
			DialogueIfBranch ifLine = (DialogueIfBranch)currentLine;
			if (CheckConditions (ifLine.ConditionsToCheck, ifLine.CheckType)) {
				if (ifLine.TrueLineId == 0) {
					EndDialogue ();
					return;
				}
				currentLine = currentDialogue.FindLineById (ifLine.TrueLineId);
				HandleDialogue ();
				return;
			} else if (ifLine.FalseLineId == 0) {
				EndDialogue ();
				return;
			} else {
				currentLine = currentDialogue.FindLineById (ifLine.FalseLineId);
				;
				HandleDialogue ();
			}
			return;
		} else if (currentLine.GetType () == typeof(DialogueOption)) {
			DialogueOption options = (DialogueOption)currentLine;
			SetLine (options.Speaker, options.Text);
			SetUpOptions (options);
			return;
		} else {
			EndDialogue ();
		}
		return;
}

	public void SetLine(string speaker, string text){
		if (speaker == "Player") {
			speakerName.text = playerName;
			playerImage.color = new Color (255f, 255f, 255f, 1f);
			npcImage.color = new Color (255f, 255f, 255f, 0.5f);
		} else {
			speakerName.text = speaker;
			npcImage.color = new Color (255f, 255f, 255f, 1f);
			playerImage.color = new Color (255f, 255f, 255f, 0.5f);
		}
		dialogueText.text = text;
		return;
	}

	public void SetUpOptions(DialogueOption optionsElement){
		float optionsSpace = Mathf.Max(0, (150 * (optionsElement.Options.Length - 1) - 30)/2);
		Debug.Log (optionButtons.Length);
		numOptions = optionsElement.Options.Length;
		for (int i = 0; i < optionsElement.Options.Length; i++) {
			optionButtons[i].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(optionsSpace - (150 * (numOptions - 1 - i)) ,0,0);
			optionButtons[i].gameObject.GetComponentsInChildren<Text> ().First ().text = optionsElement.Options [i];
			optionButtons [i].gameObject.SetActive (true);
		}
		selectedButton = 0;
		EventSystem.current.SetSelectedGameObject (optionButtons [0].gameObject);
	}

	public void ChangeHoveredOption(int whichWay){
		if (currentLine is DialogueOption) {
			selectedButton += whichWay;
			if (selectedButton < 0) {
				selectedButton = numOptions - 1;
			}
			if (selectedButton >= numOptions) {
				selectedButton = 0;
			}
			EventSystem.current.SetSelectedGameObject (optionButtons [selectedButton].gameObject);
		}
	}

	public void SelectOption(int optionNumber){
		DialogueOption options = (DialogueOption)currentLine;
		if (optionNumber > (options.Options.Length - 1)) {
			EndDialogue ();
		} else {
			currentLine = currentDialogue.FindLineById (options.DecisionLineIds [optionNumber]);
			CleanUpOptions();
			HandleDialogue ();
		}
	}

	public void CleanUpOptions(){
		foreach (Button opBut in optionButtons) {
			opBut.gameObject.SetActive (false);
		}
		EventSystem.current.SetSelectedGameObject (null);
	}
		
	public void AdvanceDialogue(){
		if (currentLine is DialogueLine) {
			if (currentLine.SetWhenDone != null) {
				SetFlag (currentLine.SetWhenDone, currentLine.SetType);
				currentLine.TriggerPassed = true;
			}
			DialogueLine line = (DialogueLine)currentLine;
			if (line.NextLineId == 0) {
				EndDialogue ();
			} else {
				currentLine = currentDialogue.FindLineById (line.NextLineId);
				Debug.Log ("next line");
				HandleDialogue ();
			}
		} else if (currentLine is DialogueOption) {
			SelectOption (selectedButton);
		}
	}

	public void EndDialogue(){
		dialogueBox.SetActive (false);
		gameState = GameState.OverWorld;
		player.SetMovement (true);
	}

	public void SetFlag(string flagToSet, string setTo){
		if (string.Equals ("true", setTo, StringComparison.InvariantCultureIgnoreCase)) {
			flags [flagToSet] = true;
		} else {
			flags [flagToSet] = false;
		}
	}

	public bool CheckConditions(string[] flagsToCheck, string checkType){
		if (checkType.Equals ("OR")) {
			return IsOneFlagTrue (flagsToCheck);
		} else if (checkType.Equals ("AND")) {
			return AreFlagsTrue (flagsToCheck);
		}
		return false;
	}

	public bool IsFlagTrue(string flagToCheck){
		if (flagToCheck.StartsWith("!")) {
			return !(flags[flagToCheck.Split('!')[1]]);
		}
		return flags [flagToCheck];
	}

	public bool IsOneFlagTrue(string[] flagsToCheck){
		foreach (string f in flagsToCheck) {
			Debug.Log ("Checking " + f);
			if (f.StartsWith("!")) {
				if(!flags[f.Split('!')[1]]){
					return true;
				}
			} else if(flags[f]){
				return true;
			}
		}
		return false;
	}

	//array version
	public bool AreFlagsTrue(string[] flagsToCheck){
		foreach (string f in flagsToCheck) {
			Debug.Log ("Checking " + f);
			if (f.StartsWith("!")) {
				if(flags[f.Split('!')[1]]){
					return false;
				}
			} else if(!flags[f]){
				return false;
			}
		}
		return true;
	}

	public void DialogueHitTrue(){
		dialogueHit = true;
	}

	public void CheckDialogueHit(){
		if (dialogueHit) {
			branchCondition = true;
		} else {
			branchCondition = false;
		}
	}

	/*
	public void DeserializeXMLDialogue(string xmlDialoguePath){
		System.IO.FileStream filestream;
		XmlReader reader;
		XmlSerializer serializer = new XmlSerializer (typeof(DialogueHead), new XmlRootAttribute("DialogueHead"));

		//TextAsset xmlDialogue = (TextAsset)Resources.Load (xmlDialoguePath);


		if (System.IO.File.Exists(UnityEngine.Application.dataPath + xmlDialoguePath)) {
			filestream = new System.IO.FileStream (UnityEngine.Application.dataPath + xmlDialoguePath, System.IO.FileMode.Open);
			reader = new XmlTextReader (filestream);
		} else {
			Debug.Log ("ERROR PARSING XML");
			EndDialogue ();
			return;
		}

		try {
			if (serializer.CanDeserialize (reader)) {
				currentDialogue = serializer.Deserialize (reader) as Dialogue;
			}
			else{
				Debug.Log("Falied");
			}
		} finally {
			reader.Close ();
			filestream.Close ();
			filestream.Dispose ();
			Debug.Log ("FINISHED");
			Debug.Log (currentDialogue.Id);
			Debug.Log (currentDialogue.Speaker);
			Debug.Log (currentDialogue.Length);
			Debug.Log (currentDialogue.Speech[0]);
			Debug.Log (currentDialogue.Speech[1]);
			Debug.Log (currentDialogue.Speech[2]);
		}
	}
	*/
	
	public DialogueHead UnpackDeserializeXMLDialogue(string xmlDialoguePath){
		System.IO.FileStream filestream;
		XmlReader reader;
		XmlSerializer serializer = new XmlSerializer (typeof(DialogueHead), new XmlRootAttribute("DialogueHead"));

		//TextAsset xmlDialogue = (TextAsset)Resources.Load (xmlDialoguePath);


		if (System.IO.File.Exists(UnityEngine.Application.dataPath + xmlDialoguePath)) {
			filestream = new System.IO.FileStream (UnityEngine.Application.dataPath + xmlDialoguePath, System.IO.FileMode.Open);
			reader = new XmlTextReader (filestream);
		} else {
			Debug.Log ("ERROR PARSING XML");
			EndDialogue ();
			return null;
		}

		DialogueHead toReturn;

		try {
			if (serializer.CanDeserialize (reader)) {
				toReturn = serializer.Deserialize (reader) as DialogueHead;
			}
			else{
				Debug.Log("Falied");
				return null;
			}
		} finally {
			reader.Close ();
			filestream.Close ();
			filestream.Dispose ();
		}
		return toReturn;
	}

	/*
	public DialogueHead DeserializeXMLDialogueLinq(TextAsset xmlDialogue){
		string xmlString = xmlDialogue.text;

		Assembly assem = Assembly.GetExecutingAssembly ();
		XDocument xDoc = XDocument.Load (new StreamReader (xmlString));

		DialogueHead dialogue = new DialogueHead();
		dialogue.NPCName = xDoc.Element("NPCName").Value;
		dialogue.FirstLineId = Int32.Parse(xDoc.Element("FirstLineId").Value);

		dialogue.dialogueElements = xDoc.Descendants("DialogueElement").Select(element => {
			string typeName = element.Attribute("Type").Value;
			var type = assem.GetTypes().Where(t => t.Name == typeName).First();
			DialogueElement e = Activator.CreateInstance(type) as DialogueElement;
			foreach(var property in element.Descendants()){
				type.GetProperty(property.Name.LocalName).SetValue(e, property.Value, null);
			}
			return e;
		}).ToArray();

		return dialogue;
	}
	*/

	public DialogueHead DeserializeXMLDialogueLinq(string xmlDialoguePath){
		//string xmlString = xmlDialogue.text;

		Assembly assem = Assembly.GetExecutingAssembly ();
		XDocument xDoc = XDocument.Load (UnityEngine.Application.dataPath + xmlDialoguePath);

		DialogueHead dialogue = new DialogueHead();
		//Debug.Log (xDoc.Element("NPCName").Value);
		dialogue.NPCName =  xDoc.Descendants("NPCName").First().Value;
		dialogue.FirstLineId = Int32.Parse(xDoc.Descendants("FirstLineId").First().Value);

		dialogue.dialogueElements = xDoc.Descendants("DialogueElement").Select(element => {
			string typeName = element.Attribute("Type").Value;
			var type = assem.GetTypes().Where(t => t.Name == typeName).First();
			DialogueElement e = Activator.CreateInstance(type) as DialogueElement;
			Debug.Log("pass 0");
			foreach(var property in element.Descendants()){
				if(property.Name != "value"){
					var setProp = type.GetProperty(property.Name.LocalName);
					if(setProp.PropertyType.IsArray){
						Debug.Log("Pass 1");
						if(setProp.Name.Contains("Id")){
							int[] i = property.Descendants("value").Select(v => {return Int32.Parse(v.Value);}).ToArray();
							Debug.Log(i);
							setProp.SetValue(e, i, null);
						}
						else {
							string[] s = property.Descendants("value").Select(v => v.Value).ToArray();
							setProp.SetValue(e, s, null);
							Debug.Log("Pass 2");
						}
					}
					else {
						if(setProp.Name.Contains("Id")){
							setProp.SetValue(e, Int32.Parse(property.Value), null);
						}
						else {
							setProp.SetValue(e, property.Value, null);
						}
					}
				}
			}
			return e;
		}).ToArray();

		return dialogue;
	}
}

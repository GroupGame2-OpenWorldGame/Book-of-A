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
	Dialogue,
	QuestMenu
}

//TODO: Set Cursor LockStates later

public class GameDriver : MonoBehaviour {

	private static GameDriver instance;


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

	[Header("Quests")]
	public UIQuestMenu questMenu;
	public string questListPath;

	[Header("Testing")]
	public string playerName = "Mel";
	public bool branchCondition = false;

	//TESTING VARIABLES
	public bool dialogueHit = false;

	private Button[] optionButtons;
	private int numOptions;
	private int selectedButton = 0;

	//private Dictionary<string, bool> flags;
	private List<string> flags;
	private Dictionary<string, int> counters;

	private NPCScript npcTarget;
	//private Dialogue currentDialogue;
	//private int currentLine = 0;
	private DialogueHead currentDialogue;
	private DialogueElement currentLine;
	private PlayerController player;

	private QuestList questList;
	private List<Quest> questsUnlocked;
	private List<Quest> questsInProgress;
	private List<Quest> questsCompleted;

	public static GameDriver Instance{
		get{
			if (instance == null) {
				instance = FindObjectOfType<GameDriver> ();
				if (instance == null) {
					var newGD = new GameObject ();
					newGD.name = "GameDriver";
					instance = newGD.AddComponent<GameDriver> ();
					DontDestroyOnLoad (instance.gameObject);
				}
			}
			return instance;
		}
	}

	// Use this for initialization
	void Awake () {
		Cursor.visible = false;
		if (instance != null) {
			Destroy (this.gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (this.gameObject);
			flags = new List<string> ();
			counters = new Dictionary<string, int> ();
			//flags = new Dictionary<string, bool>();

			//for( int i = 0; i < flagNames.Length; i++){
				//flags.Add(flagNames[i], false);
			//}

			optionButtons = optionsBox.GetComponentsInChildren<Button> ();
			CleanUpOptions ();
			dialogueBox.SetActive (false);

			questList = DeserializeQuestListLinq (questListPath);
			questsUnlocked = new List<Quest> ();
			Debug.Log ("QU SET");
			questsInProgress = new List<Quest> ();
			questsCompleted = new List<Quest> ();
		}

		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerController> ();
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

	public List<Quest> QuestsUnlocked{
		get{ return questsUnlocked; }
	}

	/*
	public int DialogueLength{
		get{ return currentDialogue.Length; }
		set { currentDialogue.Length = value; }
	}
	*/

	public void OpenQuestMenu(){
		//player.SetMovement (false);
		Time.timeScale = 0f;
		gameState = GameState.QuestMenu;
		questMenu.gameObject.SetActive (true);
		questMenu.SetQuestPanels (0);
	}

	public void CloseQuestMenu(){
		//player.SetMovement (true);
		Time.timeScale = 1f;
		gameState = GameState.OverWorld;
		questMenu.gameObject.SetActive (false);
	}

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
			if (currentLine.SetWhenDone != null && currentLine.SetWhenDone.Length != 0) {
				SetCheck (currentLine.SetWhenDone, currentLine.SetType);
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

	public void SetCheck(string[] thingsToSet, string[] setTypes){
		for (int i = 0; i < thingsToSet.Length; i++) {
			if (setTypes [i].StartsWith ("quest", StringComparison.OrdinalIgnoreCase)) {
				SetQuest (thingsToSet [i], setTypes [i].Split (',') [1], setTypes [i].Split (',') [2]);
			} else if (setTypes [i].StartsWith ("flag", StringComparison.OrdinalIgnoreCase)) {
				SetFlag (thingsToSet [i], setTypes [i].Split (',') [1]);
			} else if (setTypes [i].StartsWith ("object", StringComparison.OrdinalIgnoreCase)) {
				SetObject (thingsToSet [i], setTypes [i].Split (',') [1]);
			} else if (setTypes [i].StartsWith ("counter", StringComparison.OrdinalIgnoreCase)) {
				SetCounter (thingsToSet [i], setTypes [i].Split (',') [1], setTypes [i].Split (',') [2]);
			}
		}
	}

	public void SetFlag(string flagToSet, string setTo){
		if (string.Equals ("true", setTo, StringComparison.InvariantCultureIgnoreCase)) {
			if (!flags.Contains (flagToSet)) {
				flags.Add (flagToSet);
			}
			//flags [flagToSet] = true;
		} else {
			if (flags.Contains (flagToSet)) {
				flags.Remove(flagToSet);
			}
		}
	}

	public void SetCounter(string counterToSet, string setType, string value){
		if (!counters.ContainsKey (counterToSet)) {
			counters.Add (counterToSet, 0);
		}
		if (setType.Equals ("Add", StringComparison.OrdinalIgnoreCase)) {
			counters [counterToSet] += Int32.Parse(value);
		} else if (setType.Equals ("Sudtract", StringComparison.OrdinalIgnoreCase)) {
			counters [counterToSet] -= Int32.Parse(value);
		}else if (setType.Equals ("Set", StringComparison.OrdinalIgnoreCase)) {
			counters [counterToSet] = Int32.Parse(value);
		}

		Debug.Log (counterToSet + " = " + counters [counterToSet]);
	}

	public void SetQuest(string name, string setType, string description){
		foreach (Quest q in questList.Quests) {
			if (q.Name == name) {
				if (description != "") {
					q.Description = description;
				}
				if (setType == "InProgress") {
					if (q.Status != "InProgress") {
						QuestsUnlocked.Add (q);
						q.Status = "InProgress";
						if (q.SetWhenStart != null && q.SetWhenStart.Length != 0) {
							SetCheck (q.SetWhenStart, q.SetStartType);
						}
					}
					return;
				} else if (setType == "Failed"){
					if (questsUnlocked.Contains (q)) {
						questsUnlocked.Add (q);
					}
					questsCompleted.Add(q);
					q.Status = "Failed";
					if (q.SetWhenFailed != null && q.SetWhenFailed.Length != 0) {
						SetCheck (q.SetWhenFailed, q.SetFailedType);
					}
					return;
				}else if(setType == "Success"){
					questsCompleted.Add(q);
					q.Status = "Success";
					if (q.SetWhenPassed != null && q.SetWhenPassed.Length !=0) {
						SetCheck (q.SetWhenPassed, q.SetPassedType);
					}
				}
				return;
			}
		}
		Debug.Log ("ERROR: Quest not found");
		return;
	}

	public void SetObject(string name, string setType){
		if (GameObject.Find (name).GetComponent<QuestObject> () == null) {
			Debug.LogError("Quest object '" + name + "' not found.");
		} else{
			QuestObject qO = GameObject.Find (name).GetComponent<QuestObject> ();

			if(setType.Equals("Active",StringComparison.OrdinalIgnoreCase)){
				qO.SetActive(true);
			} else if(setType.Equals("Inactive",StringComparison.OrdinalIgnoreCase)){
				qO.SetActive(false);
			}
		}
	}
		
	public bool CheckConditions(string[] thingsToCheck, string checkType){
		if (checkType.Equals ("OR")) {
			return IsOneConditionTrue (thingsToCheck);
		} else if (checkType.Equals ("AND")) {
			return AreAllConditionsTrue (thingsToCheck);
		}
		return false;
	}

	public bool IsConditionTrue(string condition){
		string conType = condition.Split (',') [0];
		if (conType.Equals ("flag", StringComparison.OrdinalIgnoreCase)) {
			return IsFlagTrue (condition.Split (',') [1]);
		} else if (conType.Equals ("counter", StringComparison.OrdinalIgnoreCase)) {
			return CheckCounter (condition.Split (',') [1], condition.Split (',') [2]);
		}
		return false;
	}

	public bool IsFlagTrue(string flagToCheck){
		if (flagToCheck.StartsWith("!")) {
			return !(flags.Contains(flagToCheck.Split('!')[1]));
		}
		return flags.Contains(flagToCheck);
	}

	public bool CheckCounter(string counterName, string compare){
		if (!counters.ContainsKey (counterName)) {
			counters.Add (counterName, 0);
		}

		if(compare.StartsWith("==")){
			return  (counters [counterName] == Int32.Parse (compare.Split ('=') [2]));
		} else if(compare.StartsWith("!=")){
			return  (counters [counterName] != Int32.Parse (compare.Split ('=') [1]));
		} else if(compare.StartsWith(">=")){
			return  (counters [counterName] >= Int32.Parse (compare.Split ('=') [1]));
		} else if(compare.StartsWith("<=")){
			return  (counters [counterName] <= Int32.Parse (compare.Split ('=') [1]));
		} else if(compare.StartsWith(">")){
			return  (counters [counterName] > Int32.Parse (compare.Split ('>') [1]));
		} else if(compare.StartsWith("<")){
			return  (counters [counterName] < Int32.Parse (compare.Split ('<') [1]));
		} 

		return false;
	}

	public bool IsOneConditionTrue(string[] thingsToCheck){
		foreach (string c in thingsToCheck) {
			Debug.Log ("Checking " + c);
			if (IsConditionTrue (c)) {
				return true;
			}
		}
		return false;
	}

	//array version
	public bool AreAllConditionsTrue(string[] thingsToCheck){
		foreach (string c in thingsToCheck) {
			Debug.Log ("Checking " + c);
			if (!IsConditionTrue (c)) {
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
		TextAsset xmlDialogueAsset = (TextAsset)Resources.Load (xmlDialoguePath);
		XDocument xDoc = XDocument.Parse(xmlDialogueAsset.text);

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

	public QuestList DeserializeQuestListLinq(string xmlQuestListPath){
		//string xmlString = xmlDialogue.text;

		Assembly assem = Assembly.GetExecutingAssembly ();
		TextAsset xmlDialogueAsset = (TextAsset)Resources.Load (xmlQuestListPath);
		XDocument xDoc = XDocument.Parse(xmlDialogueAsset.text);

		QuestList qL = new QuestList ();
		qL.Quests = xDoc.Descendants("Quest").Select(element => {
			var type = assem.GetTypes().Where(t => t.Name == "Quest").First();
			Quest e = new Quest();
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

		return qL;
	}
}

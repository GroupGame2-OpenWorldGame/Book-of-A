using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Linq;
using System.Xml.Linq;

[System.Serializable]
public class DialogueHead {

	[XmlElement]
	public string NPCName {
		get ;
		set ;
	}
		
	public int FirstLineId {
		get;
		set;
	}
		
	public DialogueElement[] dialogueElements;

	/*
	public void SetHeadToLines(){
		foreach (DialogueElement ele in dialogueElements) {
			ele.SetHead (this);
		}
	}
	*/

	/*
	public void SetLines(){
		FirstLine.SetLine ();
		IdsSet.Clear ();
	}
	*/

	public DialogueElement FindLineById(int id){
		if (id == 0) {
			return null;
		}
		foreach (DialogueElement ele in dialogueElements) {
			if (ele.Id == id) {
				return ele;
			}
		}
		return null;
	}
}


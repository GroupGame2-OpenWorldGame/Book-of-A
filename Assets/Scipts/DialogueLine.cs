using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

public class DialogueLine : DialogueElement {

	public string Speaker {
		get;
		set;
	}

	public string Text {
		get;
		set;
	}
		
	public int NextLineId {
		get;
		set ;
	}
}

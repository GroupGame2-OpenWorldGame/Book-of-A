using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

public class DialogueOption : DialogueElement {

	public string Speaker {
		get;
		set;
	}

	public string Text {
		get;
		set;
	}

	public string[] Options {
		get;
		set;
	}

	public int[] DecisionLineIds {
		get;
		set;
	}
}

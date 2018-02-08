using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

public class DialogueIfBranch : DialogueElement {

	[XmlElement]
	public string CheckType {
		get ;
		set ;
	}

	public string[] ConditionsToCheck {
		get;
		set;
	}
		
	public int TrueLineId {
		get;
		private set;
	}

	public int FalseLineId {
		get;
		private set;
	}
}

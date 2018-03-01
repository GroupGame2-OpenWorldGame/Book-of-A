using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Linq;


public abstract class DialogueElement {

	[XmlElement]
	public int Id {
		get ;
		set ;
	}

	public string[] SetWhenDone {
		get;
		set;
	}

	public string[] SetType {
		get;
		set;
	}

	public bool TriggerPassed {
		get;
		set;
	}
}

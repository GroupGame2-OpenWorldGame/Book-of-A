using System.Collections;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Linq;

[System.Serializable]
public class Quest {

	[XmlElement]
	public string Name {
		get ;
		set ;
	}

	public string Description {
		get;
		set;
	}

	public string Status {
		get;
		set;
	}

	public string[] SetWhenStart {
		get;
		set;
	}

	public string[] SetStartType {
		get;
		set;
	}

	public string[] SetWhenPassed {
		get;
		set;
	}

	public string[] SetPassedType {
		get;
		set;
	}

	public string[] SetWhenFailed {
		get;
		set;
	}

	public string[] SetFailedType {
		get;
		set;
	}
}

using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;


public class Dialogue {

	[XmlAttribute]
	public int Id {
		get ;
		private set ;
	}


	public string Speaker {
		get ;
		private set;
	}


	public int Length {
		get ;
		set;
	}

	[XmlArray("Speech")]
	[XmlArrayItem("Line")]
	public List<string> Speech {
		get ;
		set ; 
	}

	public Dialogue(){
	}
}

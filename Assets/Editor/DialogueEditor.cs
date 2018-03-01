using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml.Linq;
using UnityEngine;
using UnityEditor;

public class DialogueEditor : EditorWindow {

	[MenuItem("Window/Dialogue Editor")]

	public static void ShowWindow(){
		EditorWindow.GetWindow (typeof(DialogueEditor));
	}

	void OnGui(){
	}
}

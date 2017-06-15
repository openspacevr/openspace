//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class DeviceLog : MonoBehaviour {
//
//	private List<string> debugLogs = new List<string>();
//	private int logIndex;
//
//	public void DebugLog(string logmsg){
//		debugText.text = logmsg;
//		debugLogs.Add (logmsg);
//	}
//
//	public void DebugLog(int alert, string logmsg){
//		string alertmsg;
//		switch (alert) {
//		case 0:
//			alertmsg = "<color=red>Fail! </color>";
//			debugText.text = alertmsg + logmsg;
//			debugLogs.Add (alertmsg + logmsg);
//			Debug.Log (alertmsg + logmsg);
//			break;
//		case 1:
//			alertmsg = "<color=green> Success! </color>";
//			debugText.text = alertmsg + logmsg;
//			debugLogs.Add (alertmsg + logmsg);
//			Debug.Log (alertmsg + logmsg);
//			break;
//		}
//	}
//
//	public void DebugButton(string button){
//
//		string previous = "previous";
//		string next = "next";
//		string last = "last";
//		int i = logIndex;
//
//		if (button == previous) {
//			if (logIndex > 0) 
//				i -= 1;
//		} else if (button == next) {
//			if (logIndex < debugLogs.Count)
//				i += 1;
//		} else if (button == last) {
//			i = debugLogs.Count;
//		} else {
//			DebugLog (0, "An incorrect int is being passed into DebugButton mehtod.");
//		}
//
//		DebugLog (debugLogs [i]);
//		Debug.Log (button + i + logIndex + debugLogs [i]);
//	}
//}

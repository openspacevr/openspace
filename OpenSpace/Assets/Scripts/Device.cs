using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Device : MonoBehaviour {

	public static Device device;
	public bool debug = true; 
	[SerializeField]
	private Text debugText;
	[SerializeField]
	private List<string> debugLogs = new List<string>();
	[SerializeField] private int logIndex;
	private bool showButtons = false;
	private GameObject debugPanel;
	private RectTransform buttonRect;
	private Vector3 buttonVect;

	void Start(){
		debugPanel = GetComponentInChildren<LayoutElement> ().gameObject;
		buttonRect = debugPanel.GetComponent<RectTransform> ();
		debugPanel.SetActive (false);
		debugLogs.Add ("Device.Log has intilialized.");
		debugLogs.Add ("Filling belly of Device.Log.");
		device = this;
	}

	public void Log(string logmsg){
		debugText.text = logmsg;
		debugLogs.Add (logmsg);
		logIndex++;
	}

	public void Log(int alert, string logmsg){
		string alertmsg;
		switch (alert) {
		case 0:
			alertmsg = "<color=red>Fail! </color>";
			debugText.text = alertmsg + logmsg;
			debugLogs.Add (alertmsg + logmsg);
			Debug.Log (alertmsg + logmsg);
			logIndex++;
			break;
		case 1:
			alertmsg = "<color=green> Success! </color>";
			debugText.text = alertmsg + logmsg;
			debugLogs.Add (alertmsg + logmsg);
			Debug.Log (alertmsg + logmsg);
			logIndex++;
			break;
		}
	}
		
	public void ButtonPrevious(){
		if (logIndex == 0) {
			return;
		}
		logIndex--;
		Debug.Log (debugLogs [logIndex]);
		debugText.text = debugLogs [logIndex];
	}

	public void ButtonNext(){
		if (logIndex == debugLogs.Count -1) {
			return;
		}
		logIndex++;
		debugText.text = debugLogs [logIndex];
		Debug.Log (debugLogs [logIndex]);
	}

	public void ButtonLast(){
		logIndex = debugLogs.Count -1;
		debugText.text = debugLogs [logIndex];
		Debug.Log (debugLogs [logIndex]);
	}

	public void ShowButtons(){
		if (!showButtons) {
			showButtons = true;
			buttonVect.y = 70f;
			debugPanel.SetActive (true);
		} else {
			buttonVect.y = 0f;
			debugPanel.SetActive (false);
			showButtons = false;
		}
		buttonRect.localPosition = buttonVect;
	}
}

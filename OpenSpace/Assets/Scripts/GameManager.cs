using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VR;
using Firebase;
using Firebase.Auth;

public class GameManager : MonoBehaviour {

	public static GameManager instanceGameManager;
	[SerializeField] public Registration instanceRegistration;
	[SerializeField] public Database instanceDatabase;
	[SerializeField] public RandomMatchmaker instanceRandomMatchmaker;

	[SerializeField] public string version;
	private DeviceOrientation myOrientation;
	[SerializeField] private Text debugText;
	[SerializeField] private bool debug = true; 


	public enum GameState 
	{
		Logo,
		Registering,
		Registered,
		Connecting,
		Connected,
		Disconected
	};

	private GameState gameState = GameState.Logo;

	// Use this for initialization
	void Start () {
		instanceGameManager = this;
		DontDestroyOnLoad (this);
		DisableVr ();
	}

	void Update(){

		if (debug) {
			debugText.text = "Photon: " + PhotonNetwork.connectionStateDetailed.ToString () + " VR Device: " + VRDevice.model;
		}

		if (myOrientation == DeviceOrientation.Portrait | myOrientation == DeviceOrientation.PortraitUpsideDown) {
			VRSettings.enabled = false;
		} else {
			if (VRSettings.enabled == false) {
				VRSettings.enabled = true;
			}
		}
	}
		
	IEnumerator LoadDevice(string newDevice, bool enable)
	{
		VRSettings.LoadDeviceByName(newDevice);
		yield return null;
		VRSettings.enabled = enable;
	}

	void EnableVr()
	{
		StartCoroutine(LoadDevice("daydream", true));
	}

	void DisableVr() {
		StartCoroutine(LoadDevice("", false));
	}

	public void StateManager(GameState state){

		switch (state) {
		case GameState.Logo:
			break;
		case GameState.Registering:
			break;
		case GameState.Registered:
			break;
		case GameState.Connecting:
			break;
		case GameState.Connected:
			break;
		}
	}

}

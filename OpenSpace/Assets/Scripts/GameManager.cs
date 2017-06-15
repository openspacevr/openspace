using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

	public FirebaseAuth auth;
	Firebase.Auth.FirebaseUser user;
	public User localUser;
	private string displayName = "";


	public string version;
	DeviceOrientation myOrientation;




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
		InitializeFirebase ();
	}

	// Handle initialization of the necessary firebase modules:
	void InitializeFirebase() {
		Device.device.Log("Setting up Firebase Auth");
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		auth.StateChanged += AuthStateChanged;
		AuthStateChanged(this, null);
	}

	// Track state changes of the auth object.
	void AuthStateChanged(object sender, System.EventArgs eventArgs) {
		if (auth.CurrentUser != user) {
			bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
			if (!signedIn && user != null) {
				Device.device.Log("Signed out " + user.UserId);
			}
			user = auth.CurrentUser;
			if (signedIn) {
				Device.device.Log("Signed in " + user.UserId);
				displayName = user.DisplayName ?? "";
				DisplayUserInfo(user, 1);
				Device.device.Log("  Anonymous: " + user.IsAnonymous);
				Device.device.Log("  Email Verified: " + user.IsEmailVerified);
				var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
				if (providerDataList.Count > 0) {
					Device.device.Log("  Provider Data:");
					foreach (var providerData in user.ProviderData) {
						DisplayUserInfo(providerData, 2);
					}
				}
			}
		}
	}

	// Display user information.
	void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel) {
		string indent = new String(' ', indentLevel * 2);
		var userProperties = new Dictionary<string, string> {
			{"Display Name", userInfo.DisplayName},
			{"Email", userInfo.Email},
			{"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
			{"Provider ID", userInfo.ProviderId},
			{"User ID", userInfo.UserId}
		};
		foreach (var property in userProperties) {
			if (!String.IsNullOrEmpty(property.Value)) {
				Debug.Log(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
			}
		}
	}




		
	void Update(){

//		if (debug) {
//			debugText.text = "Photon: " + PhotonNetwork.connectionStateDetailed.ToString () + " VR Device: " + VRDevice.model;
//		}

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

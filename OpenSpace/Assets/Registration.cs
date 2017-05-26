using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;
using Firebase;
using Firebase.Auth;
public class Registration : MonoBehaviour {

	private DeviceOrientation myOrientation;
	FirebaseAuth auth;
	private string email = "undefined";
	private string password = "undefined";

	enum UserFlow {Choice, SignUp, SignIn, Complete};
	UserFlow sessionFlow;

	[SerializeField]
	private GameObject choicePanel;
	[SerializeField]
	private GameObject signUpPanel;
	[SerializeField]
	private GameObject signInPanel; 


	// Use this for initialization
	void Start () {
		//The FirebaseAuth class is the gateway for all API calls. 
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		//DisablingVr mode in order to allow user register through sign up form.
		DisableVr ();

		sessionFlow = UserFlow.Choice;
		signUpPanel.SetActive (false);
		signInPanel.SetActive (false);
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

	void DisableVr()
	{
		StartCoroutine(LoadDevice("", false));
	}
		
	public void SetEmail(string emailInput){
		email = emailInput;
	}

	public void SetPassword(string passwordInput){
		password = passwordInput;
	}
		
	public void SignUp(){
		

		auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
				return;
			}

			// Firebase user has been created.
			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("Firebase user created successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
		});

		Debug.Log ("<color=green>Register: </color>" + email + " " + password);
		SignIn ();
	}

	public void SignIn(){

		auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
		if (task.IsCanceled) {
			Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
			return;
		}
		if (task.IsFaulted) {
			Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
			return;
		}

		Firebase.Auth.FirebaseUser newUser = task.Result;
		Debug.LogFormat("User signed in successfully: {0} ({1})",
			newUser.DisplayName, newUser.UserId);
	});
	
	}

	public void SignInButton(){
		sessionFlow = UserFlow.SignIn;
		choicePanel.SetActive (false);
		signInPanel.SetActive (true);
	}

	public void SignUpBotton(){
		sessionFlow = UserFlow.SignUp;
		choicePanel.SetActive (false);
		signUpPanel.SetActive (true);
	}
		
	public void OnClick(){
		
		TouchScreenKeyboard.hideInput = true;
		TouchScreenKeyboard.Open ("email", TouchScreenKeyboardType.Default);
	}
		
	void Update(){

		if (myOrientation == DeviceOrientation.Portrait | myOrientation == DeviceOrientation.PortraitUpsideDown) {
			VRSettings.enabled = false;
		} else {
			if (VRSettings.enabled == false) {
				VRSettings.enabled = true;
			}
		}
	}
}
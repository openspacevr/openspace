using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;

public class Registration : MonoBehaviour {

	FirebaseAuth auth;
	DatabaseReference dbReference;
	private string email = "undefined";
	private string password = "undefined";
	private GameManager instanceGameManager;

	MentalHealth mentalHealthInstance;
	GameObject[] buttons;
	private int count = 0;

	enum UserFlow {Choice, SignUp, SignIn, Complete};
	UserFlow sessionFlow;

	//	List containing our panels: 0 choicePanel 1 signUpPanel 2 signInPanel 3 problemsPanel 4 inGamePanel
	//	5 connectPanel
	[SerializeField] private List<GameObject> panels = new List<GameObject> (6);
	[SerializeField] private List<string> symptoms = new List<string> ();
	[SerializeField] private GameObject button;
	// Use this for initialization
	void Start () {

		instanceGameManager = GameManager.instanceGameManager;

		mentalHealthInstance = ScriptableObject.CreateInstance<MentalHealth>();
		buttons = new GameObject[mentalHealthInstance.problems.Length];
		dbReference = FirebaseDatabase.DefaultInstance.RootReference;

		//The FirebaseAuth class is the gateway for all API calls. 
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		//DisablingVr mode in order to allow user register through sign up form.
		sessionFlow = UserFlow.Choice;
		panels[1].SetActive (false);
		panels[2].SetActive (false);
		panels[3].SetActive (false);
	}

	void SetSymtoms(){

		for (int i = 0; i < mentalHealthInstance.problems.Length; i++) {
			button = Instantiate (button,panels[3].transform);
			button.GetComponentInChildren<Text> ().text = mentalHealthInstance.problems [i];
			button.SetActive (false);
			buttons [i] = button;
		}
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
			GameManager.instanceGameManager.StateManager(GameManager.GameState.Registered);

			GenerateIDToken ();
	});
			
	
	}
		
	private void GenerateIDToken(){
		
		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		user.TokenAsync(true).ContinueWith(task => {
			if (task.IsCanceled) {
				Debug.LogError("TokenAsync was canceled.");
				return;
			}

			if (task.IsFaulted) {
				Debug.LogError("TokenAsync encountered an error: " + task.Exception);
				return;
			}

			string idToken = task.Result;
			Debug.Log("<color=green> Success! </color>"+idToken);
			Debug.Log("<color=green> Success! </color>"+ "Firebase Current UserId: " + auth.CurrentUser.UserId+ "Firebase UserId: " + user.UserId);

			writeNewUser(email, user.UserId, symptoms);
		});
	}

	private void writeNewUser(string email, string uid, List<string> symptoms) {

		User user = new User(email, uid, symptoms);
		string json = JsonUtility.ToJson(user);

		FirebaseDatabase.DefaultInstance.GetReference ("users").Child(uid).SetRawJsonValueAsync (json).ContinueWith (task => {
			if(task.IsFaulted){
				//Handle Error
				Debug.Log("<color=red> Error! </color>" + " Sending user info failed.");

			}else if(task.IsCompleted){
				//Resend Data or ask to resend.
				Debug.Log("<color=green> Success! </color>" + " Sending user info complete!");
			}
		});
	}

	public void ShowSymptoms(){

		int nob = 5;

		panels[3].SetActive (true);

		for (int i = count; i < count + nob; i++) {
			buttons [i].SetActive (true);
		}

		count += nob;
		if (count == mentalHealthInstance.problems.Length)
			count = 0;
		Debug.Log (count);
	}

	public void ShowMoreSymptoms(){

		for (int i = count; i < count; i++) {
			buttons [i].SetActive (true);
		}
	}

	public void HideSymptoms(){
		panels[3].SetActive (false);
	}

	public void AddSymptom(){
		string symptom;
		symptom = GetComponentInChildren<Text> ().text;
		symptoms.Add (symptom);
		Debug.Log (symptom);
	}

	public void SignInButton(){
		sessionFlow = UserFlow.SignIn;
		panels[0].SetActive (false);
		panels[2].SetActive (true);
	}

	public void SignUpBotton(){
		sessionFlow = UserFlow.SignUp;
		panels[0].SetActive (false);
		panels[1].SetActive (true);
	}

	public void ConnectButton(){
		instanceGameManager.instanceRandomMatchmaker.Connect (instanceGameManager.version);
	}
		
	public void OnClick(){

		TouchScreenKeyboard.Open ("email", TouchScreenKeyboardType.EmailAddress);
		TouchScreenKeyboard.hideInput = true;
	}
		


}
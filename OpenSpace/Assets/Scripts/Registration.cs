using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;
//	This class handles the registration of the u
public class Registration : MonoBehaviour {

	FirebaseAuth auth;
	DatabaseReference dbReference;
	GameManager instanceGameManager;
	MentalHealth instanceMentalHealth;
	int count;
	private string email = "undefined";
	private string password = "undefined";

	// 0 choicePanel 1 signUpPanel 2 signInPanel 3 problemsPanel 4 inGamePanel 5 connectPanel
	[SerializeField] private List<GameObject> panels = new List<GameObject> ();
	[SerializeField] private List<string> symptoms = new List<string> ();
	[SerializeField] private GameObject button;
	[SerializeField] private List<GameObject> buttons = new List<GameObject> ();


	// Use this for initialization
	void Start () {
		//Class References
		instanceGameManager = GameManager.instanceGameManager;
		instanceMentalHealth = ScriptableObject.CreateInstance<MentalHealth>();
		dbReference = FirebaseDatabase.DefaultInstance.RootReference;

		auth = instanceGameManager.auth;
		//Deactivating ui panels.
		foreach(GameObject p in panels){
			p.SetActive(false);
		}
		panels [0].SetActive (true);

	}


	void ShowPanel(GameObject panel){
		foreach(GameObject p in panels){
			p.SetActive(false);
		}
		panel.SetActive (true);
	}

	void SetSymtoms(){

		for (int i = 0; i < instanceMentalHealth.problems.Length; i++) {
			button = Instantiate (button,panels[3].transform);
			button.GetComponentInChildren<Text> ().text = instanceMentalHealth.problems [i];
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
				Device.device.Log(0,"CreateUserWithEmailAndPasswordAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Device.device.Log(0,"CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
				return;
			}

			// Firebase user has been created.
			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("Firebase user created successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
			instanceGameManager.localUser = WriteNewUser(email, newUser.UserId, symptoms);
			Device.device.Log(instanceGameManager.localUser.symptoms[0]);

		});

		Device.device.Log(1,email + " " + password);

		SignIn ();
	}

	public void SignIn(){

		auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
		if (task.IsCanceled) {
				Device.device.Log(0,"SignInWithEmailAndPasswordAsync was canceled.");
			return;
		}
		if (task.IsFaulted) {
				Device.device.Log(0,"SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
			return;
		}

		Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("User signed in successfully: {0} ({1})",
			newUser.DisplayName, newUser.UserId);
			Device.device.Log("User signed in successfully: {0} ({1})" +
				newUser.DisplayName + newUser.UserId);
			Device.device.Log(1, "User signed in succesfully");
			GenerateIDToken ();
	});
		panels [5].SetActive (true);
	
	}
		
	private void GenerateIDToken(){
		
		Firebase.Auth.FirebaseUser user = auth.CurrentUser;
		user.TokenAsync(true).ContinueWith(task => {
			if (task.IsCanceled) {
				Device.device.Log(0,"TokenAsync was canceled.");
				return;
			}

			if (task.IsFaulted) {
				Device.device.Log(0,"TokenAsync encountered an error: " + task.Exception);
				return;
			}

			string idToken = task.Result;
			Device.device.Log(1,idToken);
			Device.device.Log(1, "Firebase Current UserId: " + auth.CurrentUser.UserId+ "Firebase UserId: " + user.UserId);

		});
	}

	private User WriteNewUser(string email, string uid, List<string> symptoms) {

		User user = new User(email, uid, symptoms);
		string json = JsonUtility.ToJson(user);

		FirebaseDatabase.DefaultInstance.GetReference ("users").Child(uid).SetRawJsonValueAsync (json).ContinueWith (task => {
			if(task.IsFaulted){
				//Handle Error
				Device.device.Log(0, " Sending user info failed.");

			}else if(task.IsCompleted){
				//Resend Data or ask to resend.
				Device.device.Log(1," Sending user info complete!");
			}
		});

		return user;

	}

	public void ShowSymptoms(){

		int nob = 5;

		panels[3].SetActive (true);

		for (int i = count; i < count + nob; i++) {
			buttons [i].SetActive (true);
		}

		count += nob;
		if (count == instanceMentalHealth.problems.Length)
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
		panels[0].SetActive (false);
		panels[2].SetActive (true);
	}

	public void SignUpBotton(){
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
using System.Collections.Generic;
using UnityEngine;

public class User {
	public string email;
	public string uid;
	public bool verified;
	public List<string> symptoms;

	public User() {
		
	}

	public User(string email, string uid, List<string> symptoms) {
		this.symptoms = new List<string> ();
		this.email = email;
		this.uid = uid;
		//This will be updated when the user verifies their email.
		this.verified = false;
		for (int i = 0; i < symptoms.Capacity; i++) {
			this.symptoms.Add (symptoms [i]);
			Debug.Log (this.symptoms [i]);
		}

	}
}
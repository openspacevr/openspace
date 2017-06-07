using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine;

public class Database: MonoBehaviour {

	public DatabaseReference reference;

	void Start() {
		// Set up the Editor before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://openspace-2418d.firebaseio.com/");
		FirebaseApp.DefaultInstance.SetEditorP12FileName ("OpenSpace-1720969c8c15.p12");
		FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail ("admin-227@openspace-2418d.iam.gserviceaccount.com");
		FirebaseApp.DefaultInstance.SetEditorP12Password ("notasecret");

		// Get the root reference location of the database.
		reference = FirebaseDatabase.DefaultInstance.RootReference;
	}
}

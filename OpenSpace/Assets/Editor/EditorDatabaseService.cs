
using UnityEditor;
using Firebase;
using Firebase.Unity.Editor;

public class EditorDatabaseService: Editor {

	void Start ()
	{
		// Set these values before calling into the realtime database.
		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://openspace-2418d.firebaseio.com/");
		FirebaseApp.DefaultInstance.SetEditorP12FileName ("OpenSpace-1720969c8c15.p12");
		FirebaseApp.DefaultInstance.SetEditorServiceAccountEmail ("admin-227@openspace-2418d.iam.gserviceaccount.com");
		FirebaseApp.DefaultInstance.SetEditorP12Password ("notasecret");

	}
}
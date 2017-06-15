using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class CustomMatchmaker : Photon.PunBehaviour {

	[SerializeField]
	private Transform spawnPoint;
	[SerializeField] private GameManager instanceGameManager;
	private bool createRoomOperation;
	private RoomOptions roomOptions;

	void Start(){
		PhotonNetwork.logLevel = PhotonLogLevel.Full;

	}

	public void Connect(string version){
		PhotonNetwork.ConnectUsingSettings (version);
	}

	//You don't need to call the base implementation of pun callbacks.
	public override void OnJoinedLobby()
	{
		Device.device.Log (1,"Photon joined a Lobby.");
		roomOptions = new RoomOptions ();
		string[] lobbyProperties = new string[instanceGameManager.localUser.symptoms.Count];
		for(int i = 0; i < lobbyProperties.Length;i++){
			lobbyProperties[i] = instanceGameManager.localUser.symptoms[i];
		}
		roomOptions.CustomRoomPropertiesForLobby = lobbyProperties;
		roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable ();
		roomOptions.MaxPlayers = 2;
		foreach (string sympt in instanceGameManager.localUser.symptoms) {
			roomOptions.CustomRoomProperties.Add (sympt, true);
			Debug.Log (sympt);
		}
		createRoomOperation = PhotonNetwork.CreateRoom (null, roomOptions, null);
	}
		
	public override void OnCreatedRoom ()
	{
		Room createdRoom;
		createdRoom = PhotonNetwork.room;
		createdRoom.SetCustomProperties (roomOptions.CustomRoomProperties, roomOptions.CustomRoomProperties, false);
		Device.device.Log(1,"Created a room with symptoms: " + instanceGameManager.localUser.symptoms[0]);
		Device.device.Log ("OnCreatedRoom called.");
	}

	public override void OnPhotonCreateRoomFailed (object[] codeAndMsg)
	{
		Device.device.Log ("OnPhotonCreateRoomFailed: " + codeAndMsg.ToString());
	}
	public override void OnJoinedRoom ()
	{
		GameObject player =	PhotonNetwork.Instantiate ("Prefabs/Player", spawnPoint.position, spawnPoint.rotation,0);
		player.GetComponent<NetworkCharacter> ().Init ();
		Device.device.Log("Photon UserId: " + PhotonNetwork.player.userId);

		//+ createdRoom.propertiesListedInLobby.GetValue(0,1)

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class RandomMatchmaker : Photon.PunBehaviour {

	[SerializeField]
	private Transform spawnPoint;

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
		PhotonNetwork.JoinRandomRoom ();
	}

	// Joining a random room fails if no one else is playing or if all rooms are maxed out with players.
	void OnPhotonRandomJoinFailed ()
	{
		Device.device.Log (0,"Photon failed to join a random room.");
		PhotonNetwork.CreateRoom (null);
	}
		
	public override void OnJoinedRoom ()
	{
		GameObject player =	PhotonNetwork.Instantiate ("Prefabs/Player", spawnPoint.position, spawnPoint.rotation,0);
		player.GetComponent<NetworkCharacter> ().Init ();
		Device.device.Log("Photon UserId: " + PhotonNetwork.player.userId);
	}
}

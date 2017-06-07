using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class GameLogic : Photon.PunBehaviour {

	public static int playerWhoIsIt;

	public override void OnJoinedRoom ()
	{
		if (PhotonNetwork.playerList.Length == 1) {
			
		}
	} 
}

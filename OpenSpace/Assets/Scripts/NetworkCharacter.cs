using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkCharacter : MonoBehaviour {

	[SerializeField] private GameObject camera;
	[SerializeField] private GameObject controller;

	public void Init(){
		camera.SetActive (true);
		controller.SetActive (true);
	}

}

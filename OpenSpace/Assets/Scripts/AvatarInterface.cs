using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarInterface : MonoBehaviour {

	public float shape;
	public float originalValue;
	public SkinnedMeshRenderer skm;

	public float floatStrength;

	void Start (){
		skm = GetComponent<SkinnedMeshRenderer> ();
		originalValue = skm.GetBlendShapeWeight (0);
	}

	void Update () {

		shape = skm.GetBlendShapeWeight(0);
		shape = originalValue + (Mathf.Sin(Time.time) * floatStrength);
		skm.SetBlendShapeWeight (5, shape);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bloon : MonoBehaviour {

    public BalloonAffinity Balloon;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<MeshRenderer>().material = Balloon.materials[(int)Balloon.affinity];
	}
	
}

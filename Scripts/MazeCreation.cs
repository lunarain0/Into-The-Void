using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MazeCreation : MonoBehaviour {
    
	// Use this for initialization
	void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag =="Player")
        transform.Find("Enemies").gameObject.SetActive(true);
	}
	
}
    


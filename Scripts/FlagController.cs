using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagController : MonoBehaviour {
    public bool flagged;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            flagged = true;
        }
    }
}

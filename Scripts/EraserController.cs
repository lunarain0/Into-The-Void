using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserController : MonoBehaviour {

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player"){
            gameObject.GetComponent<Animator>().SetBool("PlayerCollided",true);
        }

    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag == "Player")
        {
            gameObject.GetComponent<Animator>().SetBool("PlayerCollided", false);
        }
    }
}

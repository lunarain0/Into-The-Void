using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CraneController : MonoBehaviour {
    bool playerCollision = false;
    bool travelled = false;
    bool initialised = false;
    public GameObject player;

    public void Initialise () {
        if (initialised == false)
        {
            initialised = true;
            gameObject.GetComponent<Animator>().SetTrigger("Initialise");

        }

	}
	

    private void Travel()
    {
        if (travelled == false)
        {
            travelled = true;
            gameObject.GetComponent<Animator>().SetTrigger("Drop");

        }
    }

    void OnTriggerEnter(Collider collision)
    {
        
            if (collision.gameObject.tag == "Player")
            {

            player.transform.SetParent(transform.Find("Floor"),true);
            Travel();
            

            }
	}
    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("SpawnPoint").transform.position = new Vector3 (363.73f, 2617.538f, -198.41f) ;
            player.transform.SetParent(null, true);


        }
    }


}

            
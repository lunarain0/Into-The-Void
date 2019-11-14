using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FallingBuilding : MonoBehaviour {

    bool playerCollision = false;
    bool travelled = false;
    bool initialised = false;
    public GameObject player;
    Tweener tween;

    private void Update()
    {
        if (travelled == false && playerCollision == true)
        {
            travelled = true;
            Travel();
        }

    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            playerCollision = true;
            player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
                player.transform.parent = null;

        }
    }
    void Travel()
    {
        tween = transform.DORotate(new Vector3(25.6f, -51f, 0f), 3f);
        
    }
}


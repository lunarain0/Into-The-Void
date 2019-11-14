using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPlane : MonoBehaviour {
    public GameObject SpawnPoint;
    bool isColliding;
    // Update is called once per frame
    void OnTriggerEnter(Collider collision)
    {

        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (isColliding) return;
        isColliding = true;
        if (collision.gameObject.tag == "Player")
        {
            GameObject player = GameObject.Find("character");
            player.GetComponent<PlayerStats>().Damage(20);
            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            if (player.GetComponent<PlayerStats>().health != 0)
            player.transform.position = SpawnPoint.transform.position;

        }

        
    }

    private void Update()
    {
        isColliding = false;
    }
}

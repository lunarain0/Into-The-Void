using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMover2 : MonoBehaviour
{

    public Vector3 spawnPosition;
    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            GameObject.Find("SpawnPoint").transform.position = spawnPosition;
        }
    }
}

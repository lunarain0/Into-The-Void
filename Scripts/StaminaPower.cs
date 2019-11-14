using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class StaminaPower : MonoBehaviour
{
    private bool collected;
    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.GetComponent<AudioSource>().Play();
            transform.GetComponent<Animator>().SetTrigger("Collected");
            GameObject.Find("character").GetComponent<PlayerStats>().stamina = 1f;
            
        }
    }
}

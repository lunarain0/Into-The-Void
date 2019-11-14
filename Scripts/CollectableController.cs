using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour {
    private bool collected;
    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!collected)
            {
                GameObject.Find("character").GetComponent<PlayerStats>().Heal(20);
                GameObject.Find("character").GetComponent<PlayerStats>().collectedStrawbs += 1;
            }
            transform.GetComponent<AudioSource>().Play();
            transform.GetComponent<Animator>().SetTrigger("Collected");
            
            collected = true;
        }
    }
    void Update () {
		if (collected)
        {
            Destroy(transform.parent.gameObject, 0.5f);
        }
	}
}

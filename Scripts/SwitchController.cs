using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchController : MonoBehaviour {
    
    public GameObject player;
    public GameObject prompt;
    public GameObject crane;
    bool activated = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && activated == false)
        {
            prompt.SetActive(true);
            if (Input.GetButtonDown("Jump"))
            {
                player.GetComponent<PlayerController>().interactable = true;
                activated = true;
                prompt.SetActive(false);
            }
        }
            
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            prompt.SetActive(false);
        }
    }

    void Update()
    {
        if(player.GetComponent<Animator>().GetBool("Interacted") == true)
        crane.GetComponent<CraneController>().Initialise();
    }
}

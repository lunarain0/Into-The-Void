using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReturnTeleporter : MonoBehaviour
{

    public GameObject player;
    public TextMeshProUGUI time;
    public GameObject teleportLocation;
    public GameObject prompt;
    bool activated = false;
    private void Start()
    {
    }
    // Update is called once per frame
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && activated == false)
        {
            prompt.SetActive(true);
            if (Input.GetButtonDown("Jump")){
                player.GetComponent<Animator>().SetTrigger("Teleport");
                activated = true;
                prompt.SetActive(false);
                StartCoroutine(Teleport());
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        
            if (other.gameObject.tag == "Player")
                prompt.SetActive(false);
        
    }
    IEnumerator Teleport()
    {
        yield return new WaitForSeconds(1f);
        player.transform.position = teleportLocation.transform.position;
        try
        {
            time.text = "";
            GameObject.Find("LevelSelection").GetComponent<LevelController>().musicController.Resume();
            GameObject.Find("LevelSelection").GetComponent<LevelController>().musicController.ChangeTrack();
            activated = true;
        }
        catch (System.NullReferenceException e) { Debug.Log("LevelController not found"); };
    }
}

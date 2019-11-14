using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    public GameObject player;
    public GameObject teleportLocation;
    public GameObject prompt;
    bool activated = false;
    public int hintNum;

    // Update is called once per frame
    void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && activated == false)
        {
            prompt.SetActive(true);
            if (Input.GetButtonDown("Jump"))
            {

                player.GetComponent<Animator>().SetTrigger("Teleport");
                activated = true;
                StartCoroutine(Teleport());
                prompt.SetActive(false);
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
            GameObject.Find("LevelSelection").GetComponent<LevelController>().HintDisplay(hintNum);
            GameObject.Find("LevelSelection").GetComponent<LevelController>().musicController.BonusLevel();
            GameObject.Find("LevelSelection").GetComponent<LevelController>().musicController.ChangeTrack();
           activated = true;
        }
        catch (System.NullReferenceException) { Debug.Log("LevelController not found"); };
    }
}

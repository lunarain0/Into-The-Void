using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMusicTrack : MonoBehaviour {
    private bool activated = false;
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player" && !activated)
        {
            Debug.Log("Nextrack");
            try
            {
                GameObject.Find("LevelSelection").GetComponent<LevelController>().musicController.Nexttrack();

                activated = true;
            }
            catch (System.NullReferenceException e) { Debug.Log("LevelController not found"); };



        }
    }
}

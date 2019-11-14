using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour {


    GameObject oldSelection;
	// Use this for initialization
	void Start () {
        EventSystem.current.SetSelectedGameObject(transform.Find("Menu").GetChild(0).gameObject);
        oldSelection = EventSystem.current.currentSelectedGameObject;
        transform.Find("Menu").GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(delegate { GameObject.Find("LevelSelection").GetComponent<LevelController>().ChangeLevel(1); });
        transform.Find("Menu").GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(delegate { GameObject.Find("LevelSelection").GetComponent<LevelController>().ChangeLevel(10); });
    }
	
	// Update is called once per frame
	void Update () {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(oldSelection);
        }
        if (EventSystem.current.currentSelectedGameObject != oldSelection)
        {
            PlaySound();
            oldSelection = EventSystem.current.currentSelectedGameObject;
        }

    }
    public void PlaySound()
    {
        transform.GetComponent<AudioSource>().Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EndingController : MonoBehaviour {
    public string strawbtext;
    TextMeshProUGUI strawberrycount;
    TextMeshProUGUI strawberrycount2;
    GameObject button;
    // Use this for initialization
    void Start () {

        button = transform.Find("Menu").GetChild(0).gameObject;
        button.GetComponent<Button>().onClick.AddListener(delegate { GameObject.Find("LevelSelection").GetComponent<LevelController>().ChangeLevel(1); });
        strawberrycount = transform.Find("StrawbValue").GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        strawberrycount2 = transform.Find("StrawbValue").GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
    }
	
	// Update is called once per frame
	void Update () {
        EventSystem.current.SetSelectedGameObject(button);
        strawberrycount.text = strawbtext + " Strawberries Collected";
        strawberrycount2.text = strawbtext + " Strawberries Collected";

    }
}

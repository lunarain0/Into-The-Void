using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VNController : MonoBehaviour {
    public bool pause;
    public Queue <GameObject> dialogs;
    [System.Serializable]
    public class Speaker
    {
        public string name;
        public Sprite portrait;
        public Color32 color;
    }
    public GameObject dialogTemplate;
    [System.Serializable]
    public class Text
    {
        public string dialog;
        public string name;
        public AudioClip dialogSound;

    }
    [System.Serializable]
    public class Scene
    {
        public Text[] scene;
    }
    public Scene[] scenes; 
    public bool active = false;
    public Speaker[] speakers;
    public GameObject currentDialog;
    // Use this for initialization

    private void Start()
    {
    }
    void StartMethod (int scene) {
        dialogs = new Queue<GameObject>();
        foreach (Text text in scenes[scene].scene)
        {
            string name = text.name;
            string dialog = text.dialog;
            GameObject newDialog = Instantiate(dialogTemplate);
            newDialog.transform.SetParent(transform,false);

            GameObject activeName = newDialog.transform.Find("Name").gameObject;
            GameObject activePortrait = newDialog.transform.Find("Portrait").gameObject;
            GameObject activeDialog = newDialog.transform.Find("Dialog").gameObject;
            newDialog.GetComponent<AudioSource>().clip = text.dialogSound; 
            activeName.GetComponent<TextMeshProUGUI>().text = name + ":";
            activeDialog.GetComponent<TextMeshProUGUI>().text = dialog;

            foreach (Speaker speaker in speakers)
            {
                if (speaker.name == text.name)
                {
                    activePortrait.GetComponent<Image>().sprite = speaker.portrait;
                    activeName.GetComponent<TextMeshProUGUI>().color = speaker.color;
                }
            }

            dialogs.Enqueue(newDialog);
            newDialog.SetActive(false);
        }
        currentDialog = dialogs.Dequeue();
        currentDialog.SetActive(true);
        currentDialog.GetComponent<AudioSource>().Play();
        gameObject.SetActive(true);
        active = true;
    }
	
    public void AdvanceDialog()
    {
        if (dialogs.Count == 0)
        {
            active = false;
        }
        Destroy(currentDialog);
        if (active == true)
        {
            
            currentDialog = dialogs.Dequeue();
            currentDialog.SetActive(true);
            currentDialog.GetComponent<AudioSource>().Play();

        }
    }
	// Update is called once per frame
	void Update () {
        if (!pause)
        {
            currentDialog.GetComponent<AudioSource>().UnPause();
            if (Input.GetButtonDown("Jump"))
            {
                AdvanceDialog();
            }

            if (active == false && currentDialog.GetComponent<AudioSource>().isPlaying == false)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            currentDialog.GetComponent<AudioSource>().Pause();
        }
	}
}

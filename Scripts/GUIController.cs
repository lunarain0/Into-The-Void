using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GUIController : MonoBehaviour {

    public GameObject actionMenu;
    public GameObject actionList;
    public GameObject actionView;
    public GameObject actionButton;
    GameObject lastButton;
    public GameObject display;
    PlayerStats stats;
    public GameObject character;
    TextMeshProUGUI healthDisplay;
    TextMeshProUGUI affinityDisplay;
    TextMeshProUGUI apDisplay;
    TextMeshProUGUI augDisplay;
    TextMeshProUGUI debDisplay;
    TextMeshProUGUI strawbDisplay;
    public Color[] colors;
    RectTransform healthBar;
    RectTransform staminaBar;
    GameObject apBar;
    public GameObject oldSelection;
    float oldTime;
    bool inList;
    public GameObject apChunk;

    // Use this for initialization
    public void ListDisplay ()
    {
        inList = true;
        actionView.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
        character.GetComponent<PlayerController>().rangeProjector.SetActive (true);
        EventSystem.current.SetSelectedGameObject(actionList.transform.GetChild(0).gameObject);
    }
    public void MenuDisplay()
    {
        EventSystem.current.SetSelectedGameObject(actionMenu.transform.GetChild(0).gameObject);
        actionMenu.transform.DOScale(new Vector3(1f, 1f, 1f), 0.3f);
    }

    public void PlaySound()
    {
        transform.GetComponent<AudioSource>().Play();
    }


    void Start () {

        healthDisplay = display.transform.Find("HP").Find("Value").gameObject.GetComponent<TextMeshProUGUI>();
        healthBar = display.transform.Find("HP").Find("HP Bar").gameObject.GetComponent<RectTransform>();
        affinityDisplay = display.transform.Find("Affinity").Find("Value").gameObject.GetComponent<TextMeshProUGUI>();
        apDisplay = display.transform.Find("AP").Find("Value").gameObject.GetComponent<TextMeshProUGUI>();
        augDisplay = display.transform.Find("Augmentation").Find("Value").gameObject.GetComponent<TextMeshProUGUI>();
        debDisplay = display.transform.Find("Debilitation").Find("Value").gameObject.GetComponent<TextMeshProUGUI>();
        apBar = display.transform.Find("AP").Find("BarContainer").Find("AP Bar").gameObject;
        staminaBar = display.transform.Find("Stamina").Find("Stamina Bar").gameObject.GetComponent<RectTransform>();
        strawbDisplay = display.transform.Find("Strawbies").Find("Value").gameObject.GetComponent<TextMeshProUGUI>();
        stats = character.GetComponent<PlayerStats>();

        oldSelection = EventSystem.current.currentSelectedGameObject;
        while (apBar.transform.childCount < stats.maxAP)
        {
            GameObject chunk = Instantiate<GameObject>( apChunk);
            chunk.transform.SetParent(apBar.transform);
            chunk.transform.localScale = Vector3.one;
        }
        actionMenu.transform.SetParent(transform);

    }
	
	// Update is called once per frame
	void Update () {
        RefreshStats();
        transform.Find("Overlay").SetAsLastSibling();

        if (Input.GetButtonDown("ActionMenu") && !transform.Find("Overlay").gameObject.activeInHierarchy 
            && !GameObject.Find("character").GetComponent<PlayerController>().inCutscene)
        {
            
            MenuDisplay();
            oldSelection = EventSystem.current.currentSelectedGameObject;

        }

        if (Input.GetButton("ActionMenu"))
        {
            if (EventSystem.current.currentSelectedGameObject != oldSelection)
            {
                PlaySound();
                oldSelection = EventSystem.current.currentSelectedGameObject;
            }
        }

        if ((!Input.GetButton("ActionMenu") && !transform.Find("Overlay").gameObject.activeInHierarchy )
            || (GameObject.Find("character").GetComponent<PlayerController>().inCutscene && !transform.Find("Overlay").gameObject.activeInHierarchy))
        {
            EventSystem.current.SetSelectedGameObject(null);
            inList = false;
            actionMenu.transform.DOScale(new Vector3(0f, 0f, 0f), 0.3f);
            actionView.transform.DOScale(new Vector3(0f, 0f, 0f), 0.3f);
            character.GetComponent<PlayerController>().rangeProjector.SetActive(false);
            foreach (Transform child in actionList.transform)
            {
                GameObject.Destroy(child.gameObject);
                
            }
        }

        if (inList)
        {
            foreach (Button action in actionMenu.GetComponentsInChildren<Button>())
            {
                action.enabled = false;
                action.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);
            }
        }
        if (!inList)
        {
            foreach (Button action in actionMenu.GetComponentsInChildren<Button>())
            {
                action.enabled = true;
                action.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
            }
        }
        if (Input.GetButtonDown("Pause"))
        {
            
            PlaySound();
            if (transform.Find("Overlay").gameObject.activeInHierarchy)
            {
                transform.Find("Overlay").gameObject.SetActive(false);
                ResetTime();
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.GetComponent<ModifiedInputModule>().InMenu = false;
            }
            else
            {
                transform.Find("Overlay").GetChild(6).gameObject.GetComponent<Button>().onClick.AddListener(delegate { GameObject.Find("LevelSelection").GetComponent<LevelController>().ChangeLevel(1); });
                EventSystem.current.GetComponent<ModifiedInputModule>().InMenu = true;
                if (GameObject.Find("LevelSelection").GetComponent<LevelController>().currentVN != null)
                GameObject.Find("LevelSelection").GetComponent<LevelController>().currentVN.GetComponent<VNController>().pause = true;
                transform.Find("Overlay").gameObject.SetActive(true);
                GameObject.Find("character").GetComponent<PlayerController>().inCutscene = true;
                oldTime = Time.timeScale;
                EventSystem.current.SetSelectedGameObject(transform.Find("Overlay").Find("Resume").gameObject);
                oldSelection = EventSystem.current.currentSelectedGameObject;
                Time.timeScale = 0f;

            }
        }

        if (transform.Find("Overlay").gameObject.activeInHierarchy)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(oldSelection);
            }
            else if (EventSystem.current.currentSelectedGameObject != oldSelection)
            {
                PlaySound();
                oldSelection = EventSystem.current.currentSelectedGameObject;
            }

        }
    }
    void InstantiateButton (GameObject button, Color32 color, Action action)
    {
        GameObject newButton = Instantiate<GameObject>(button);
        newButton.name = action.name;
        newButton.transform.SetParent(actionList.transform);
        newButton.transform.localScale = Vector3.one;
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = action.name;
        newButton.GetComponentInChildren<TextMeshProUGUI>().color = color;
        newButton.GetComponent<Button>().onClick.AddListener(delegate { CallFunction(action); }); 
    }
    public void PopulateActionList(int type)
    {


        switch (type)
        {
            case 1:
                
                foreach (Action action in stats.AvailableActions.Buffs)
                {
                    InstantiateButton(actionButton, new Color32(14, 186, 255, 255), action);

                }
                break;
            case 2:
                foreach (Action action in stats.AvailableActions.Debuffs)
                {
                    InstantiateButton(actionButton, new Color32(255, 18, 105, 255), action);
                }
                break;
            case 3:
                foreach (Action action in stats.AvailableActions.Healing)
                {
                    InstantiateButton(actionButton, new Color32(90, 255, 45, 255), action);
                }
                break;
            case 4:
                foreach (Action action in stats.AvailableActions.Specials)
                {
                    InstantiateButton(actionButton, new Color32(225, 141, 255, 255), action);
                }
                break;
        }
    }

    public void RefreshStats()
    {
        healthDisplay.text = stats.health.ToString();
        healthBar.Find("Bar").localScale = new Vector3(stats.health/100f, 1,1);
        affinityDisplay.text = stats.AffinityToText();
        affinityDisplay.color = colors[stats.Affinity];
        augDisplay.text = stats.augment;
        apDisplay.text = stats.APToText();
        staminaBar.Find("Bar").localScale = new Vector3(stats.stamina, 1, 1);
        strawbDisplay.text = stats.StrawbsToText();
        for (int i = 0; i < stats.maxAP - stats.ActionPoints; i++)
        {
            apBar.transform.GetChild(i).Find("Chunk").gameObject.SetActive(false);
        }
        for (int i = stats.maxAP - stats.ActionPoints; i < stats.maxAP; i++)
        {
            apBar.transform.GetChild(i).Find("Chunk").gameObject.SetActive(true);
        }
    }

    public void CallFunction(Action action)
    {
        PlaySound();
        character.GetComponent<PlayerStats>().Command(action);
    }

    public void ResetTime()
    {
        Time.timeScale = oldTime;
        GameObject.Find("character").GetComponent<PlayerController>().inCutscene = false;
        StartCoroutine(WaitOneFrame());

    }

    IEnumerator WaitOneFrame()
    {
        yield return 0;
        if (GameObject.Find("LevelSelection").GetComponent<LevelController>().currentVN != null)
            GameObject.Find("LevelSelection").GetComponent<LevelController>().currentVN.GetComponent<VNController>().pause = false;
        

    }
}

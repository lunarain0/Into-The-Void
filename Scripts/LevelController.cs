using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {


    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemies;
    private DemonController[] enemyControllers;
    private GameObject mcamera;
    [SerializeField] private GameObject visualnovel;
    Tweener deathTween;
    public int currentLevel =0;
    private IEnumerator coroutine;
    public GameObject currentVN;
    private GameObject guiInstance;
    [SerializeField] private GameObject hintDisplayPrefab;
    public GameObject hintDisplay;
    public GameObject endFlag;
    private GameObject arena;
    public string strawbstext;
    public MusicController musicController;

    // Use this for initialization
    void Start () {
        ChangeLevel(1);

	}
	
    
	// Update is called once per frame
	public void ChangeLevel (int level) {
        coroutine = LevelChanger(level);
        StartCoroutine(coroutine);
    }

    IEnumerator LevelChanger(int level)
    {
        yield return new WaitForSeconds(1f);
        switch (level)
        {
            case 1:
                currentLevel = 1;
                DontDestroyOnLoad(gameObject);
                DontDestroyOnLoad(GameObject.Find("MusicController"));
                musicController = GameObject.Find("MusicController").GetComponent<MusicController>();
                SceneManager.LoadScene("Title");
                musicController.Reset();
                musicController.Title();
                break;
            case 2:
                if (currentLevel == 1)
                {
                    currentLevel = 2;
                    Begin();
                }

                break;
            case 3:
                currentLevel = 3;
                musicController.Nexttrack();
                musicController.Nexttrack();
                PreBattle();
                break;
            case 4:
                currentLevel = 4;
                Battle();
                break;
            case 5:
                musicController.Nexttrack();
                musicController.Nexttrack();
                AfterBattle();
                break;
            case 6:
                currentLevel = 6;
                Platform();
                break;
            case 7:
                currentLevel = 7;
                FinalWarning();
                break;
            case 8:
                currentLevel = 8;
                musicController.Nexttrack();
                musicController.ChangeTrack();
                StartCoroutine(Ending());
                break;
            case 9:
                if (currentLevel != 9)
                {
                    musicController.GameOver();
                    musicController.ChangeTrack();
                    currentLevel = 9;
                    Death();
                }
                break;
            case 10:
                Application.Quit();
                break;

        }
    }

    IEnumerator WaitToFinish()
    { 
            yield return new WaitUntil(() => currentVN == null);
        
        VNStart(3);
        player.GetComponent<PlayerController>().SelfActualize();
        HintDisplay(1);
        player.transform.SetParent(null);
        mcamera.transform.SetParent(null);
        endFlag = GameObject.Find("EndFlag" + 1);

    }
    private void Update()
    {
        if (endFlag != null){
            if (endFlag.GetComponent<FlagController>().flagged == true)
            {
                Destroy(endFlag);
                ChangeLevel(currentLevel + 1);
            }
        }

        if (player != null)
        {
            if (!player.GetComponent<PlayerStats>().alive)
            {
                ChangeLevel(9);
            }
            if (currentVN != null)
            {

                player.GetComponent<PlayerController>().inCutscene = true;
                if (enemies.transform.childCount != 0 && currentLevel == 4)
                {
                    foreach (DemonController demon in enemyControllers)
                    {
                        demon.Pause();
                    }
                }
                
                
            }
            else if (currentVN == null)
            {
                player.GetComponent<PlayerController>().inCutscene = false;

                if (enemies.transform.childCount != 0 && currentLevel == 4)
                {
                    foreach (DemonController demon in enemyControllers)
                    {
                        demon.Resume();
                    }
                }
                if (currentLevel == 3)
                {
                    ChangeLevel(4);
                }
            }
            if (enemies.transform.childCount == 0 && currentLevel == 4)
            {
                currentLevel = 5;
                ChangeLevel(5);
            }
            strawbstext = player.GetComponent<PlayerStats>().StrawbsToText();

        }



    
    }

    private void Begin()
    {

        SceneManager.LoadScene("Level");
        musicController.Nexttrack();
        musicController.Nexttrack();
        coroutine = Load();
        StartCoroutine(coroutine);
    }

    private void PreBattle()
    {

        arena.GetComponent<Animator>().SetTrigger("ArenaTrigger");
        player.transform.SetParent(arena.transform);
        mcamera.transform.SetParent(arena.transform);
        VNStart(1);
    }


    private void FinalWarning()
    {

        VNStart(4);
    }

    private IEnumerator Ending()
    {
        
        SceneManager.LoadScene("Ending");
        yield return new WaitUntil(() => GameObject.Find("EndingGUI") != null);
        guiInstance = GameObject.Find("EndingGUI");
        guiInstance.GetComponent<EndingController>().strawbtext = strawbstext;
        HintDisplay(4);
        musicController.Nexttrack();
        musicController.Nexttrack();

    }
    private void Battle()
    {

        enemies.SetActive(true);
    }

    private void VNStart(int scene)
    {

        currentVN = Instantiate(visualnovel);
        currentVN.transform.SetParent(guiInstance.transform, false);
        currentVN.SendMessage("StartMethod", scene);

    }

    private void AfterBattle()
    {

        arena.GetComponent<Animator>().SetTrigger("ReverseTrigger");
        VNStart(2);
    }
    private void Platform()
    {
        coroutine = WaitToFinish();
        StartCoroutine(coroutine);

    }

    private void Death()
    {
        deathTween = DOTween.To(() => mcamera.GetComponents<PostProcessVolume>()[2].weight, x => mcamera.GetComponents<PostProcessVolume>()[2].weight = x, 1f, 2f);
        coroutine = DeathRoutine();
        StartCoroutine(coroutine);
        
    }

    public void HintDisplay(int hintnum)
    {
        hintDisplay = Instantiate(hintDisplayPrefab);
        hintDisplay.transform.SetParent(guiInstance.transform, false);
        hintDisplay.SendMessage("ChangeHint", hintnum);
    }
    private IEnumerator Load()
    {
        yield return new WaitUntil(() => GameObject.Find("character") != null);

        player = GameObject.Find("character");
        mcamera = GameObject.Find("Main Camera");
        guiInstance = GameObject.Find("GUI");
        endFlag = GameObject.Find("EndFlag"+0);
        arena = GameObject.Find("Arena");
        enemies = arena.transform.Find("Enemies").gameObject;
        enemyControllers = enemies.GetComponentsInChildren<DemonController>();
        HintDisplay(0);
        VNStart(0);

    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver");
    }

}

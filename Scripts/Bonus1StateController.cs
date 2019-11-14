using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bonus1StateController : MonoBehaviour {
    Transform stage1;
    Transform stage2;
    Transform stage3;
    Transform stage4;
    Transform stage5;
    public TextMeshProUGUI time;
    public float timeleft;
    public bool active = false;
    public bool complete = false;
    // Use this for initialization
    void Start () {
        stage1 = transform.Find("Balloons1");
        stage2 = transform.Find("Balloons2");
        stage3 = transform.Find("Balloons3");
        stage4 = transform.Find("Balloons4");
        stage5 = transform.Find("Balloons5");
    } 
	
	// Update is called once per frame
	void Update () {
        if (Time.timeScale != 0)
        {

            if (active == true && complete == false)
            {
                    timeleft -= Time.deltaTime;
                    time.text = timeleft.ToString();
                
                if (stage1.GetComponentsInChildren<Transform>().GetLength(0) <= 5)
                {
                    GetComponent<Animator>().SetTrigger("StageOneComplete");
                }
                if (stage2.GetComponentsInChildren<Transform>().GetLength(0) <= 7)
                {
                    GetComponent<Animator>().SetTrigger("StageTwoComplete");
                }
                if (stage3.GetComponentsInChildren<Transform>().GetLength(0) <= 6)
                {
                    GetComponent<Animator>().SetTrigger("StageThreeComplete");
                }
                if (stage4.GetComponentsInChildren<Transform>().GetLength(0) <= 10)
                {
                    GetComponent<Animator>().SetTrigger("StageFourComplete");
                }
                if (stage5.GetComponentsInChildren<Transform>().GetLength(0) <= 2)
                {
                    complete = true;
                    time.text = "Complete";
                    GetComponent<Animator>().SetTrigger("StageFiveComplete");
                    StartCoroutine(Deactivate());
                }
                else if (timeleft <= 0)
                {
                    complete = true;
                    time.text = "Failed";
                    GetComponent<Animator>().enabled = false;
                    stage1.gameObject.SetActive(false);
                    stage2.gameObject.SetActive(false);
                    stage3.gameObject.SetActive(false);
                    stage4.gameObject.SetActive(false);
                    stage5.gameObject.SetActive(false);
                    StartCoroutine(Deactivate());
                }
            }
        }

    }

    private IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(2f);
        time.text = "";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && active == false)
        {
            active = true;
            GetComponent<Animator>().enabled = true;
        }
    }
}

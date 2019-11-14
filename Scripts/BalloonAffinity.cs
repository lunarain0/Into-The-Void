using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonAffinity : MonoBehaviour {

    public enum Affinity
    {
        Null, Dark, Light
    }
    public GameObject Balloon;
    public Affinity affinity;
    private float timer;
    public Material[] materials;

    private void ChangeAffinity()
    {
        if (affinity == Affinity.Null)
        {
            affinity = Affinity.Dark;
        }
        else if (affinity == Affinity.Dark)
        {
            affinity = Affinity.Light;
        }
        else if (affinity == Affinity.Light)
        {
            affinity = Affinity.Null;
        }

    }
    // Use this for initialization
    void Update () {
        if (timer > 5f)
        {
            ChangeAffinity();
            timer = 0;
        }
        timer += Time.deltaTime;
        Balloon.GetComponent<MeshRenderer>().material = materials[(int)affinity];
    }
	
}

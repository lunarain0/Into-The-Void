using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonPunch : MonoBehaviour
{
    DemonController.Affinity playerAffinity;
    DemonController.Affinity affinity;
    private void Update()
    {
        playerAffinity = GetComponentInParent<DemonController>().playerAffinity;
        affinity = GetComponentInParent<DemonController>().affinity;
    }

    // Use this for initialization
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("owo");
            if (GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Slam")){
                GetComponentInParent<DemonController>().playerStats.Damage((int)(30 * StrengthCalculator()));
            }
            if (GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("GroundAttack"))
            {
                GetComponentInParent<DemonController>().playerStats.Damage((int)(20 * StrengthCalculator()));
            }
        }
    }

    private float StrengthCalculator()
    {
        if ((playerAffinity == DemonController.Affinity.Dark && affinity == DemonController.Affinity.Light) ||
            (playerAffinity == DemonController.Affinity.Light && affinity == DemonController.Affinity.Null) ||
            (playerAffinity == DemonController.Affinity.Null && affinity == DemonController.Affinity.Dark))
        {
            return 2f;
        }
        else if ((playerAffinity == DemonController.Affinity.Dark && affinity == DemonController.Affinity.Null) ||
                 (playerAffinity == DemonController.Affinity.Light && affinity == DemonController.Affinity.Dark) ||
                 (playerAffinity == DemonController.Affinity.Null && affinity == DemonController.Affinity.Light))
        {
            return 0.5f;
        }
        else if ((playerAffinity == affinity ))
        {
            return 1f;
        }
        else return 1f;
    }  
}

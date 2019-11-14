using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCast : MonoBehaviour {
    Collider[] localEnemies;
    int layermask = 1 << 12;
    public void FindEnemies(string debuff)
    {
        localEnemies = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation, layermask, QueryTriggerInteraction.Ignore);
        for (int i = 0;i < localEnemies.Length; i++)
        {

            localEnemies[i].transform.parent.GetComponent<DemonController>().debuff = (DemonController.Debuff)System.Enum.Parse(typeof(DemonController.Debuff), debuff);
            localEnemies[i].transform.parent.GetComponent<DemonController>().Debilitate();
        }
    }
}

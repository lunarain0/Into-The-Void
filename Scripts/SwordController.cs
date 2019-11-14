using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour {
    private int damage;
    private string attackMode;
    int enemyAffinity;
    private int affinity;
    bool attacking;
    Rigidbody rb;
	// Use this for initialization
	void Start () {
		
	}
	
    public void Attack(int aft, int damageAmount = 10,string mode="default")
    {
        affinity = aft;
        damage = damageAmount;
        attackMode = mode;
        if (mode == "null")
        {
            attacking = false;
        }
        else 
        {
            attacking = true;
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" && attacking)
        {
            attacking = false;
            enemyAffinity = (int)other.gameObject.GetComponentInParent<DemonController>().affinity;
            rb = other.gameObject.GetComponentInParent<Rigidbody>();
            other.gameObject.GetComponentInParent<DemonController>().Damage((int)(damage * StrengthCalculator()));
            if (attackMode == "launch")
            {
                rb.AddForceAtPosition(Vector3.up * 1000f, other.contacts[0].point);
            }
            else if (attackMode == "juggle")
            {
                rb.velocity = Vector3.zero;
            }

            else if (attackMode == "smashdown")
            {
                rb.AddForceAtPosition(Vector3.down * 1000f, other.contacts[0].point);
            }
            else if (attackMode == "stagger")
            {
                other.gameObject.GetComponentInParent<Animator>().SetTrigger("Stagger");
                StartCoroutine(Staggered(other.gameObject.GetComponentInParent<Animator>()));
            }
        }
    }

    IEnumerator Staggered(Animator animator)
    {
        if (animator != null)
        animator.SetBool("Stagger", true);
        yield return new WaitForSeconds(3f);
        if (animator != null)
        animator.SetBool("Stagger", false);
    }
    private float StrengthCalculator()
    {
        if ((affinity == 0 && enemyAffinity == 2) ||
            (affinity == 1 && enemyAffinity == 0) ||
            (affinity == 2 && enemyAffinity == 1))
        {
            return 2f;
        }
        else if ((affinity == 0 && enemyAffinity == 1) ||
                (affinity == 1 && enemyAffinity == 2) ||
                (affinity == 2 && enemyAffinity == 0))
        {
            return 0.5f;
        }
        else if ((affinity == enemyAffinity))
        {
            return 1f;
        }
        else return 1f;
    }
}

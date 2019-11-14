using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour {
    Rigidbody rb;
    public float speed;
    bool isColliding = false;
    int enemyAffinity;
    public int affinity;


    void BulletLaunch(Vector3 direction)
    {
        rb = transform.GetComponent<Rigidbody>();
        rb.AddForce(direction * speed);
        
        Destroy(gameObject, 5f);
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isColliding) return;
        isColliding = true;
        if (collision.gameObject.tag == "Enemy")
        {
            enemyAffinity = (int)collision.gameObject.GetComponentInParent<DemonController>().affinity;
            GameObject.Find("character").GetComponent<PlayerStats>().APRestore((int)(2 *StrengthCalculator()));
        }

        if (collision.gameObject.tag == "Balloon")
        {
            enemyAffinity = (int)collision.gameObject.GetComponent<BalloonAffinity>().affinity;
            if (affinity == enemyAffinity)
            {
                GameObject.Find("character").GetComponent<PlayerStats>().APRestore(5);
                collision.gameObject.GetComponent<Animator>().SetTrigger("Hit");
                Destroy(collision.gameObject, 1);
            }

        }

        if (collision.gameObject.tag == "EnemyBullet")
        {
            enemyAffinity = (int)collision.gameObject.GetComponent<EnemyProjectileController>().affinity;
            GameObject.Find("character").GetComponent<PlayerStats>().APRestore((int)(2 * StrengthCalculator()));
        }
        Destroy(gameObject,0.1f);
    }


    private float StrengthCalculator()
    {
        if ((affinity == 0 && enemyAffinity == 2) ||
            (affinity == 1 && enemyAffinity == 0) ||
            (affinity == 2 && enemyAffinity == 1) )
        {
            return 2f;
        }
        else if ((affinity == 0 && enemyAffinity == 1) ||
                (affinity == 1 && enemyAffinity == 2) ||
                (affinity == 2 && enemyAffinity == 0))
        {
            return 0.5f;
        }
        else if ((affinity == enemyAffinity ))
        {
            return 1f;
        }
        else return 1f;
    }
}

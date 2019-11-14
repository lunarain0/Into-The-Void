using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    Rigidbody rb;
    public float speed;

    bool isColliding = false;
    int playerAffinity;
    public int affinity;
    public GameObject player;
    // Use this for initialization
    private void Update()
    {
        playerAffinity = player.GetComponent<PlayerStats>().Affinity;
    }

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
        if (collision.gameObject.tag == "Player")
        {
            player.GetComponent<PlayerStats>().Damage((int)(10 * StrengthCalculator()));


        }
        Destroy(gameObject,0.1f);


    }

    private float StrengthCalculator()
    {
        if ((playerAffinity == (int)DemonController.Affinity.Dark && affinity == (int)DemonController.Affinity.Light) ||
            (playerAffinity == (int)DemonController.Affinity.Light && affinity == (int)DemonController.Affinity.Null) ||
            (playerAffinity == (int)DemonController.Affinity.Null && affinity == (int)DemonController.Affinity.Dark))
        {
            return 2f;
        }
        else if ((playerAffinity == (int)DemonController.Affinity.Dark && affinity == (int)DemonController.Affinity.Null) ||
                 (playerAffinity == (int)DemonController.Affinity.Light && affinity == (int)DemonController.Affinity.Dark) ||
                 (playerAffinity == (int)DemonController.Affinity.Null && affinity == (int)DemonController.Affinity.Light))
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

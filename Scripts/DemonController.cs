using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DemonController : MonoBehaviour {
    Rigidbody rb;
    private IEnumerator coroutine;
    public float thrust;
    public GameObject projectile;
    private bool turning =false;
    public float walkDistance;
    float currentWalked = 0f;
    public bool onAlert;
    public float distance;
    bool soundTrigger;
    public GameObject player;
    public bool fleeing;
    public float shootdistance;
    public float meleedistance;
    private float timeAmount;
    private float timer2Amount;
    public float strafeDistance;
    private float timer;
    private float timer2;
    public float healthValue;
    public bool grounded;
    public AudioClip notice;
    public AudioClip whereareyou;
    public AudioClip shoot;
    public AudioClip dodge;
    public AudioClip jump;
    public AudioClip heal;
    public AudioClip hit;
    public AudioClip punch;
    public AudioClip dive;
    public AudioClip dead;
    public AudioClip walk;
    public AudioClip spawn;
    public AudioSource audioSource;
    private Animator animator;
    public Material[] Materials;
    public Material[] BulletMaterials;
    public Material[] FistMaterials;
    public Color[] Colors;
    public PlayerStats playerStats;
    public float speed =1;
    public Light demonAura;
    public Affinity affinity;
    public GameObject fist;
    public Affinity playerAffinity;
    public Debuff debuff;
    private bool paused;
    public GameObject canvas;
    TextMeshProUGUI health;
    TextMeshProUGUI healthShadow;
    TextMeshProUGUI debilitate;
    TextMeshProUGUI debilitateShadow;
    public enum Affinity
    {
        Null, Dark, Light
    }

    public enum Debuff
    {
        None,Poison,Slow,Stop
    }
    private void ChangeAffinity()
    {
        if (affinity == Affinity.Null)
        {
            affinity = Affinity.Dark;
        }
        else if(affinity == Affinity.Dark)
        {
            affinity = Affinity.Light;
        }
        else if (affinity == Affinity.Light)
        {
            affinity = Affinity.Null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            

            onAlert = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {


            onAlert = false;
            soundTrigger = false;


        }
    }
    
    int layermask = 1 << 9;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        animator = transform.GetComponent<Animator>();
        debuff = Debuff.None;
        player = GameObject.Find("character");
        playerStats = player.GetComponent<PlayerStats>();
        rb.centerOfMass = transform.position;
        if (animator != null)
        {
        animator.SetTrigger("Spawn");
        health = canvas.transform.Find("Health").Find("Health").GetComponent<TextMeshProUGUI>();
            healthShadow = canvas.transform.Find("Health").GetComponent<TextMeshProUGUI>();
            debilitate = canvas.transform.Find("Debuff").Find("Debuff").GetComponent<TextMeshProUGUI>();
            debilitateShadow = canvas.transform.Find("Debuff").GetComponent<TextMeshProUGUI>();
        }

        timer = 0;
        timer2 = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (!paused && Time.timeScale!=0)
        {


            gameObject.GetComponentInChildren<MeshRenderer>().material = Materials[(int)affinity];
            fist.GetComponent<MeshRenderer>().material = FistMaterials[(int)affinity];
            demonAura.color = Colors[(int)affinity];
            if (animator != null)
                animator.SetBool("PlayerAttacking", PlayerAttacking());
            playerAffinity = (Affinity)playerStats.Affinity;
            IsAlive();
            Walk();
            Alert();
            Distance();
            Fleeing();
            if (animator != null)
            {
                animator.speed = speed;
                if (healthValue <= 0)
                {
                    healthValue = 0;
                    animator.SetTrigger("DeathTrigger");
                }
                animator.SetInteger("Health", (int)healthValue);
                health.text = ((int)healthValue).ToString();
                healthShadow.text = ((int)healthValue).ToString();
                if (debuff != Debuff.None)
                {
                    debilitate.text = debuff.ToString();
                    debilitateShadow.text = debuff.ToString();
                }
                else
                {
                    debilitate.text = "";
                    debilitateShadow.text = "";
                }
                animator.SetBool("Grounded", Grounded());
            }


            timer += Time.deltaTime;
            if (timer >= timeAmount)
            {
                RandomMovement();
                timeAmount = Random.Range(0.3f, 3f);
                timer = 0;
            }

            timer2 += Time.deltaTime;
            if (timer2 >= timer2Amount)
            {
                RandomTrigger();
                timer2Amount = Random.Range(0.5f, 1.5f);
                timer2 = 0;
            }
        }

    }
    
    public void Debilitate()
    {
        if (debuff == Debuff.Poison)
        {
            StartCoroutine(Poison());
        }
        else if (debuff == Debuff.Slow)
        {
            StartCoroutine(Slow());
        }
        else if (debuff == Debuff.Stop)
        {
            StartCoroutine(Stop());
        }
    }

    private IEnumerator Poison()
    {
        for (float i=0; i<10; i += Time.deltaTime)
        {
            
            healthValue -= Time.deltaTime * 8;
            yield return null;
        }
        
        debuff = Debuff.None;
    }
    private IEnumerator Slow()
    {
        speed = 0.5f;
        yield return new WaitForSeconds(10f);
        debuff = Debuff.None;
        speed = 1.0f;
    }
    private IEnumerator Stop()
    {
        speed = 0f;
        yield return new WaitForSeconds(5f);
        debuff = Debuff.None;
        speed = 1.0f;
    }
    public void Pause()
    {
        paused = true;
        if (animator != null)
            animator.speed = 0;

    }

    public void Resume()
    {
        paused = false;
        if (animator != null)
            animator.speed = 1;

    }
    private void RandomMovement()
    {
        if (PlayerAttacking())
        {
            if (animator != null)
                animator.SetTrigger("DodgeTrigger");
        }
        int random = Random.Range(0, 4);
        switch (random)
        {
            case 0:
                if (animator != null)
                    animator.SetTrigger("BackAway");
                break;
            case 1:
                if (animator != null)
                    animator.SetTrigger("Approach");
                break;
            case 2:
                if (animator != null)
                    animator.SetTrigger("StrafingLeft");
                break;
            case 3:
                if (animator != null)
                    animator.SetTrigger("StrafingRight");
                break;

        }
    }
    private void RandomTrigger()
    {
        int random = Random.Range(0, 5);
        switch (random)
        {
            case 0:
                ChangeAffinity();
                break;
            case 1:
                if (animator != null)
                    animator.SetTrigger("Exit");
                break;
            case 2:
                if (animator != null)
                    animator.SetTrigger("Attack");
                break;
            case 3:
                if (animator != null)
                    animator.SetTrigger("Shoot");
                break;
            case 4:
                if (animator != null)
                    animator.SetTrigger("Jump");
                break;

        }
    }
    private bool Grounded()
    {
        Debug.DrawRay(transform.position, Vector3.down*1.5f,Color.cyan);
        return (Physics.Raycast(transform.position, Vector3.down, 1.5f, ~layermask, QueryTriggerInteraction.Ignore));
    }
    public void Damage(int amount)
    {
        if (animator != null)
        {
        animator.SetTrigger("Damaged");
            healthValue -= amount;

        }
            
    }

    public bool PlayerAttacking()
    {
        if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName("Fire")||
        !player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(2).IsName("NullState")||
        !player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(3).IsName("NullState"))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void Distance()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);


            if (distance <= shootdistance)
            {
            if (animator != null)
                animator.SetBool("ShootingDistance", true);
            }
            else
            {
            if (animator != null)
                animator.SetBool("ShootingDistance", false);
            }
            if (distance <= meleedistance)
            {
            if (animator != null)
                animator.SetBool("MeleeDistance", true);
            }
            else
            {
            if (animator != null)
                animator.SetBool("MeleeDistance", false);
            }
            if (distance <= strafeDistance)
            {
            if (animator != null)
                animator.SetBool("StrafingDistance", true);
            }
            else
            {
            if (animator != null)
                animator.SetBool("StrafingDistance", false);
            }
        

    }
    public void IsAlive()
    {
        if (animator != null)
        {
            if (animator.GetBool("Dead"))
        {
                Destroy(gameObject,1f);            
        }
        }

    }

    public void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x,100f,rb.velocity.z);
    }
    public void JumpHang()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }

    public void Drop()
    {
        rb.velocity = new Vector3(rb.velocity.x, -400, rb.velocity.z);
    }
    
    public void Fleeing()
    {
        if (animator != null)
        {
            if (animator.GetBool("Fleeing"))
        {
            rb.velocity = (transform.forward * -1 * thrust * speed);
            if(distance > 35f)
            {
                coroutine = Turn(transform.rotation * Quaternion.Euler(0, 180f, 0));
                StartCoroutine(coroutine);
            }
        }
        }



    }

    public void Strafe(int direction)
    {
            rb.velocity = (transform.right * thrust * speed * direction/2.5f);
    }

    public void Running()
    {
                rb.velocity = (transform.forward * speed * thrust/2);
               
    }

    public void BackingAway()
    {
        rb.velocity = (-1 * transform.forward * speed * thrust / 3);

    }

    public void Walk()
    {
        if (animator != null)
        {
        if (animator.GetBool("Walking"))
        {
            if(!turning)
            {
                rb.velocity = (transform.forward * speed * thrust/4);
                currentWalked += 0.1f;
            }

            if (currentWalked >= walkDistance)
            {
                coroutine = Turn(transform.rotation * Quaternion.Euler(0, 180f, 0));
                StartCoroutine(coroutine);

                currentWalked = 0;
            }
        }
        }
           

    }

    public void Fire()
    {
        JumpHang();
        GameObject bullet = Instantiate<GameObject>(projectile);
        bullet.GetComponent<EnemyProjectileController>().affinity = (int)affinity;
        bullet.GetComponent<EnemyProjectileController>().player = player;
        bullet.GetComponent<MeshRenderer>().material = BulletMaterials[(int)affinity];
        bullet.GetComponentInChildren<Light>().color = Colors[(int)affinity];
        bullet.transform.SetParent(transform);
                bullet.transform.localPosition = new Vector3(0f, 2.3f,6f);
        Debug.DrawLine(bullet.transform.position, player.transform.position, Color.red);
        bullet.SendMessage("BulletLaunch",(player.transform.position + Vector3.up*3)- bullet.transform.position );
                bullet.transform.SetParent(transform.parent);


    }
    
    public void Alert()
    {

        if (onAlert && !turning && !fleeing)
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(player.transform.position.x, 0, player.transform.position.z) - new Vector3(transform.position.x,0, transform.position.z), Vector3.up);
            if (!soundTrigger)
            {
                audioSource.PlayOneShot(notice);
                soundTrigger = true;
            }
        }

        if (animator != null)
            animator.SetBool("OnAlert", onAlert);
    }
    public IEnumerator Turn(Quaternion destination, float turnSpeed = 1)
    {
        if (grounded)
        rb.velocity = Vector3.zero;
        turning = true;
        Quaternion start = transform.rotation;
        for (float t = 0f; t < turnSpeed *speed; t += Time.deltaTime)
        {
            transform.rotation = Quaternion.Lerp(start,destination, t/(turnSpeed * speed));
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = destination;
        turning = false;
       
    }

    public void Dodge()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce ((transform.position - player.transform.position)*thrust*10 * speed);
    }

    public void AirAttack()
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(-1*(transform.position - player.transform.position) * thrust * 10 * speed);
    }


}

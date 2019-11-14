using DG.Tweening;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Animator animator;
    public CapsuleCollider LeftFoot;
    public CapsuleCollider RightFoot;
    public CapsuleCollider LeftToe;
    public CapsuleCollider RightToe;
    public GameObject projectile;
    public GameObject target;
    public GameObject gunHolster;
    public GameObject gunPositioner;
    [SerializeField]
    private GameObject gunPrefab;
    public GameObject gun;
    private float actionFloat = 0;
    [SerializeField]
    private GameObject swordPrefab;
    public GameObject swordPositioner;
    public GameObject swordHolster;
    public GameObject sword;
    public bool teleporting;
    public Tweener tween;
    float leftHeight;
    float rightHeight;
    public bool merged;
    [SerializeField]public bool inCutscene =false;
    public float swordSpeed;
    float joystickTilt;
    bool swordThrown = false;
    bool grounded;
    PlayerStats stats;
    public bool interactable = false;
    Rigidbody rb;
    public Material[] Materials;
    public Color[] Colors;
    public GameObject rangeProjector;
    Transform mainCamera;

    int layermask = 1 << 9;
    // Use this for initialization
    void Start() {
        stats = GetComponent<PlayerStats>();
        sword = Instantiate<GameObject>(swordPrefab);
        Positioner(sword, swordHolster);
        gun = Instantiate<GameObject>(gunPrefab);
        Positioner(gun, gunHolster);
        animator = transform.GetComponent<Animator>();
        mainCamera = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    public bool Grounded()
    {
        return (Physics.Raycast(LeftFoot.transform.position + Vector3.up, -Vector3.up, 2.5f, ~layermask, QueryTriggerInteraction.Ignore) || Physics.Raycast(RightFoot.transform.position + Vector3.up, -Vector3.up, 2.5f, ~layermask, QueryTriggerInteraction.Ignore) || Physics.Raycast(LeftToe.transform.position + Vector3.up, -Vector3.up, 2.5f, ~layermask, QueryTriggerInteraction.Ignore) || Physics.Raycast(RightToe.transform.position + Vector3.up, -Vector3.up, 2.5f, ~layermask, QueryTriggerInteraction.Ignore));
 
    }
    public bool Falling()
    {
        if (rb.velocity.y < -0.1f && !grounded)
        {
            return true;
            }
            else
            {
                return false;
            }
        }
      
    private void LateUpdate()
    {
     


}

// Update is called once per frame
void Update() {



        if (inCutscene)
        {
            Positioner(sword, swordHolster);
            Positioner(gun, gunHolster);
            animator.SetBool("Idle", true);
        }

        else if (Time.timeScale != 0)
        {
            animator.speed = stats.speed;
            if (interactable)
            {
                Interact();
            }
            else if (teleporting == true)
            {

            }
            else
            {
                animator.SetBool("Idle", false);
                if (grounded && rb.velocity.y < 0)
                {
                    animator.SetTrigger("DoubleJumpTrigger");
                    animator.SetTrigger("JumpActivator");
                }
                if (Input.GetButton("SlowTime") && stats.ActionPoints > 0)
                {
                    actionFloat += 2 * Time.deltaTime;
                    if (Mathf.Lerp(0f, 1f, actionFloat) == 1)
                    {
                        stats.ActionPoints -= 1;
                        actionFloat = 0;
                    }

                    SlowTime();
                }
                if ((Input.GetButtonUp("SlowTime")) || stats.ActionPoints == 0)
                {
                    RestoreTime();
                }
                joystickTilt = Mathf.Clamp01(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).magnitude);
                Movement(joystickTilt);
                RelativeMovement();

                animator.SetBool("Grounded", grounded);

                if (grounded)
                {
                    stats.stamina = 1f;
                    animator.SetBool("IsJumping", false);

                }
                animator.SetBool("Falling", Falling());
                if (stats.stamina > 0)
                {
                    animator.SetBool("Floating", Input.GetButton("Jump"));
                }

                if (stats.stamina <= 0f)
                {
                    stats.stamina = 0f;
                    animator.SetBool("Floating", false);

                }
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Floating") && stats.stamina > 0)
                {
                    stats.stamina -= Time.deltaTime / 5;
                }

                if (Input.GetButtonDown("Jump"))
                {

                    Jump();


                }

                if (Input.GetButtonDown("Dodge"))
                {
                    if (grounded)
                        Dodge();
                }

                if (Input.GetButtonDown("Fire"))
                {
                    Shoot();
                }

                if (Input.GetButtonDown("Heavy") && stats.ActionPoints >= 2)
                {
                    stats.ActionPoints -= 2;
                    Heavy();

                }

                if (Input.GetButtonDown("Light") && stats.ActionPoints >= 1)
                {
                    stats.ActionPoints -= 1;
                    Light();
                }

                if (Input.GetButtonDown("AffinityShift"))
                {
                    AffinityShift();
                }

                if (!animator.GetCurrentAnimatorStateInfo(1).IsName("NullState") || animator.GetCurrentAnimatorStateInfo(0).IsName("Double Jump"))
                {
                    Positioner(gun, gunPositioner);
                }
                else
                {
                    Positioner(gun, gunHolster);
                    Positioner(gun, gunHolster);
                }
                if (!animator.GetCurrentAnimatorStateInfo(2).IsName("NullState") || animator.GetCurrentAnimatorStateInfo(0).IsName("Floating") || animator.GetCurrentAnimatorStateInfo(3).IsName("Javelin"))
                {
                    if (swordThrown == false)
                        Positioner(sword, swordPositioner);
                    else if (swordThrown == true)
                    {
                        sword.GetComponent<SwordController>().Attack(stats.Affinity,45);
                    }
                    if (animator.GetCurrentAnimatorStateInfo(2).IsName("Stagger"))
                    {
                        sword.GetComponent<SwordController>().Attack(stats.Affinity,30, "stagger");
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(2).IsName("SmashDown"))
                    {
                        sword.GetComponent<SwordController>().Attack(stats.Affinity, 40, "smashdown");
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(2).IsName("Launch"))
                    {
                        sword.GetComponent<SwordController>().Attack(stats.Affinity, 30, "launch");
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(2).IsName("Light1")|| animator.GetCurrentAnimatorStateInfo(2).IsName("Light2"))
                    {
                        sword.GetComponent<SwordController>().Attack(stats.Affinity, 20);
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(2).IsName("Spin"))
                    {
                        sword.GetComponent<SwordController>().Attack(stats.Affinity, 25);
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(2).IsName("Heavy1") || animator.GetCurrentAnimatorStateInfo(2).IsName("Heavy2"))
                    {
                        sword.GetComponent<SwordController>().Attack(stats.Affinity, 30);
                    }
                    else if (animator.GetCurrentAnimatorStateInfo(2).IsName("Juggle1") 
                        || animator.GetCurrentAnimatorStateInfo(2).IsName("Juggle2")
                        || animator.GetCurrentAnimatorStateInfo(2).IsName("Juggle3"))
                    {
                        sword.GetComponent<SwordController>().Attack(stats.Affinity, 20,"juggle");
                    }
                }
                else
                {
                    Positioner(sword, swordHolster);
                    sword.GetComponent<SwordController>().Attack(stats.Affinity, 0,"null");
                }
            }
        }
        sword.GetComponent<ParticleSystemRenderer>().material = Materials[stats.Affinity];
        gun.GetComponent<MeshRenderer>().material = Materials[stats.Affinity];
        animator.SetBool("Merged", merged);
    }

    public void SlowTime()
    {

                Camera.main.GetComponent<CameraController>().FilterModify(true);
                Time.timeScale = 0.35f;

        
    }

    public void RestoreTime()
    {
        Camera.main.GetComponent<CameraController>().FilterModify(false);
        Time.timeScale = 1.0f;
    }

    public void Movement(float direction)
    {
        animator.SetFloat("VerticalMovement", direction);
    }


    public void Light()
    {
        animator.SetTrigger("LightTrigger");

    }

    public void Heavy()
    {
        animator.SetTrigger("HeavyTrigger");

    }

    public void Jump()
    {
        animator.SetTrigger("JumpTrigger");

    }

    public void AffinityShift()
    {
        stats.Affinity = 1;
    }

    public void JumpForce(float amount)
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.velocity += Vector3.up * amount ;
    }

    public void Interact()
    {
        animator.SetTrigger("InteractTrigger");
        interactable = false;
    }

    public void Dodge()
    {
        animator.SetTrigger("DodgeTrigger");
    }
    
    public void Buff()
    {
        animator.SetTrigger("Buff");
    }
    public void Debuff()
    {
        animator.SetTrigger("Debuff");
    }
    public void Attack(string attack)
    {
        if (attack == "Javelin")
        {
            FaceCamera();
            animator.SetTrigger("JavelinTrigger");
        }
        //animator.SetTrigger(attack);
    }
    public void Shoot()
    {
        animator.SetTrigger("Fire");
    }

    public void Damage()
    {
        animator.SetTrigger("Damage");
        GetComponent<AudioController>().PlaySound("damage");
    }

    public void SelfActualize()
    {
        animator.SetTrigger("Merge");
        merged = true;
    }

    public void Die()
    {
        GameObject.Find("GUI").SetActive(false);
        Material[] mats = transform.Find("Ragdoll").Find("KiraMesh").GetComponent<SkinnedMeshRenderer>().materials;
        if (merged)
        {
            foreach (Material mat in mats)
            {
                mat.SetFloat("_Blend", 1f);
            }
        }
        transform.Find("Ragdoll").gameObject.SetActive(true);
        GetComponent<AudioController>().PlaySound("death");
        gunHolster = transform.Find("Ragdoll").Find("Ragdoll").Find("thigh_L").Find("GunHolster").gameObject;
        swordHolster = transform.Find("Ragdoll").Find("Ragdoll").Find("SwordHolster").gameObject;
        gunPositioner = transform.Find("Ragdoll").Find("Ragdoll").Find("spine").Find("chest").Find("shoulder_L").Find("upper_arm_L").Find("forearm_L").Find("hand_L").Find("GunPositioner").gameObject;
        swordPositioner = transform.Find("Ragdoll").Find("Ragdoll").Find("spine").Find("chest").Find("shoulder_R").Find("upper_arm_R").Find("forearm_R").Find("hand_R").Find("SwordPositioner").gameObject;
        transform.Find("Kira").Find("KiraMesh").gameObject.SetActive(false);

    }

    public void Fire(string direction)
    {

        GameObject bullet = Instantiate<GameObject>(projectile, transform.position, transform.rotation);
        bullet.GetComponentInChildren<Light>().color = Colors[stats.Affinity];
        bullet.GetComponent<MeshRenderer>().material = Materials[stats.Affinity];
        bullet.GetComponent<ProjectileController>().affinity = stats.Affinity;
        GetComponent<AudioController>().PlaySound("fire");
        switch (direction)
        {
            case "forward":
                bullet.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 15;
                bullet.SendMessage("BulletLaunch", Camera.main.transform.forward );
                bullet.transform.SetParent(transform.parent);
                break;
            case "down":
                bullet.SendMessage("BulletLaunch", Vector3.down);
                bullet.transform.SetParent(transform.parent);
                break;
        }
    }

    public void Positioner(GameObject item, GameObject position)
    {
        item.transform.SetParent(position.transform);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }
    
    public void RelativeMovement()
    {

        if (joystickTilt != 0)
        {
            Vector3 direction = (new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")));
            Vector3 turnedDirection = mainCamera.TransformDirection(direction);
            Quaternion rotation = Quaternion.LookRotation(turnedDirection);
            rotation.x = 0f;
            rotation.z = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 30);
            turnedDirection.y = 0;
            Vector3 moveDirection = transform.forward * turnedDirection.magnitude * 30 * stats.speed;
            if (!Falling())
            {
                rb.velocity = new Vector3(moveDirection.x,rb.velocity.y,moveDirection.z) ;
            }

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Floating"))
            {
                rb.velocity = new Vector3(moveDirection.x, -0.1f * stats.speed, moveDirection.z) ;
            }


        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
        {
            rb.velocity = transform.forward * 70 * stats.speed;
        }
        if (joystickTilt == 0)
        {
            if ((grounded && rb.velocity.y <= 0))
                rb.velocity = Vector3.zero; 
            if (!grounded)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, 0) ;
            }
        }
    }

    private void FixedUpdate()
    {
        if ((rb.velocity.y < 0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Floating")) || (rb.velocity.y> 0 && !animator.GetBool("IsJumping")))
        {
            rb.velocity += Physics.gravity * 0.1f ;
        }
        grounded = Grounded();
    }

    public void ThrowSword()
    {
        sword.transform.SetParent(Camera.main.transform);
        sword.transform.localEulerAngles = new Vector3 (-7,85,85);
        sword.transform.localPosition = new Vector3(0, 0, 18);
        sword.transform.SetParent(null);
        sword.GetComponent<Rigidbody>().velocity = Camera.main.transform.forward * swordSpeed * stats.speed;
        swordThrown = true;
        transform.Find("Kira").Find("KiraMesh").gameObject.SetActive(false);
        gun.SetActive(false);
        Camera.main.GetComponent<CameraController>().target = sword.transform;
    }

    public void ThrowEnd()
    {

        transform.Find("Kira").Find("KiraMesh").gameObject.SetActive(true);
        gun.SetActive(true);
        swordThrown = false;
        transform.position = sword.transform.position;
        Quaternion rotation = transform.rotation;
        rotation.x = 0f;
        transform.rotation = rotation;
        Camera.main.GetComponent<CameraController>().target = transform;
        
    }

    public void FaceCamera()
    {
        transform.rotation = Camera.main.transform.rotation;
    }

}

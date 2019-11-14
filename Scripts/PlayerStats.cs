using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerStats : MonoBehaviour
{
    public ActionData AvailableActions;
    public string playerName;
    public int health;
    public float speed = 1f;
    public bool alive = true;
    public int maxAP = 10;
    AudioSource audioSource;
    public bool victorious = false;
    [SerializeField]private int maxStrawbs;
    public int collectedStrawbs = 0;
    [SerializeField]private int actionPoints;
    public bool strawbsComplete = false;
    public AudioClip strawbSound;
    public bool apDepleted = false;
    public AudioClip noAPSound;
    Coroutine augRoutine;
  

    private void Start()
    {
        audioSource = transform.GetComponent<AudioSource>();
        maxStrawbs = GameObject.FindGameObjectsWithTag("Collectable").Length + 20;

    }
    public int ActionPoints
    {
        set
        {
            actionPoints = value;
            actionPoints = (actionPoints < 0) ? 0 : (actionPoints > maxAP) ? maxAP : actionPoints;
        }
        get
        {
            return actionPoints;
        }
    }
    public string augment ="";
    public string augStat;
    private int tweenvalue;
    public bool invincible;
    public float stamina = 1f;
    private int currentHealth;
    private float augTime;
    [SerializeField]
    private int affinity=0;

    public int Affinity
    {
        set
        {
            affinity++;
            affinity = affinity % 3;
        }
        get
        {
            return affinity;
        }
    }
    public string AffinityToText()
    {
        switch (affinity)
        {
            case 0:
                return "NULL";
            case 1:
                return "DARK";
            case 2:
                return "LIGHT";
            default:
                return "ERROR";
        }


    }

    public string APToText()
    {
        return actionPoints + "/" + maxAP;
    }

    public string StrawbsToText()
    {
        return collectedStrawbs + "/" + maxStrawbs;
    }

    public void Command(Action action)
    {
        if (actionPoints >= action.cost)
        {
            switch (action.anim)
            {
                case (selectedAnimation.Buff):
                    GetComponent<PlayerController>().Buff();
                    augment = action.name;
                    augTime = action.time;
                    if (augment == "Haste")
                    {
                        augRoutine = StartCoroutine(Haste());
                    }
                    else if (augment == "Protect"|| augment == "High Protect")
                    {
                        augRoutine = StartCoroutine(Protect(augTime));
                    }
                    break;
                case (selectedAnimation.Debuff):
                    GetComponent<PlayerController>().Debuff();
                    transform.Find("SpellRange").GetComponent<SpellCast>().FindEnemies(action.name);
                    break;
                case (selectedAnimation.Heal):
                    GetComponent<PlayerController>().Buff();
                    Heal(action.amount);
                    break;
                case (selectedAnimation.Attack):
                    GetComponent<PlayerController>().Attack(action.name);
                    break;
            }
            ActionPoints -= action.cost;

        }
        
    }


    private IEnumerator Protect(float time)
    {
        invincible = true;
        for (float i = 0; i < time; i += Time.deltaTime)
        {
            
            yield return null;
        }
        invincible = false;
        augment = "";
    }
    private IEnumerator Haste()
    {
        speed = 2f;
        yield return new WaitForSeconds(10f);
        speed = 1f;
        augment = "";
    }

    public void APRestore (int amount)
    {
        apDepleted = false;
        ActionPoints += amount;
    }

    public void Damage (int amount)
    {
        if (invincible)
        {
            return;
        }
        GetComponent<PlayerController>().Damage();
        health -= amount;
    }

    public void Heal (int amount)
    {
        if (health < 100){
            if (health + amount < 100)
                health += amount;
            else
                health = 100;
        }

    }

    private void Update()
    {
        if (actionPoints == 0&& apDepleted==false)
        {
            apDepleted = true;
            audioSource.PlayOneShot(noAPSound);
        }
        if (collectedStrawbs == maxStrawbs&&strawbsComplete== false)
        {
            strawbsComplete = true;
            audioSource.PlayOneShot(strawbSound);
        }
        if (health <= 0 && alive == true)
        {
            alive = false;
            GetComponent<PlayerController>().Die();
        }
    }



}

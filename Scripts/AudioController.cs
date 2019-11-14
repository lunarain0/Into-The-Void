using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {
    private AudioSource audioSource;
    public AudioClip fireSound;
    public AudioClip footstepSound;
    public AudioClip deathSound;
    public AudioClip dodgeSound;
    public AudioClip damageSound;
    // Use this for initialization
    void Start () {
        audioSource = transform.GetComponent<AudioSource>();
    }
	

    public void PlaySound(string name)
    {
        switch (name)
        {
            case "fire":
                audioSource.PlayOneShot(fireSound);
                break;
            case "step":
                audioSource.PlayOneShot(footstepSound);
                break;
            case "death":
                audioSource.PlayOneShot(deathSound);
                break;
            case "damage":
                audioSource.PlayOneShot(damageSound);
                break;
        }

    }
}

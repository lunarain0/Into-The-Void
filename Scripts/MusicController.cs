using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[System.Serializable]
    public class Music
    {
        public string name;
        public AudioClip music;
        public bool isLoop;

    }
/// <summary>
/// Controls the selection and playing of the music tracks within the game.
/// </summary>
public class MusicController : MonoBehaviour {
    
    public AudioSource audioSource;
    public Music[] tracks;
    public Music title;
    public Music gameOver;
    public Music bonusLevel;
    private int tracknum = 0;
    private Coroutine routine;
    [SerializeField]
    private Queue<Music> playQueue = new Queue<Music>();
    [SerializeField]
    private Music previous;
    private Music current;
    // Use this for initialization
    void Start () {
        audioSource = gameObject.GetComponent<AudioSource>();
	}

    private void Update()
    {
        if (audioSource.isPlaying == false)
        {
            ChangeTrack();
        }

    }

    public void ChangeTrack()
    {
        if (current != null)
        previous = current;
        try
        {
            current = playQueue.Dequeue();
        }
        catch (System.InvalidOperationException) { }
        if (current != null)
        {
            audioSource.clip = current.music;
            audioSource.loop = current.isLoop;
            Debug.Log("Track:" + current.name);
        }

        audioSource.Play();
    }

	/// <summary>
    /// Play the next track in the track list
    /// </summary>
	public void Nexttrack()
    {
        audioSource.loop = false;
        try
        {
            playQueue.Enqueue(tracks[tracknum]);
            tracknum++;
        }
        catch (IndexOutOfRangeException)
        {

        }
    } 
    /// <summary>
    /// Resume previous track from track list
    /// </summary>
    public void Resume()
    {
        audioSource.loop = false;
        playQueue.Enqueue(previous);
    }
    /// <summary>
    /// Change to Game Over track
    /// </summary>
    public void GameOver()
    {
        audioSource.loop = false;
        try
        {
            current = playQueue.Last();
        }
        catch (System.InvalidOperationException) { }
        playQueue.Clear();
        playQueue.Enqueue(gameOver);
    }
    /// <summary>
    /// Change to Title track
    /// </summary>
    public void Title()
    {
        audioSource.loop = false;
        playQueue.Enqueue(title);
    }
    /// <summary>
    /// Change to Bonus Level track
    /// </summary>
    public void BonusLevel()
    {
        audioSource.loop = false;
        try
        {
            current = playQueue.Last();
        }
        catch (System.InvalidOperationException) { }
        playQueue.Clear();
        playQueue.Enqueue(bonusLevel);
    }
    public void Reset()
    {
        audioSource.loop = false;
        playQueue.Clear();
        tracknum = 0;
        audioSource.Stop();
    }
}

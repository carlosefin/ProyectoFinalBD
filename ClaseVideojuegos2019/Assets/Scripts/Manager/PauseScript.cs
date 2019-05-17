﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public GameObject audioManager;
    private AudioSource audioSource;
    private AudioSource pauseAudio;
    public AudioClip pause;
    public AudioClip pause2;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = audioManager.GetComponent<AudioSource>();
        pauseAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown (KeyCode.P))
        {
            if (GameIsPaused)
            {
                Resume();
            }else
            {
                Pause();
            }
        }
    }

    void Resume()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        audioSource.Play();
        pauseAudio.Stop();
        pauseAudio.PlayOneShot(pause2);
    }
    void Pause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
        audioSource.Pause();
        pauseAudio.PlayOneShot(pause2);
        pauseAudio.PlayOneShot (pause);
    }
}

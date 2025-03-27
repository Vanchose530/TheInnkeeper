using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gramophone : MonoBehaviour
{
    public static Gramophone instance { get; private set; } // рассмотреть должен ли грамофон быть синглтоном

    [Header("Note Cue")]
    [SerializeField] private GameObject noteCue;
    [SerializeField] private Animator noteCueAnimator;

    [Header("Music")]
    [SerializeField] private AudioSource gramophoneMusic;
    [SerializeField] private AudioSource innMusic;

    bool isPlayerInRange = false;
    public bool isMusicPlaying = false;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Find more than one Gramophone in scene");
        }

        instance = this;

        noteCue.SetActive(false);
    }

    void Update()
    {
        if (isPlayerInRange)
        {
            noteCue.SetActive(true);

            if (InputManager.instance.GetInteractPressed())
            {
                OnOffMusic();
            }
        }
        else
        {
            noteCue.SetActive(false);
        }

        
        
        noteCueAnimator.SetBool("MusicOnCue", !isMusicPlaying);
    }

    void OnOffMusic()
    {
        if (isMusicPlaying)
        {
            isMusicPlaying = false;

            innMusic.mute = false;

            gramophoneMusic.Stop();
        }
        else
        {
            isMusicPlaying = true;

            innMusic.mute = true;

            gramophoneMusic.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isPlayerInRange = false;
        }
    }
}

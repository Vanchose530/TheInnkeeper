using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    static public PauseMenuManager instance { get; private set; }

    public bool isOnPause { get; private set; }

    [Header("Pause Menu UI")]
    [SerializeField] private GameObject pauseMenuUIPanel;
    public GameObject firstButton;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Find more than one Pause Menu Manager in scene");
        }
        instance = this;   
    }

    private void Start()
    {
        Resume();
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        Innkeeper.instance.canMove = false;

        isOnPause = true;
        pauseMenuUIPanel.SetActive(true);

        StartCoroutine(SelectFirstButton());

        GameEventsManager.instance.inputEvents.onPauseButtonPressed -= Pause;
        GameEventsManager.instance.inputEvents.onPauseButtonPressed += Resume;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Innkeeper.instance.canMove = true;

        isOnPause = false;
        pauseMenuUIPanel.SetActive(false);

        GameEventsManager.instance.inputEvents.onPauseButtonPressed -= Resume;
        GameEventsManager.instance.inputEvents.onPauseButtonPressed += Pause;
    }

    public void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Debug.Log("Quit Button pressed");
        Application.Quit();
    }

    private IEnumerator SelectFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(firstButton);
    }
}

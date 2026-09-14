using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//Pauses the game and handles pause menu logic
public class GamePause : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI savingmsg;

    private AudioSource[] backAudios;

    private DataManager dataManager;
    private Animator fader;

    private bool pauseOn;
    private bool menuOn;
    private bool saveCoroutineRunning;

    private float opacity;

    private void Start()
    { 
        dataManager = FindFirstObjectByType<DataManager>();
        pauseOn = false;
        pauseMenu.SetActive(false);
        fader = pauseMenu.GetComponent<Animator>();
        menuOn = false;
        opacity = pauseMenu.GetComponent<Image>().color.a;
        GameObject seagulls = FindFirstObjectByType<GullAudioController>().gameObject;
        Transform generalAudio = seagulls.transform.parent;
        backAudios = generalAudio.GetComponentsInChildren<AudioSource>();
    }

    void Update()
    {
        opacity = pauseMenu.GetComponent<Image>().color.a;
        if (saveCoroutineRunning)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape) && pauseOn == false)
        {
            Time.timeScale = 0;
            pauseOn = true;
            pauseMenu.SetActive(true);
            if (fader != null)
            {
                fader.SetTrigger("TrIn");
                StartCoroutine(EntryAnimation());
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && pauseOn && menuOn)
        {
            if (pauseMenu.activeSelf)
            {
                Resume();
            }
        }
    }

    public void Resume()
    {
        if (saveCoroutineRunning)
        {
            return;
        }
        if (fader != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            fader.SetTrigger("TrOut");
            StartCoroutine(ExitAnimation());
        }
    }

    public void SaveGame()
    {
        if (saveCoroutineRunning)
        {
            return;
        }
        audioSource.Play();
        saveCoroutineRunning = true;
        savingmsg.enabled = true;
        StartCoroutine(SaveCoroutine());
    }

    private IEnumerator SaveCoroutine()
    {
        yield return null;
        dataManager.SaveGame();
        saveCoroutineRunning = false;
        savingmsg.enabled = false;
    }

    public IEnumerator ExitAnimation()
    {
        foreach (AudioSource audio in backAudios)
        {
            audio.mute = false;
        }
        yield return new WaitUntil(() => opacity <= 0.0f);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        pauseOn = false;
        menuOn = false;
    }

    public IEnumerator EntryAnimation()
    {
        foreach (AudioSource audio in backAudios)
        {
            audio.mute = true;
        }
        yield return new WaitUntil(() => opacity >= 0.67f);
        menuOn = true;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public bool Paused()
    {
        return pauseOn;
    }

    public void QuitToMenu()
    {
        audioSource.Play();
        SceneManager.LoadSceneAsync(0);
    }
}

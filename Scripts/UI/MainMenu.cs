
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

//Main Menu scene does not run a datamanager, and instead handles game state logic here

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string mainDataFileName;
    [SerializeField] private string arrayDataFileName;
    [SerializeField] private GameObject panel;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Button continueButton;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject manual;

    private FileHandler dataHandler;
    private ArrayFileHandler arrayFileHandler;
    private GameData gameData;
    private ArrayGameData arrayGameData;
    private int gameScene = 1;
    private int endScene = 2;
    

    private void Start()
    {
        dataHandler = new FileHandler(Application.persistentDataPath, mainDataFileName);
        arrayFileHandler = new ArrayFileHandler(Application.persistentDataPath, arrayDataFileName);
        gameData = dataHandler.Load();
        arrayGameData = arrayFileHandler.Load();
        if (gameData == null || arrayGameData == null)
        {
            continueButton.enabled = false;
            continueButton.gameObject.SetActive(false);
        }
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Continue()
    {
        audioSource.Play();
        if (gameData.gameFinished)
        {
            SceneManager.LoadSceneAsync(endScene);
        }
        else
        {
            SceneManager.LoadSceneAsync(gameScene);
        }
    }

    public void NewGameQuery()
    {
        audioSource.Play();
        foreach (Button button in gameObject.GetComponentsInChildren<Button>())
        {
            if (button.gameObject.activeInHierarchy)
            {
                button.interactable = false;
            }
        }
        panel.SetActive(true);
    }

    public void Settings()
    {
        audioSource.Play();
        settings.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Manual()
    {
        audioSource.Play();
        manual.SetActive(true);
        gameObject.SetActive(false);
    }

    //Deleting file paths forces the datamanger to start a new game in the next scene
    public void NewGame()
    {
        audioSource.Play();
        string path1 = Path.Combine(Application.persistentDataPath, mainDataFileName);
        string path2 = Path.Combine(Application.persistentDataPath, arrayDataFileName);
        if (File.Exists(path1))
        {
            File.Delete(path1);
        }
        if (File.Exists(path2))
        {
            File.Delete(path2);
        }
        SceneManager.LoadSceneAsync(gameScene);
    }

    public void GoBack()
    {
        audioSource.Play();
        panel.SetActive(false);
        foreach (Button button in gameObject.GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

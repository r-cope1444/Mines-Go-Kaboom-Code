
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//Displays game stats
public class EndScreen : MonoBehaviour, DataInterface
{
    [SerializeField] private TextMeshProUGUI[] texts;

    [SerializeField] private GameObject gameStats;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject viewButton;
    [SerializeField] private AudioSource audioSource;

    private int mainMenu = 0;

    private int finalItems = 0;
    private int finalMonstersKilled = 0;
    private int finalDeaths = 0;
    private int finalScraps = 0;
    private float finalTime = 0f;

    public void LoadData(GameData data)
    {
        finalItems = data.totalItems;
        finalMonstersKilled = data.monstersKilled;
        finalDeaths = data.deaths;
        finalScraps = data.totalScraps;
        finalTime = data.timeTaken;
    }

    public void SaveData(ref GameData data)
    {
        return;
    }

    private void Start()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ViewStats()
    {
        audioSource.Play();
        int seconds;
        int minutes;
        int hours;

        hours = (int)(finalTime / 3600f);
        if (hours == 0)
        {
            minutes = (int)(finalTime / 60f);
            if (minutes == 0)
            {
                seconds = (int)finalTime;
            }
            else
            {
                seconds = (int)(finalTime % 60f);
            }
        }
        else
        {
            int tempSeconds = (int)(finalTime % 3600f);
            minutes = tempSeconds / 60;
            seconds = tempSeconds % 60;
        }

        texts[0].text = $"Time Taken: {hours}:{minutes}:{seconds}";
        texts[1].text = $"Kills: {finalMonstersKilled}";
        texts[2].text = $"Deaths: {finalDeaths}";
        texts[3].text = $"Items Used: {finalItems}";
        texts[4].text = $"Scraps Obtained: {finalScraps}";

        viewButton.SetActive(false);
        menuButton.SetActive(true);
        gameStats.SetActive(true);
    }

    public void ReturnMainMenu()
    {
        audioSource.Play();
        SceneManager.LoadSceneAsync(mainMenu);
    }
}

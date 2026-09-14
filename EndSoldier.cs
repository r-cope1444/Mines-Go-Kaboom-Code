using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

//The soldier who allows the player to complete the game
public class EndSoldier : MonoBehaviour, DataInterface
{
    private DataManager dataManager;
    private MineCounter mineCounter;
    private bool msgTimerOn;
    private bool gameDone;

    private int endScene = 2;

    public void LoadData(GameData data)
    {
        gameDone = data.gameFinished;
    }

    public void SaveData(ref GameData data)
    {
        data.gameFinished = gameDone;
    }

    private void Start()
    {
        dataManager = FindFirstObjectByType<DataManager>();
        mineCounter = FindFirstObjectByType<MineCounter>();
    }

    public void CheckForMines(TextMeshProUGUI notClearedMines)
    {
        int mineCount = mineCounter.GetMineCount();
        if (mineCount == 0)
        {
            EndSequence();
        }
        else
        {
            if (!msgTimerOn)
            {
                msgTimerOn = true;
                StartCoroutine(ClearMsgTimer(notClearedMines, mineCount));
            }
        }
    }

    private void EndSequence()
    {
        gameDone = true;
        dataManager.SaveGame();
        SceneManager.LoadSceneAsync(endScene);
    }

    private IEnumerator ClearMsgTimer(TextMeshProUGUI notClearedMines, int mineCount)
    {
        notClearedMines.enabled = true;
        notClearedMines.text = $"There are still {mineCount} mines left!";
        yield return new WaitForSeconds(5f);
        notClearedMines.text = " ";
        notClearedMines.enabled = false;
        msgTimerOn = false;
    }
}

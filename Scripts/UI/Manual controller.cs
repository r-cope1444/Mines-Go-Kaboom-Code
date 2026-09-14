
using UnityEngine;
using UnityEngine.UI;

//Controls the in game manual
public class Manualcontroller : MonoBehaviour
{
    [SerializeField] Sprite[] pages;
    [SerializeField] Image display;
    [SerializeField] GameObject menu;
    [SerializeField] AudioSource uiAudio;
    [SerializeField] AudioSource pageTurn;

    private int currentPage;

    void Start()
    {
        currentPage = 0;
        display.sprite = pages[currentPage];
    }

    private void OnEnable()
    {
        currentPage = 0;
        display.sprite = pages[currentPage];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (currentPage < pages.Length-1)
            {
                currentPage++;
                display.sprite = pages[currentPage];
                PlayPageAudio();
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (currentPage > 0)
            {
                currentPage--;
                display.sprite = pages[currentPage];
                PlayPageAudio();
            }
        }
    }

    public void ReturnToMenu()
    {
        uiAudio.Play();
        menu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void PlayPageAudio()
    {
        if (!pageTurn.isPlaying)
        {
            pageTurn.Play();
        }
    }
}

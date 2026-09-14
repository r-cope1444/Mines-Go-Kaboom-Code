
using UnityEngine;
using UnityEngine.UI;

//Handles game settings
public class Settings : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider sensitivitySlider;
    [SerializeField] GameObject menu;
    [SerializeField] AudioSource audioSource;


    private void OnEnable()
    {
        LoadPlayerPrefs();
    }

    private void OnDisable()
    {
        SavePlayerPrefs();
    }

    private void UpdateSensitivity()
    {
        PlayerPrefs.SetFloat("Look Sensitivity", 800f * sensitivitySlider.value);
        PlayerPrefs.Save();
        PlayerBehaviour playerBehaviour = GetComponentInParent<PlayerBehaviour>();
        if (playerBehaviour == null)
        {
            return;
        }
        playerBehaviour.sensitivity = 800f * sensitivitySlider.value;
    }

    private void UpdateVolume()
    {
        PlayerPrefs.SetFloat("Master Volume", volumeSlider.value);
        PlayerPrefs.Save();
        AudioListener.volume = volumeSlider.value;
    }

    public void ReturnToMenu()
    {
        audioSource.Play();
        menu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void LoadPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("Look Sensitivity"))
        {
            sensitivitySlider.value = PlayerPrefs.GetFloat("Look Sensitivity")/800f;
            PlayerBehaviour playerBehaviour = GetComponentInParent<PlayerBehaviour>();
            if (playerBehaviour == null)
            {
                return;
            }
            playerBehaviour.sensitivity = 800f * sensitivitySlider.value;
        }
        else
        {
            sensitivitySlider.value = 0.45f;
            PlayerPrefs.SetFloat("Look Sensitivity", 800f * sensitivitySlider.value);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("Master Volume"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("Master Volume");
            AudioListener.volume = volumeSlider.value;
        }
        else
        {
            volumeSlider.value = 0.45f;
            PlayerPrefs.SetFloat("Master Volume", volumeSlider.value);
            PlayerPrefs.Save();
        }
    }

    private void SavePlayerPrefs()
    {
        UpdateSensitivity();
        UpdateVolume();
    }
}

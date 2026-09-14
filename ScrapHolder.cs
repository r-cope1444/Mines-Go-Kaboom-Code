
using UnityEngine;
using TMPro;

//Displays number of scraps the player has
public class ScrapHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scrapText;

    private Money money;

    void Start()
    {
        money = FindFirstObjectByType<Money>();
    }

    
    void LateUpdate()
    {
        if (money != null)
        {
            scrapText.text = $"{money.ScrapCount()}";
        }
        else
        {
            money = FindFirstObjectByType<Money>();
        }
    }
}

using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    [Header("UI")]
    public TMP_Text coinText; // Assign in Inspector

    private int coinCount = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        UpdateUI();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateUI();
    }

public void RemoveCoin(int amount)
{
    coinCount -= amount;
    if (coinCount < 0) coinCount = 0; // prevent negative coins
    UpdateUI();
}


    void UpdateUI()
    {
        if (coinText != null)
        {
            coinText.text = "Coins: " + coinCount;
        }
        else
        {
            Debug.LogWarning("CoinManager: coinText not assigned!");
        }
    }
}


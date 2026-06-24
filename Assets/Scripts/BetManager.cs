using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Manages the player's bet selection.

// Cycles through fixed bet amounts: $100, $500, $1000.
public class BetManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI betText;

    [Header("Bet Settings")]
    private int[] betOptions = { 100, 500, 1000 };
    private int currentBetIndex = 1; // Default to $500

    // Public property so other scripts can read current bet
    public int CurrentBet => betOptions[currentBetIndex];

    private void Start()
    {
        UpdateBetDisplay();
    }
    // Called by the Plus button to increase bet amount
    public void IncreaseBet()
    {
        if (currentBetIndex < betOptions.Length - 1)
        {
            currentBetIndex++;
            UpdateBetDisplay();
        }
    }
 
    // Called by the Minus button to decrease bet.
    public void DecreaseBet()
    {
        if (currentBetIndex > 0)
        {
            currentBetIndex--;
            UpdateBetDisplay();
        }
    }

    // Display the bet 
    private void UpdateBetDisplay()
    {
        betText.text = "$" + CurrentBet.ToString();
    }
}
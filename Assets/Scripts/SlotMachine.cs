using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// Main slot machine controller.
/// Pre-determines outcome before animation for fair, controlled RNG.
/// Outcome probabilities mirror real slot machine design.
/// </summary>
public class SlotMachine : MonoBehaviour
{
    [Header("Reels")]
    [SerializeField] private Reel reel1;
    [SerializeField] private Reel reel2;
    [SerializeField] private Reel reel3;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private GameObject resultPopup;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI resultAmountText;

    [Header("Lever")]
    [SerializeField] private Button leverButton;

    // Player balance
    private int balance = 10000;

    // Is machine currently spinning?
    private bool isSpinning = false;

    // Reference to BetManager
    private BetManager betManager;

    // Payout multipliers - index matches symbol index
    private float[] twoMatchMultipliers  = { 1f, 1.5f, 2f, 3f };
    private float[] threeMatchMultipliers = { 5f, 10f, 25f, 50f };

    private void Start()
    {
        betManager = FindObjectOfType<BetManager>();
        UpdateBalanceDisplay();
        winText.gameObject.SetActive(false);
    }

    /// <summary>
    /// Called when lever is pulled.
    /// </summary>
    public void OnLeverPulled()
    {
        if (isSpinning) return;

        int currentBet = betManager.CurrentBet;

        if (balance < currentBet)
        {
            resultText.text = "INSUFFICIENT BALANCE!";
            resultAmountText.text = "";
            resultPopup.SetActive(true);
            return;
        }

        balance -= currentBet;
        UpdateBalanceDisplay();

        StartCoroutine(SpinAllReels(currentBet));
    }

    /// <summary>
    /// Pre-determines outcome, then runs reel animations.
    /// Reels stop sequentially for authentic feel.
    /// </summary>
    private IEnumerator SpinAllReels(int bet)
    {
        isSpinning = true;
        leverButton.interactable = false;
        winText.gameObject.SetActive(false);

        // Step 1 - Determine outcome BEFORE animation
        int[] results = PredetermineOutcome();
        reel1.SetPredeterminedResult(results[0]);
        reel2.SetPredeterminedResult(results[1]);
        reel3.SetPredeterminedResult(results[2]);

        Debug.Log("Outcome - Reel1: " + results[0] + 
                  " Reel2: " + results[1] + 
                  " Reel3: " + results[2]);

        // Step 2 - Staggered reel animations (authentic slot feel)
        StartCoroutine(reel1.Spin(1.5f));
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(reel2.Spin(1.8f));
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(reel3.Spin(2.1f));

        // Wait for all reels to finish
        yield return new WaitForSeconds(2.5f);

        // Step 3 - Evaluate result
        CheckWin(bet, results);

        isSpinning = false;
        leverButton.interactable = true;
    }

    
    /// Controls win probability distribution.
    /// Mirrors real slot machine outcome weighting.
    /// 
    /// Loss:        50%
    /// Two match:   25%
    /// 3x Cherry:   12%
    /// 3x Bell:      8%
    /// 3x Diamond:   3%
    /// 3x Seven:     2% (Jackpot)

    private int[] PredetermineOutcome()
    {
        int[] results = new int[3];
        int roll = Random.Range(0, 1000);

        if (roll < 500)
        {
            // 50% — Force a loss (all different symbols)
            results[0] = Random.Range(0, 4);
            do { results[1] = Random.Range(0, 4); } 
            while (results[1] == results[0]);
            do { results[2] = Random.Range(0, 4); } 
            while (results[2] == results[0] || results[2] == results[1]);
        }
        else if (roll < 750)
        {
            // 25% — Two matching symbols
            int symbol = GetWeightedSymbol();
            int other;
            do { other = Random.Range(0, 4); } while (other == symbol);

            // Place the odd symbol in a random reel position
            int oddPosition = Random.Range(0, 3);
            for (int i = 0; i < 3; i++)
                results[i] = (i == oddPosition) ? other : symbol;
        }
        else if (roll < 870)
        {
            // 12% — Three Cherry
            results[0] = results[1] = results[2] = 0;
        }
        else if (roll < 950)
        {
            // 8% — Three Bell
            results[0] = results[1] = results[2] = 1;
        }
        else if (roll < 980)
        {
            // 3% — Three Diamond
            results[0] = results[1] = results[2] = 2;
        }
        else
        {
            // 2% — Three Seven (JACKPOT)
            results[0] = results[1] = results[2] = 3;
        }

        return results;
    }

    /// <summary>
    /// Weighted symbol picker for two-match outcomes.
    /// Cherry most common, Seven rarest.
    /// </summary>
    private int GetWeightedSymbol()
    {
        int roll = Random.Range(0, 100);
        if (roll < 50) return 0; // Cherry
        if (roll < 80) return 1; // Bell
        if (roll < 95) return 2; // Diamond
        return 3;                 // Seven
    }

    /// <summary>
    /// Evaluates spin result and applies payout.
    /// </summary>
    private void CheckWin(int bet, int[] results)
    {
        int s1 = results[0];
        int s2 = results[1];
        int s3 = results[2];

        int winAmount = 0;

        if (s1 == s2 && s2 == s3)
        {
            // Three of a kind
            winAmount = Mathf.RoundToInt(bet * threeMatchMultipliers[s1]);
            bool isJackpot = (s1 == 3); // Only 3x Seven = Jackpot
            ShowWin(winAmount, isJackpot);
        }
        else if (s1 == s2 || s2 == s3 || s1 == s3)
        {
            // Two of a kind
            int matchedSymbol = (s1 == s2) ? s1 : (s2 == s3) ? s2 : s1;
            winAmount = Mathf.RoundToInt(bet * twoMatchMultipliers[matchedSymbol]);
            ShowWin(winAmount, false);
        }
        else
        {
            // No match
            ShowLose();
        }
    }

    private void ShowWin(int amount, bool isJackpot)
    {
        balance += amount;
        UpdateBalanceDisplay();

        winText.text = "YOU WIN! $" + amount;
        winText.gameObject.SetActive(true);

        resultText.text = isJackpot ? "JACKPOT!!!" : "YOU WIN!";
        resultAmountText.text = "+ $" + amount;
        resultPopup.SetActive(true);
    }

    private void ShowLose()
    {
        resultText.text = "TRY AGAIN!";
        resultAmountText.text = "Good Luck!!";
        resultPopup.SetActive(true);
    }

    /// <summary>
    /// Closes result popup. Called by OK button.
    /// </summary>
    public void ClosePopup()
    {
        resultPopup.SetActive(false);
        winText.gameObject.SetActive(false);
    }

    private void UpdateBalanceDisplay()
    {
        balanceText.text = "Balance:\n$" + balance.ToString();
    }
}
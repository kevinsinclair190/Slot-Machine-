using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

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

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip leverSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip jackpotSound;
    [SerializeField] private AudioClip loseSound;

    private int balance = 5000;
    private bool isSpinning = false;
    private BetManager betManager;

    private float[] twoMatchMultipliers  = { 1f, 1.5f, 2f, 3f };
    private float[] threeMatchMultipliers = { 5f, 10f, 25f, 50f };

    private void Start()
    {
        betManager = FindObjectOfType<BetManager>();
        UpdateBalanceDisplay();
        winText.gameObject.SetActive(false);
    }

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

        audioSource.PlayOneShot(leverSound);

        balance -= currentBet;
        UpdateBalanceDisplay();

        StartCoroutine(SpinAllReels(currentBet));
    }

    private IEnumerator SpinAllReels(int bet)
    {
        isSpinning = true;
        leverButton.interactable = false;
        winText.gameObject.SetActive(false);

        int[] results = PredetermineOutcome();
        reel1.SetPredeterminedResult(results[0]);
        reel2.SetPredeterminedResult(results[1]);
        reel3.SetPredeterminedResult(results[2]);

        Debug.Log("Outcome - Reel1: " + results[0] + 
                  " Reel2: " + results[1] + 
                  " Reel3: " + results[2]);

        StartCoroutine(reel1.Spin(1.5f));
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(reel2.Spin(1.8f));
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(reel3.Spin(2.1f));

        yield return new WaitForSeconds(2.5f);

        CheckWin(bet, results);

        isSpinning = false;
        leverButton.interactable = true;
    }

    private int[] PredetermineOutcome()
    {
        int[] results = new int[3];
        int roll = Random.Range(0, 1000);

        if (roll < 500)
        {
            results[0] = Random.Range(0, 4);
            do { results[1] = Random.Range(0, 4); } 
            while (results[1] == results[0]);
            do { results[2] = Random.Range(0, 4); } 
            while (results[2] == results[0] || results[2] == results[1]);
        }
        else if (roll < 750)
        {
            int symbol = GetWeightedSymbol();
            int other;
            do { other = Random.Range(0, 4); } while (other == symbol);

            int oddPosition = Random.Range(0, 3);
            for (int i = 0; i < 3; i++)
                results[i] = (i == oddPosition) ? other : symbol;
        }
        else if (roll < 870)
        {
            results[0] = results[1] = results[2] = 0;
        }
        else if (roll < 950)
        {
            results[0] = results[1] = results[2] = 1;
        }
        else if (roll < 980)
        {
            results[0] = results[1] = results[2] = 2;
        }
        else
        {
            results[0] = results[1] = results[2] = 3;
        }

        return results;
    }

    private int GetWeightedSymbol()
    {
        int roll = Random.Range(0, 100);
        if (roll < 50) return 0;
        if (roll < 80) return 1;
        if (roll < 95) return 2;
        return 3;
    }

    private void CheckWin(int bet, int[] results)
    {
        int s1 = results[0];
        int s2 = results[1];
        int s3 = results[2];

        int winAmount = 0;

        if (s1 == s2 && s2 == s3)
        {
            winAmount = Mathf.RoundToInt(bet * threeMatchMultipliers[s1]);
            bool isJackpot = (s1 == 3);
            ShowWin(winAmount, isJackpot);
        }
        else if (s1 == s2 || s2 == s3 || s1 == s3)
        {
            int matchedSymbol = (s1 == s2) ? s1 : (s2 == s3) ? s2 : s1;
            winAmount = Mathf.RoundToInt(bet * twoMatchMultipliers[matchedSymbol]);
            ShowWin(winAmount, false);
        }
        else
        {
            ShowLose();
        }
    }

    private void ShowWin(int amount, bool isJackpot)
    {
        balance += amount;
        UpdateBalanceDisplay();

        if (isJackpot)
            audioSource.PlayOneShot(jackpotSound);
        else
            audioSource.PlayOneShot(winSound);

        winText.text = "YOU WIN! $" + amount;
        winText.gameObject.SetActive(true);
        resultText.text = isJackpot ? "JACKPOT!!!" : "YOU WIN!";
        resultAmountText.text = "+ $" + amount;
        resultPopup.SetActive(true);
    }

    private void ShowLose()
    {
        audioSource.PlayOneShot(loseSound);
        resultText.text = "TRY AGAIN!";
        resultAmountText.text = "Good Luck!!";
        resultPopup.SetActive(true);
    }

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
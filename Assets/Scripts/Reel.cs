using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controls reel animation. Final result is pre-determined 
/// by SlotMachine before spin begins — true to real slot machine design.
/// </summary>
public class Reel : MonoBehaviour
{
    [Header("Symbol Sprites - 0=Cherry 1=Bell 2=Diamond 3=Seven")]
    [SerializeField] private Sprite[] symbols;

    [Header("UI Reference")]
    [SerializeField] private Image symbolImage;

    // Final result set by SlotMachine before animation starts
    public int CurrentSymbolIndex { get; private set; }

    /// <summary>
    /// Called by SlotMachine to set result before spinning.
    /// </summary>
    public void SetPredeterminedResult(int symbolIndex)
    {
        CurrentSymbolIndex = symbolIndex;
    }

    /// <summary>
    /// Plays spin animation then reveals predetermined result.
    /// </summary>
    public IEnumerator Spin(float duration)
    {
        float elapsed = 0f;
        float switchInterval = 0.08f;

        // Animate through random symbols
        while (elapsed < duration - 0.2f)
        {
            symbolImage.sprite = symbols[Random.Range(0, symbols.Length)];
            elapsed += switchInterval;
            yield return new WaitForSeconds(switchInterval);
        }

        // Reveal the predetermined result
        symbolImage.sprite = symbols[CurrentSymbolIndex];
    }
}
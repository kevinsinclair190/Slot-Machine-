using UnityEngine;

/// <summary>
/// ScriptableObject that defines a single slot symbol.
/// </summary>
[CreateAssetMenu(fileName = "SymbolData", menuName = "SlotMachine/SymbolData")]
public class SymbolData : ScriptableObject
{
    [Header("Symbol Info")]
    public string symbolName;
    public Sprite symbolSprite;
    public int symbolIndex; // 0=Cherry, 1=Bell, 2=Diamond, 3=Seven
}
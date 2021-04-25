using UnityEngine;

[CreateAssetMenu(fileName = "Rods", menuName = "Beach Fishing/Rods/Rod")]
public class RodSO : ScriptableObject
{
    public string rodName;
    public bool isUnlocked;
    public int Index;

    [Header("Modifiers")]
    [Tooltip("Catch the fish faster")]
    public float catchModifier=0;

}

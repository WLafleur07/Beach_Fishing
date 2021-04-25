using UnityEngine;

[CreateAssetMenu(fileName = "Fish", menuName = "Beach Fishing/Fish/Fish")]
public class FishSO : ScriptableObject
{
    public string fishName;
    public Sprite FishSprite;
    public Sprite QuestionMark;
    public float minSize;
    public float maxSize;
    public int index;
    public int caughtAmount; //The total amount caught for this fish

    [Header("Tag used to differentiate between Fish and Garbage")]
    public bool isGarbage;

    public bool isUnlocked = false;
    [Header("How many total fish must the player catch before this fish gets unlocked.")]
    public int unlockTreshold; 

    [Header("Most Common (White), Less Common (Bronze), Slightly Rare (Silver), Rare (Gold), Ultra Rare (Purple?)")]
    public Color fishCaughtParticleColor;

    [Header("Fish rarity: 1 (rarest) to 100 (most commom)")]
    [Tooltip("Fish rarity. 1(rarest) to 100(most commom)")]
    public float rarity=0;

    // TODO: figure out a suitable multiplier
    [Header("Difficulty multiplier (changes how fast the fish moves)")]
     public float difficultyMultiplier=0; // Basically changes the movement speed of the fish
    //Todo - Maybe also make the fish change direction more often as part of the difficulty increase
    //Another idea is to make the minigame progress bar fill slower or decrease faster
    //We can also have fish that use the randomized speed option on the minigame, those would be trickier fish to catch, more simple ones could use constant speed
   
}

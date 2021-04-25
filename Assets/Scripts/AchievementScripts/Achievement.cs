using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Achievements", menuName = "Achievements/achievement")]
public class Achievement : ScriptableObject
{
    public enum AWARD_TYPES
    {
        TWITTER_POSTS,

        FACEBOOK_POSTS,

        FACEBOOK_LOGIN,

        BADGES_VIEWED,

        BADGES_COLLECTED,

        MAPS_CLICKED,

        PLAYED_FISHING,

        PLAYED_ORCHARD_DROP,

        PLAYED_UCTABLE,

        PLAYED_GHOSTHUNT,

        PLAYED_ALL,

        UCV_LEVEL1_COMPLETED,

        UCV_LEVEL2_COMPLETED,

        UCV_LEVEL3_COMPLETED,

        // Beach Fishing
        BF_RAREST_FISH_CAUGHT,

        BF_LEGENDARY_FISH_CAUGHT,

        BF_GOLD_FISH_CAUGHT,

        BF_SILVER_FISH_CAUGHT,

        BF_BRONZE_FISH_CAUGHT,

        BF_FISH_CAUGHT,

        BF_UNLOCK_ROD,

        BF_UNLOCK_BAIT,

        BF_UNLOCK_ALL_RODS,

        BF_UNLOCK_ALL_BAITS,

        BF_RAIN,

        BF_CATCH_TRASH,
        // End Of

        UCV_LEVEL1_TARGET_TIME,

        UCV_LEVEL2_TARGET_TIME,

        UCV_LEVEL3_TARGET_TIME,

        NUM_AWARD_TYPES,

        // Orchard Drops
        OD_PLAY_ONE,

        OD_PLAY_TEN,

        OD_PLAY_FIFTY,

        OD_PLAY_HUNDRED,

        OD_CATCH_HUNDRED,

        OD_CATCH_THOUSAND,

        OD_CATCH_TENTHOUSAND,

        OD_CATCH_HUNDREDTHOUSAND,

        OD_BUY_SKIN,

        OD_TITLE_BEGINNER,

        OD_TITLE_NOVICE,

        OD_TITLE_SKILLED,

        OD_TITLE_PRO,

        OD_TITLE_ROBOT,
        
        // Ghost Hunt
        GH_FOUND_GHOST_BRONZE,

        GH_FOUND_GHOST_SILVER,

        GH_FOUND_GHOST_GOLD,

        GH_TIME_PLAYED_BRONZE,

        GH_TIME_PLAYED_SILVER,

        GH_TIME_PLAYED_GOLD,

        GH_ACTOR_CLICK_BRONZE,

        GH_ACTOR_CLICK_SILVER,

        GH_ACTOR_CLICK_GOLD,

        GH_POINTS_BRONZE,

        GH_POINTS_SILVER,

        GH_POINTS_GOLD,

        // ADD NEW AWARDS HERE 
        // |
        // |
        // V

    }

    public enum FILTER_INDEX
    {
        ALL = -1,

        APP = 0,
        UCV,
        BF,
        OD,
        GH,

        NUM_FILTERS
    }

    public int AchievementID;

    public string achName;
    public string achDescription;

    public Sprite achImage;

    public int TotalValues;

    public AWARD_TYPES type;

    public FILTER_INDEX filter;
}

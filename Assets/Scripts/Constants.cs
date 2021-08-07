using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants
{
    public enum MonoTag
    {
        PLAYER,
        INTERACTABLE,
        TILE,
        SNAPPING_DETECTOR,
        JUMPER_GAME_RUNNER,
        JUMPER_GAME_OBSTACLE,
        JUMPER_GAME_OBSTACLE_RECYCLER
    }

    public enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD
    }
}

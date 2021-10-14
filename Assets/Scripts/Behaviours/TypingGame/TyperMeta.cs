using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VegtableType
{
    BROCOLLI,
    CARROT,
    TOMATO
}

public class TyperWordSet
{
    public string[] words;
    public float maxTimerCount;
    public VegtableType vegtable;
    public float rounds
    {
        get
        {
            return this.words.Length;
        }
    }

    public TyperWordSet(string[] words, VegtableType vegtable, float maxTimerCount)
    {
        this.words = words;
        this.vegtable = vegtable;
        this.maxTimerCount = maxTimerCount;
    }
}

public class TyperMeta
{
    public static Dictionary<Constants.Difficulty, TyperWordSet> words = new Dictionary<Constants.Difficulty, TyperWordSet>() {
        {Constants.Difficulty.EASY, new TyperWordSet(
                new string[] {
                    "cope",
                    "bite",
                    "fund",
                    "grip",
                    "boil",
                    "shop",
                    "spin",
                    "head",
                    "hold",
                    "stem",
                    "fire",
                    "wrap",
                    "flow",
                    "stir",
                    "rise",
                    "care",
                    "slow",
                    "melt",
                    "calm",
                    "feed"
                },
                VegtableType.BROCOLLI,
                120f
            )
        },
        {Constants.Difficulty.MEDIUM, new TyperWordSet(
                new string[] {
                    "alert",
                    "sense",
                    "weigh",
                    "state",
                    "trade",
                    "cover",
                    "check",
                    "smoke",
                    "agree",
                    "doubt",
                    "admit",
                    "break",
                    "occur",
                    "shake",
                    "boost",
                    "react",
                    "stick",
                    "spill",
                    "prove",
                    "clean"
                },
                VegtableType.TOMATO,
                120f
            )
        },
        {Constants.Difficulty.HARD, new TyperWordSet(
                new string[] {
                    "tempt",
                    "think",
                    "worry",
                    "relax",
                    "waste",
                    "drift",
                    "shrug",
                    "frown",
                    "press",
                    "crush",
                    "limit",
                    "plant",
                    "argue",
                    "blame",
                    "solve",
                    "adapt",
                    "store",
                    "apply",
                    "speed",
                    "carve"
                },
                VegtableType.CARROT,
                120f
            )
        }
    };
    public static Dictionary<Constants.Difficulty, float> stress = new Dictionary<Constants.Difficulty, float>() {
        {Constants.Difficulty.EASY, 100f},
        {Constants.Difficulty.MEDIUM, 100f},
        {Constants.Difficulty.HARD, 100f},
    };
    public static Color traversedColor = new Color(0.0f, 0.9f, 0.1f, 1.0f);
    public static Color untraversedColor = new Color(1.0f, 0.1f, 0.0f, 1.0f);
}

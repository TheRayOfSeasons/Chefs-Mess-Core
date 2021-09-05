using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyperWord
{
    public string word;
    public int rounds;

    public TyperWord(string word, int rounds)
    {
        this.word = word;
        this.rounds = rounds;
    }
}

public class TyperMeta
{
    public static Dictionary<Constants.Difficulty, TyperWord> words = new Dictionary<Constants.Difficulty, TyperWord>() {
        {Constants.Difficulty.EASY, new TyperWord("brocolis", 3)},
        {Constants.Difficulty.MEDIUM, new TyperWord("tomatoes", 3)},
        {Constants.Difficulty.HARD, new TyperWord("carrots", 4)}
    };
    public static Color traversedColor = new Color(0.0f, 0.9f, 0.1f, 1.0f);
    public static Color untraversedColor = new Color(1.0f, 0.1f, 0.0f, 1.0f);
}

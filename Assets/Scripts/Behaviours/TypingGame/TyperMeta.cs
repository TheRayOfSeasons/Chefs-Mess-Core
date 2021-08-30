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
}

using System.Collections.Generic;

public class JumperMeta
{
    public static Dictionary<Constants.Difficulty, float> stress = new Dictionary<Constants.Difficulty, float>() {
        {Constants.Difficulty.EASY, 100f},
        {Constants.Difficulty.MEDIUM, 100f},
        {Constants.Difficulty.HARD, 100f},
    };
}

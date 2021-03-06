using System.Collections.Generic;

public class TilePuzzleMeta
{
    public static Dictionary<Constants.Difficulty, float> timerSettings = new Dictionary<Constants.Difficulty, float>() {
        {Constants.Difficulty.EASY, 180f},
        {Constants.Difficulty.MEDIUM, 180f},
        {Constants.Difficulty.HARD, 120f}
    };
    public static Dictionary<Constants.Difficulty, float> stress = new Dictionary<Constants.Difficulty, float>() {
        {Constants.Difficulty.EASY, 100f},
        {Constants.Difficulty.MEDIUM, 100f},
        {Constants.Difficulty.HARD, 100f},
    };
}

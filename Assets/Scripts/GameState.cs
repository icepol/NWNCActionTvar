using UnityEngine.SceneManagement;

public static class GameState
{
    public static int Lives = 3;
    public static int Score = 0;

    public static float Time = 60;

    public static int Level => int.Parse(SceneManager.GetActiveScene().name);

    public static void Reset()
    {
        Lives = 3;
        Score = 0;
        Time = 60;
    }
}

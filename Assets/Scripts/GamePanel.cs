using UnityEngine;
using UnityEngine.UI;

public class GamePanel : MonoBehaviour
{
    [SerializeField] private Text score;
    [SerializeField] private Text lives;
    [SerializeField] private Text timer;

    void Update()
    {
        score.text = GameState.Score.ToString();
        lives.text = $"x{GameState.Lives}";
        timer.text = $"{(GameState.Time < 10 ? "0" : "")}{(int)GameState.Time}";
    }
}

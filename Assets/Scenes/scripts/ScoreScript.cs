using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{

    public enum Score {
        Player1,
        Player2
    }

    public Text P1ScoreText, P2ScoreText;
    public int p1score, p2score;

    public void IncrementScore(Score score)
    {
        if (score == Score.Player1)
        {
            p1score++;
            P1ScoreText.text = p1score.ToString();
        }
        else
        {
            p2score++;
            P2ScoreText.text = p2score.ToString();
        }
    }
    
}

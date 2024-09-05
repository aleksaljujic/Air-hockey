using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreScript : MonoBehaviour
{

    public enum Score {
        Player1,
        Player2
    }

    public TMP_Text P1ScoreText, P2ScoreText;
    private int p1score, p2score;
    public int MaxScore = 3;
    public SceneScript sceneScript;

    private int P1Score
    {
        get { return p1score; }
        set
        {
            p1score = value;
            if (value == MaxScore)
                sceneScript.ShowRestartCanvas(true);
        }
    }

    private int P2Score
    {
        get { return p2score; }
        set
        {
            p2score = value;
            if (value == MaxScore)
                sceneScript.ShowRestartCanvas(false);
        }
    }

    public void IncrementScore(Score score)
    {
        if (score == Score.Player1)
        {
            P1ScoreText.text = (++P1Score).ToString();
        }
        else
        {
            P2ScoreText.text = (++P2Score).ToString();
        }
    }

    public void ResetScores()
    {
        P1Score = 0;
        P2Score = 0;
        P1ScoreText.text = "0";
        P2ScoreText.text = "0"; 
    }
    
}

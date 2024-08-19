using UnityEngine;
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
            P1ScoreText.text = p1score.ToString();
            if (p1score >= MaxScore)
            {
                sceneScript.SendGoalUpdate("P1");
                sceneScript.ShowRestartCanvas(true);
            }
        }
    }

    private int P2Score
    {
        get { return p2score; }
        set
        {
            p2score = value;
            P2ScoreText.text = p2score.ToString();
            if (p2score >= MaxScore) {
                sceneScript.SendGoalUpdate("P2");
                sceneScript.ShowRestartCanvas(false);
            }
                
        }
    }

    public void IncrementScore(Score score)
    {
        if (score == Score.Player1)
        {
            P1Score++;
        }
        else
        {
            P2Score++;
        }
    }
    public void UpdateScoreFromServer(string player, int newScore)
    {
        if (player == "P1")
        {
            P1Score = newScore;
        }
        else if (player == "P2")
        {
            P2Score = newScore;
        }
    }


    public void ResetScores()
    {
        P1Score = 0;
        P2Score = 0;
        P1ScoreText.text = "0";
        P2ScoreText.text = "0";

        sceneScript.SendScoreReset(); 
    }
    
}

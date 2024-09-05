using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreScript : MonoBehaviour
{
    public TMP_Text player1ScoreText;
    public TMP_Text player2ScoreText;

    private int player1Score = 0;
    private int player2Score = 0;

    public enum Score
    {
        Player1,
        Player2
    }

    public void IncrementScore(Score player)
    {
        if (player == Score.Player1)
        {
            player1Score++;
            player1ScoreText.text = "Player1 Score: " + player1Score;
        }
        else if (player == Score.Player2)
        {
            player2Score++;
            player2ScoreText.text = "Player2 Score: " + player2Score;
        }
    }

    public void ResetScores()
    {
        player1Score = 0;
        player2Score = 0;
        player1ScoreText.text = "Player1 Score: " + player1Score;
        player2ScoreText.text = "Player2 Score: " + player2Score;
    }
}

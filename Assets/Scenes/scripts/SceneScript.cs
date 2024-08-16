using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneScript : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject CanvasRestart;
    public GameObject CanvasStart;
    public TMP_Text Win;

    public PuckScript puckScript;
    public PlayerMovement playerMovement;
    public ScoreScript scoreScript;
    
    public void ShowRestartCanvas(bool p1Won)
    {
        Time.timeScale = 0;
        Canvas.SetActive(false);
        CanvasRestart.SetActive(true); 

        if (p1Won)
        {
            Win.text = "Player1 is winner!";
        }
        else
        {
            Win.text = "Player2 is winner!";
        }
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1;

        Canvas.SetActive(true);
        CanvasRestart.SetActive(false);
        scoreScript.ResetScores();
        puckScript.ResetPuck();
    }

    public void StartGame()
    {
        Time.timeScale = 1;

        Canvas.SetActive(true);
        CanvasRestart.SetActive(false);
        CanvasStart.SetActive(false);
    }
}

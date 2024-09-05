using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneScript : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject CanvasRestart;
    public GameObject CanvasStart;
    public GameObject CanvasLogin;
    public GameObject CanvasRegister;
    public TMP_Text Win;

    public PuckScript puckScript;
    public PlayerMovement playerMovement;
    public ScoreScript scoreScript;
    public TMP_InputField inputFieldUsername;
    public TMP_InputField inputFieldPassword;
    public TMP_Text userText;
    public TMP_Text WinsText;

    private int p1Wins = 0;
    public int P1Wins
    {
        get { return p1Wins; }
        set { p1Wins = value; }
    }

    void Start()
    {
        inputFieldPassword.contentType = TMP_InputField.ContentType.Password;
    }

    public void ShowRestartCanvas(bool p1Won)
    {
        Time.timeScale = 0;
        Canvas.SetActive(false);
        CanvasRestart.SetActive(true); 

        if (p1Won)
        {
            Win.text = userText.text + " is winner!";
            WinsText.text = (++P1Wins).ToString();
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
       if(inputFieldUsername.text != "" && inputFieldPassword.text != "")
        {
            Time.timeScale = 1;
            userText.text = inputFieldUsername.text;
            CanvasLogin.SetActive(false);
            Canvas.SetActive(true);
            CanvasRestart.SetActive(false);
            CanvasStart.SetActive(false);
        }
        else
        {
            CanvasLogin.SetActive(true);
        }
        
    }

    public void RegisterToLogin()
    {
        Time.timeScale = 0;

        CanvasLogin.SetActive(true);
        Canvas.SetActive(false);
        CanvasRestart.SetActive(false);
        CanvasStart.SetActive(false);
        CanvasRegister.SetActive(false);
    }
    public void LoginToRegister()
    {
        Time.timeScale = 0;
       
        CanvasRegister.SetActive(true);
        CanvasLogin.SetActive(false);
        Canvas.SetActive(false);
        CanvasRestart.SetActive(false);
        CanvasStart.SetActive(false);
    }
    public void LogOut()
    {
        Time.timeScale = 0;

        CanvasRegister.SetActive(false);
        CanvasLogin.SetActive(true);
        Canvas.SetActive(false);
        CanvasRestart.SetActive(false);
        CanvasStart.SetActive(false);
        inputFieldUsername.text = "";
        inputFieldPassword.text = "";
        WinsText.text = "0";
    }
}



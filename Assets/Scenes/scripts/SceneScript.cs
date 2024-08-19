using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//za client dodato
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
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
    // -----UDP klient stvari-------------------------------------------------------
    private UdpClient udpClient;//za slanje stvari
    private IPEndPoint serverEndPoint;//ip+port kasnije
    private Thread receivedThread;// da bi stalno slusali bez ometanja main threada
    //------------------------------------------------------------------------------

    void Start()
    {
        inputFieldPassword.contentType = TMP_InputField.ContentType.Password;
        //-----client initialization----
        InitializeUdpClient();
    }
    //metoda za inicijalizaciju--------------------------
    private void InitializeUdpClient() {
        udpClient = new UdpClient();
        serverEndPoint = new IPEndPoint(IPAddress.Parse("ovde ubaci ip adresu servera"), 12345);
        // thrad za slusanje
        receivedThread = new Thread(ReceiveData);
        receivedThread.Start();
        // da znamo ko se povezao na servreru
        SendMessage("connect:" + inputFieldUsername.text);
    }
    //---------------------------------------------------

    //metoda a slanje---------
    private void SendMessage(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        udpClient.Send(data, data.Length, serverEndPoint);
    }
    //-----------------------

    //metoda za prijem----------------------------
    private void ReceiveData()
    {
        while (true)
        {
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 12345);
            byte[] data = udpClient.Receive(ref remoteEP);

            
            string receivedMessage = Encoding.UTF8.GetString(data);
            Debug.Log("Received from server: " + receivedMessage);

            ProcessReceivedData(receivedMessage);
        }
    }
    // -------------------------------------------
    //metoda za obradjvanje poataka format type ,data
    private void ProcessReceivedData(string data) {
        string[] parts = data.Split(':');
        string messagetype=parts[0];
        string messagedata= parts.Length > 1 ? parts[1] : null;
        switch (messagedata)
        {
            case "player2_position":
                string[] player2PositionData = messagedata.Split(',');
                float player2PosX = float.Parse(player2PositionData[0]);
                float player2PosY = float.Parse(player2PositionData[1]);
                puckScript.UpdatePlayer2Position(new Vector2(player2PosX, player2PosY));
                break;


            case "puck":
                // Example: "puck:x,y" - Update puck position
                string[] positionData = messagedata.Split(',');
                float posX = float.Parse(positionData[0]);
                float posY = float.Parse(positionData[1]);
                puckScript.UpdatePuckPosition(new Vector2(posX, posY));
                break;

            case "goal":
                // Example: "goal:P1" or "goal:P2" - Update the score
                puckScript.HandleGoal(messagedata);
                break;

            case "reset":
                // Example: "reset" - Reset the game state
                puckScript.ResetPuckInGame();
                scoreScript.ResetScores();
                break;

                // Add more cases as needed for other types of messages
        }
    






    }
    void OnApplicationQuit()
    {
       
        receivedThread?.Abort();
        udpClient?.Close();
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
        puckScript.ResetPuckInGame();
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

            SendMessage("start_game:" + inputFieldUsername.text);
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


        SendMessage("logout:" + userText.text);
    }
    public void SendPlayerInput(Vector2 input)
    {
        string message = "move:" + input.x + "," + input.y;
        SendMessage(message);
    }

    public void SendGoalUpdate(string scoringPlayer)
    {
        string message = "goal:" + scoringPlayer;
        SendMessage(message);
    }

    public void SendPuckReset()
    {
        string message = "puck_reset";
        SendMessage(message);
    }

    public void SendScoreUpdate(string player, int score)
    {
        string message = "score_update:" + player + ":" + score;
        SendMessage(message);
    }

    public void SendScoreReset()
    {
        string message = "score_reset";
        SendMessage(message);
    }





}

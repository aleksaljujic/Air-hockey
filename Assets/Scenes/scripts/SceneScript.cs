using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

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
    public TMP_Text OnlinePlayersText;

    private int p1Wins = 0;
    public int P1Wins
    {
        get { return p1Wins; }
        set { p1Wins = value; }
    }

    // UDP client
    private UdpClient udpClient;
    private IPEndPoint serverEndPoint;
    private Thread receivedThread;

    void Start()
    {
        inputFieldPassword.contentType = TMP_InputField.ContentType.Password;
        InitializeUdpClient();
    }

    private void InitializeUdpClient()
    {
        udpClient = new UdpClient();
        serverEndPoint = new IPEndPoint(IPAddress.Parse("192.168.1.104"), 12345);
        receivedThread = new Thread(ReceiveData);
        receivedThread.Start();

        // Get the client's IP address
        string clientIpAddress = GetLocalIPAddress();
        if (!string.IsNullOrEmpty(clientIpAddress))
        {
            SendMessage("connect:" + clientIpAddress);
        }
        else
        {
            Debug.LogError("Failed to retrieve client IP address.");
        }
    }
    private string GetLocalIPAddress()
    {
        string localIP = string.Empty;
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error retrieving local IP address: " + ex.Message);
        }
        return localIP;
    }

    private void SendMessage(string message)
    {
        byte[] data = Encoding.ASCII.GetBytes(message);
        udpClient.Send(data, data.Length, serverEndPoint);
    }

    private void ReceiveData()
    {
        try
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
        catch (SocketException ex)
        {
            Debug.LogError("SocketException: " + ex.Message);
        }
        catch (ObjectDisposedException ex)
        {
            Debug.LogError("ObjectDisposedException: " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception: " + ex.Message);
        }
    }

    private void ProcessReceivedData(string data)
    {
        string[] parts = data.Split(':');
        if (parts.Length < 2) return;

        string messageType = parts[0];
        string messageData = parts[1];

        switch (messageType)
        {
            case "player2_position":
                string[] player2PositionData = messageData.Split(',');
                float player2PosX = float.Parse(player2PositionData[0]);
                float player2PosY = float.Parse(player2PositionData[1]);
                puckScript.UpdatePlayer2Position(new Vector2(player2PosX, player2PosY));
                break;

            case "puck":
                string[] positionData = messageData.Split(',');
                float posX = float.Parse(positionData[0]);
                float posY = float.Parse(positionData[1]);
                puckScript.UpdatePuckPosition(new Vector2(posX, posY));
                break;

            case "goal":
                puckScript.HandleGoal(messageData);
                break;

            case "reset":
                puckScript.ResetPuckInGame();
                scoreScript.ResetScores();
                break;

            case "online_players":
                //dodati kod
                break;
            case "connected":
                HandleConnectedToServer();
                break;
        }
    }
    private void HandleConnectedToServer()
    {
        Debug.Log("Successfully connected to the server!");

        // Hide or disable the ConnectCanvas
        if (true)
        {
            CanvasLogin.SetActive(false);
        }
        else { 
        // You can also enable other UI elements or perform additional setup here
        CanvasLogin.SetActive(true);
        }// For example, showing the login canvas
        }
        Int64  roomnom;

    public void InviteButton()
    {
        SendMessage("create_room:" + roomnom.ToString());
        roomnom++;
    }

    void OnApplicationQuit()
    {
        try
        {
            receivedThread?.Abort();
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception during thread abort: " + ex.Message);
        }
        finally
        {
            udpClient?.Close();
        }
    }

    public void ShowRestartCanvas(bool p1Won)
    {
        Time.timeScale = 0;
        Canvas.SetActive(false);
        CanvasRestart.SetActive(true);

        if (p1Won)
        {
            Win.text = userText.text + " is the winner!";
            WinsText.text = (++P1Wins).ToString();
        }
        else
        {
            Win.text = "Player2 is the winner!";
        }
    }

    

    public void StartGame()
    { 
        if (!string.IsNullOrEmpty(inputFieldUsername.text) && !string.IsNullOrEmpty(inputFieldPassword.text))
        {
            Time.timeScale = 1;
            userText.text = inputFieldUsername.text;
            CanvasLogin.SetActive(false);
            Canvas.SetActive(true);
            CanvasRestart.SetActive(false);
            CanvasStart.SetActive(false);

            SendMessage("login:" + inputFieldUsername.text+"!"+inputFieldPassword.text);
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

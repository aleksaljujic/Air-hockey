using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PuckScript : MonoBehaviour   
    {
    public ScoreScript scs;
    public static bool goal{ get; private set; }
    private Rigidbody2D rb;
    public Transform Player1 , Player2;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        goal = false;
    }

    public void UpdatePuckPosition(Vector2 newPosition)
    {
        rb.MovePosition(newPosition);
    }
    public void UpdatePlayer2Position(Vector2 newPosition)
    {
        Player2.position = new Vector3(newPosition.x, newPosition.y, Player2.position.z);
    }



    public void ResetPuckInGame()
    {
        goal = false;
        rb.velocity = rb.position = new Vector2(0, 0);
        Player1.position = new Vector2(-0.007f, -1f);
        Player2.position = new Vector2(-0.007f, -1f);

    }
    public void HandleGoal(string scoringPlayer)
    {
        if (scoringPlayer == "P1")
        {
            scs.IncrementScore(ScoreScript.Score.Player1);
        }
        else if (scoringPlayer == "P2")
        {
            scs.IncrementScore(ScoreScript.Score.Player2);
        }
    }



}

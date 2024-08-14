using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuckScript : MonoBehaviour   
    {
    public ScoreScript scs;
    public static bool goal{ get; private set; }
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        goal = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!goal) 
        {
            if(collider.tag == "P2Goal")
            {
                scs.IncrementScore(ScoreScript.Score.Player1);
                goal = true;
                StartCoroutine(ResetPuck());
            }
            else if(collider.tag == "P1Goal")
            {
                scs.IncrementScore(ScoreScript.Score.Player2);
                goal = true;
                StartCoroutine(ResetPuck());
            }
        }
    }

    private IEnumerator ResetPuck()
    {
        yield return new WaitForSecondsRealtime(1);
        goal = false;
        rb.velocity = rb.position = new Vector2(0.6f, 0.6f);
    }
}

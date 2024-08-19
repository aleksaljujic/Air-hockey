using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    bool clicked = true;
    bool canMove;
    Collider2D playerCollider;

    Rigidbody2D rb;

    public Transform BoundaryHolders;
    public SceneScript sceneScript;

    Boundary playerBoundary;

    struct Boundary
    {
        public float Up, Down, Left, Right;

        public Boundary(float up, float down, float left, float right)
        {
            this.Up = up;
            this.Down = down;      
            this.Left = left;
            this.Right = right;
        }
    }

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerBoundary = new Boundary(BoundaryHolders.GetChild(0).position.y,
                                      BoundaryHolders.GetChild(1).position.y,
                                      BoundaryHolders.GetChild(2).position.x,
                                      BoundaryHolders.GetChild(3).position.x);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (clicked)
            {
                clicked = false;

                if(playerCollider.OverlapPoint(mousePosition))
                {
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
            }

            if (canMove)
            {
                Vector2 clampedMousePos = new Vector2(Mathf.Clamp(mousePosition.x, playerBoundary.Left,
                                                                  playerBoundary.Right),
                                                      Mathf.Clamp(mousePosition.y, playerBoundary.Down,
                                                                  playerBoundary.Up));
                rb.MovePosition(clampedMousePos);

                //client deo-------------------------
                sceneScript.SendPlayerInput(clampedMousePos);
            }
        }
        else
        {
            clicked = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 startingPosition;
    public float speed = 5f;
    public bool isInverted;
    public int playerDirection;
    public Transform tileCheck;
    public LayerMask obstructionTile, waterTile, upTile, downTile, leftTile, rightTile, teleporterTile, inverterTile;
    public Animator anime;
    // Start is called before the first frame update
    void Start()
    {
        tileCheck.parent = null;
        playerDirection = 1;
        isInverted = false;
        startingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, tileCheck.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, tileCheck.position) == 0f)
        {
            anime.ResetTrigger("isMoving");
            CheckCurrentTile();
            if ((Input.GetKey(KeyCode.LeftArrow)) || (Input.GetKey(KeyCode.A)))
            {
                if (!isInverted)
                {
                    ChangeDirection(2, false);
                    Movement(new Vector3(-1f, 0f, 0f));
                }
                else 
                {
                    ChangeDirection(4, false);
                    Movement(new Vector3(1f, 0f, 0f));
                }
            }
            else if ((Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D)))
            {
                if (!isInverted)
                {
                    ChangeDirection(4, false);
                    Movement(new Vector3(1f, 0f, 0f));
                }
                else 
                {
                    ChangeDirection(2, false);
                    Movement(new Vector3(-1f, 0f, 0f));
                }
            }
            else if ((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.W)))
            {
                if (!isInverted)
                {
                    ChangeDirection(3, false);
                    Movement(new Vector3(0f, 1f, 0f));
                }
                else 
                {
                    ChangeDirection(1, false);
                    Movement(new Vector3(0f, -1f, 0f));
                }
            }
            else if ((Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.S)))
            {
                if (!isInverted)
                {
                    ChangeDirection(1, false);
                    Movement(new Vector3(0f, -1f, 0f));
                }
                else 
                {
                    ChangeDirection(3, false);
                    Movement(new Vector3(0f, 1f, 0f));
                }
            }
        }
    }

    private void ChangeDirection(int newDirection, bool isRelative)
    {
        if (isRelative) 
        {
            playerDirection += newDirection;
        }
        else 
        {
            playerDirection = newDirection;
        }
        anime.SetInteger("playerDirection", playerDirection);
    }

    private void CheckCurrentTile()
    {
        if(Physics2D.OverlapCircle(tileCheck.position, 0.1f, inverterTile))
        {
            isInverted = !isInverted;
        }
        else if(Physics2D.OverlapCircle(tileCheck.position, 0.1f, upTile))
        {
            ChangeDirection(3,false);
            Movement(new Vector3(0f, 1f, 0f));
        }
        else if(Physics2D.OverlapCircle(tileCheck.position, 0.1f, leftTile))
        {
            ChangeDirection(2,false);
            Movement(new Vector3(-1f, 0f, 0f));
        }
        else if(Physics2D.OverlapCircle(tileCheck.position, 0.1f, rightTile))
        {
            ChangeDirection(4,false);
            Movement(new Vector3(1f, 0f, 0f));
        }
        else if(Physics2D.OverlapCircle(tileCheck.position, 0.1f, downTile))
        {
            ChangeDirection(1,false);
            Movement(new Vector3(0f, -1f, 0f));
        }
        else if(Physics2D.OverlapCircle(tileCheck.position, 0.1f, teleporterTile))
        {
            tileCheck.position = startingPosition;
            transform.position = startingPosition;
        }
    }

    private void Movement(Vector3 moveDirection)
    {
        Vector3 checkTile = tileCheck.position + moveDirection;
        Debug.Log(Physics2D.OverlapCircle(checkTile, 0.2f, obstructionTile));
        
        if(!Physics2D.OverlapCircle(checkTile, 0.1f, obstructionTile))
        {
            anime.SetTrigger("isMoving");
            tileCheck.position = checkTile;
            Debug.Log(Physics2D.OverlapCircle(checkTile, 0.2f, waterTile));
            if(Physics2D.OverlapCircle(checkTile, 0.1f, waterTile))
            {
                Movement(moveDirection);
            }
        }
        else 
        {
            anime.SetTrigger("isTurning");
        }
        
    }
}

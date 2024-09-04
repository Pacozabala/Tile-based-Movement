using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 startingPosition;
    public float speed = 5f;
    public bool isInverted, inputEnabled;
    public int playerDirection;
    public Transform destinationTile;
    public LayerMask obstructionTile, slidingTile, upTile, downTile, leftTile, rightTile, teleporterTile, inverterTile;
    public Animator anime;
    // Start is called before the first frame update
    void Start()
    {
        destinationTile.parent = null;
        playerDirection = 1;
        isInverted = false;
        startingPosition = transform.position;
        inputEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationTile.position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, destinationTile.position) == 0f)
        {
            anime.ResetTrigger("isMoving");
            TriggerEffect();
            if(inputEnabled)
            {
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

    private void TriggerEffect()
    {
        if(Physics2D.OverlapCircle(destinationTile.position, 0.1f, inverterTile))
        {
            isInverted = !isInverted;
        }
        else if(Physics2D.OverlapCircle(destinationTile.position, 0.1f, upTile))
        {
            inputEnabled = false;
            ChangeDirection(3,false);
            Movement(new Vector3(0f, 1f, 0f));
        }
        else if(Physics2D.OverlapCircle(destinationTile.position, 0.1f, leftTile))
        {
            inputEnabled = false;
            ChangeDirection(2,false);
            Movement(new Vector3(-1f, 0f, 0f));
        }
        else if(Physics2D.OverlapCircle(destinationTile.position, 0.1f, rightTile))
        {
            inputEnabled = false;
            ChangeDirection(4,false);
            Movement(new Vector3(1f, 0f, 0f));
        }
        else if(Physics2D.OverlapCircle(destinationTile.position, 0.1f, downTile))
        {
            inputEnabled = false;
            ChangeDirection(1,false);
            Movement(new Vector3(0f, -1f, 0f));
        }
        else if(Physics2D.OverlapCircle(destinationTile.position, 0.1f, teleporterTile))
        {
            destinationTile.position = startingPosition;
            transform.position = startingPosition;
        }
        else 
        {
            inputEnabled = true;
        }
    }

    private void Movement(Vector3 moveDirection)
    {
        Vector3 checkTile = destinationTile.position + moveDirection;
        if(!Physics2D.OverlapCircle(checkTile, 0.1f, obstructionTile))
        {
            anime.SetTrigger("isMoving");
            destinationTile.position = checkTile;
            if(Physics2D.OverlapCircle(checkTile, 0.1f, slidingTile))
            {
                Movement(moveDirection);
            }
        }
        else 
        {
            inputEnabled = true;
            anime.SetTrigger("isTurning");
        }
        
    }
}

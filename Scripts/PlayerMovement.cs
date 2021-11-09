using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Direction directionFacing;

    [SerializeField] private GameObject obstaclesDetector;
    [SerializeField] private LayerMask obstaclesLayerMask;

    [HideInInspector] public bool canMakeAnAction;

    void Start()
    {
        directionFacing = Direction.up;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        canMakeAnAction = true;
    }

    void Update()
    {
    }


    public void GoUp()
    {
        if(canMakeAnAction)
        {
            if(directionFacing == Direction.up)
            {
                if (!DetectObstacles())
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                    canMakeAnAction = false;
                    StartCoroutine(GameManager.instance.EnemiesTurn());
                }
            }
            else
            {
                directionFacing = Direction.up;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }

    public void GoDown()
    {
        if(canMakeAnAction)
        {
            if (directionFacing == Direction.down)
            {
                if (!DetectObstacles())
                {
                    transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                    canMakeAnAction = false;
                    StartCoroutine(GameManager.instance.EnemiesTurn());
                }
            }
            else
            {
                directionFacing = Direction.down;
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
        }
    }

    public void GoLeft()
    {
        if(canMakeAnAction)
        {
            if (directionFacing == Direction.left)
            {
                if (!DetectObstacles())
                {
                    transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                    canMakeAnAction = false;
                    StartCoroutine(GameManager.instance.EnemiesTurn());
                }
            }
            else
            {
                directionFacing = Direction.left;
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
        }
    }

    public void GoRight()
    {
        if(canMakeAnAction)
        {
            if (directionFacing == Direction.right)
            {
                if (!DetectObstacles())
                {
                    transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                    canMakeAnAction = false;
                    StartCoroutine(GameManager.instance.EnemiesTurn());
                }
            }
            else
            {
                directionFacing = Direction.right;
                transform.rotation = Quaternion.Euler(0, 0, 270);
            }
        }
    }

    public void GameOver()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        StartCoroutine(GameManager.instance.RestartLevel());
    }


    private bool DetectObstacles()
    {
        Collider2D[] obstacles = Physics2D.OverlapPointAll(obstaclesDetector.transform.position, obstaclesLayerMask);
        if (obstacles.Length > 0) return true;
        else return false;
    }


    private enum Direction
    {
        up,
        down,
        left,
        right
    }
}

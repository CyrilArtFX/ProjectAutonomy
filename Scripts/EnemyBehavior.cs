using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [HideInInspector] public GameObject player;
    [HideInInspector] public LayerMask playerLayerMask;

    [HideInInspector] public bool actionFinished;
    [HideInInspector] public Direction directionFacing;

    public GameObject obstaclesDetector;
    public LayerMask obstaclesLayerMask;

    public GameObject damagesZonesObject;
    [HideInInspector] public List<GameObject> damagesZones = new List<GameObject>();

    public void Start()
    {
        player = GameManager.instance.player;
        playerLayerMask = GameManager.instance.playerLayerMask;
        actionFinished = true;
        directionFacing = Direction.up;

        for (int i = 0; i < damagesZonesObject.transform.childCount; i++)
        {
            damagesZones.Add(damagesZonesObject.transform.GetChild(i).gameObject);
        }
        foreach (GameObject damageZone in damagesZones) damageZone.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Update()
    {
    }

    public void Action()
    {
        if(IsPlayerInDetectionRange())
        {
            if(IsPlayerInAttackRange())
            {
                StartCoroutine(ShowDamagesZones());
                GameManager.instance.playerMovementScript.GameOver();
            }
            else
            {
                if (Mathf.Abs(transform.position.x - player.transform.position.x) >= Mathf.Abs(transform.position.y - player.transform.position.y)) //Distance horizontale avec le joueur > distance verticale
                {
                    if (transform.position.x > player.transform.position.x) //A droite du joueur (aller � gauche)
                    {
                        directionFacing = Direction.left;
                        transform.rotation = Quaternion.Euler(0, 0, 90);
                        if (!DetectObstacles()) transform.position = new Vector2(transform.position.x - 1, transform.position.y);
                    }
                    else //A gauche du joueur (aller � droite)
                    {
                        directionFacing = Direction.right;
                        transform.rotation = Quaternion.Euler(0, 0, 270);
                        if (!DetectObstacles()) transform.position = new Vector2(transform.position.x + 1, transform.position.y);
                    }
                }
                else //Distance horizontale avec le joueur < distance verticale
                {
                    if (transform.position.y > player.transform.position.y) //En haut du joueur (aller en bas)
                    {
                        directionFacing = Direction.down;
                        transform.rotation = Quaternion.Euler(0, 0, 180);
                        if (!DetectObstacles()) transform.position = new Vector2(transform.position.x, transform.position.y - 1);
                    }
                    else //En bas du joueur (aller en haut)
                    {
                        directionFacing = Direction.up;
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        if (!DetectObstacles()) transform.position = new Vector2(transform.position.x, transform.position.y + 1);
                    }
                }
            }
        }
        else
        {
            int r = Random.Range(0, 4);
            if(r == 0) //Aller à gauche
            {
                directionFacing = Direction.left;
                transform.rotation = Quaternion.Euler(0, 0, 90);
                if (!DetectObstacles()) transform.position = new Vector2(transform.position.x - 1, transform.position.y);
            }
            else if (r == 1) //Aller à droite
            {
                directionFacing = Direction.right;
                transform.rotation = Quaternion.Euler(0, 0, 270);
                if (!DetectObstacles()) transform.position = new Vector2(transform.position.x + 1, transform.position.y);
            }
            else if (r == 2) //aller en bas
            {
                directionFacing = Direction.down;
                transform.rotation = Quaternion.Euler(0, 0, 180);
                if (!DetectObstacles()) transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            }
            else //aller en haut
            {
                directionFacing = Direction.up;
                transform.rotation = Quaternion.Euler(0, 0, 0);
                if (!DetectObstacles()) transform.position = new Vector2(transform.position.x, transform.position.y + 1);
            }
        }

        actionFinished = true;
    }


    public void Damage()
    {
        GameManager.instance.DeleteFromEnemiesList(this.gameObject);
        Destroy(gameObject);
    }

    public bool IsPlayerInDetectionRange()
    {
        Collider2D[] playerDetected = Physics2D.OverlapCircleAll(transform.position, transform.GetChild(0).gameObject.GetComponent<CircleCollider2D>().radius, playerLayerMask);
        if (playerDetected.Length > 0) return true;
        else return false;
    }

    public bool DetectObstacles()
    {
        Collider2D[] obstacles = Physics2D.OverlapPointAll(obstaclesDetector.transform.position, obstaclesLayerMask);
        if (obstacles.Length > 0) return true;
        else return false;
    }

    public bool IsPlayerInAttackRange()
    {
        bool isPlayerDetected = false;
        foreach(GameObject damageZone in damagesZones)
        {
            Collider2D[] playerDetected = Physics2D.OverlapPointAll(damageZone.transform.position, playerLayerMask);
            if (playerDetected.Length > 0) isPlayerDetected = true;
        }
        return isPlayerDetected;
    }

    public IEnumerator ShowDamagesZones()
    {
        foreach (GameObject damageZone in damagesZones) damageZone.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject damageZone in damagesZones) damageZone.GetComponent<SpriteRenderer>().enabled = false;
    }


    public enum Direction
    {
        up,
        down,
        left,
        right
    }
}

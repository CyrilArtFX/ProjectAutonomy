using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [HideInInspector] public PlayerMovement playerMovement;

    [SerializeField] private GameObject damagesZonesObject;
    [HideInInspector] public LayerMask enemyLayerMask;

    [HideInInspector] public List<GameObject> damagesZones = new List<GameObject>();

    public virtual void Start()
    {
        playerMovement = GameManager.instance.playerMovementScript;
        enemyLayerMask = GameManager.instance.enemyLayerMask;

        for(int i = 0; i < damagesZonesObject.transform.childCount; i++)
        {
            damagesZones.Add(damagesZonesObject.transform.GetChild(i).gameObject);
        }
        foreach (GameObject damageZone in damagesZones) damageZone.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void Update()
    {
        
    }

    public virtual void UseWeapon()
    {
        if(playerMovement.canMakeAnAction)
        {
            StartCoroutine(ShowDamagesZones());
            foreach(GameObject damageZone in damagesZones)
            {
                Collider2D[] enemies = Physics2D.OverlapPointAll(damageZone.transform.position, enemyLayerMask);
                foreach (Collider2D enemy in enemies) enemy.gameObject.GetComponent<EnemyBehavior>().Damage();
            }
            playerMovement.canMakeAnAction = false;
            StartCoroutine(GameManager.instance.EnemiesTurn());
        }
    }

    public IEnumerator ShowDamagesZones()
    {
        foreach (GameObject damageZone in damagesZones) damageZone.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject damageZone in damagesZones) damageZone.GetComponent<SpriteRenderer>().enabled = false;
    }
}

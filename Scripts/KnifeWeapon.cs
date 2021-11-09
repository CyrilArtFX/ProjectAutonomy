using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeWeapon : Weapon
{
    [HideInInspector] public GameObject player;

    public override void Start()
    {
        base.Start();
        player = GameManager.instance.player;
    }

    public override void UseWeapon()
    {
        if(playerMovement.canMakeAnAction)
        {
            StartCoroutine(ShowDamagesZones());
            foreach (GameObject damageZone in damagesZones)
            {
                Collider2D[] enemies = Physics2D.OverlapPointAll(damageZone.transform.position, enemyLayerMask);
                foreach (Collider2D enemy in enemies)
                {
                    player.transform.position = enemy.gameObject.transform.position;
                    enemy.gameObject.GetComponent<EnemyBehavior>().Damage();
                }
            }
            playerMovement.canMakeAnAction = false;
            StartCoroutine(GameManager.instance.EnemiesTurn());
        }
    }
}

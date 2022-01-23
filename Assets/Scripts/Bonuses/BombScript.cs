using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using UnityEngine;

public class BombScript : ShopItem
{

    public GameObject explosionAnimation;
    public int bombRange;
    [Tooltip("Percentage (0-1) - how much damage takes in percentage")]
    [Range(0, 1)]
    public float bombPercentageDamage;
    [Tooltip("How much absolute damage it takes (0-...)")]
    public float bombAbsoluteDamage;
    
    public override void RecalculateRange()
    {
        // to-do
    }

    public override bool PlaceObject()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, bombRange);
        List<GameObject> monsters = hitColliders.Select(collider2D1 => collider2D1.gameObject).ToList()
            .FindAll(coll => coll.CompareTag("Monster"))
            .OrderByDescending(gObj => gObj.GetComponent<MonsterMovingScript>().distanceTravelled).ToList();

        foreach (var monster in monsters)
        {
            monster.GetComponent<MonsterMovingScript>().ReduceHealth(bombAbsoluteDamage);
            monster.GetComponent<MonsterMovingScript>().SetHealthPercentage(bombPercentageDamage);
        }

        Instantiate(explosionAnimation, transform.position, transform.rotation);
        Destroy(gameObject);
        return true;
    }
}

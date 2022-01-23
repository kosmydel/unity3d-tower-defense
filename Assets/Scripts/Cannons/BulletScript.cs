using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cannons
{
    public class BulletScript : MonoBehaviour
    {

        public float speed, range, damage;
        private Vector2 _targetPosition;

        private void FixedUpdate()
        {
            transform.position =
                Vector2.MoveTowards(transform.position, _targetPosition,
                    Time.fixedDeltaTime*speed);
            if (transform.position == (Vector3) _targetPosition)
            {
                Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range);
                List<GameObject> monsters = hitColliders.Select(collider2D1 => collider2D1.gameObject).ToList()
                    .FindAll(coll => coll.CompareTag("Monster"))
                    .OrderByDescending(gObj => gObj.GetComponent<MonsterMovingScript>().distanceTravelled).ToList();

                foreach (var monster in monsters)
                {
                    monster.GetComponent<MonsterMovingScript>().ReduceHealth(damage);
                }
                Destroy(gameObject);
            }
        }

        public void SetTarget(Vector2 target)
        {
            _targetPosition = target;
        }
    
    }
}
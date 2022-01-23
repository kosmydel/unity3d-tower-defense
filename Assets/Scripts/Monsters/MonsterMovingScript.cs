using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class MonsterMovingScript : MonsterScript
{

    public Transform[] path;
    private int currentPathPoint = 0;

    public bool RotateOnPaths = false;
    
    private float health;
    [HideInInspector]
    public float distanceTravelled = 0;
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealth(startingHealth);
    }

    private void Awake()
    {
        path = isWaterPath ? GameManager.Instance.waterPath : GameManager.Instance.path;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Transform nextTransform = GetNextPosition();

        if (nextTransform == null)
        {
            Destroy(gameObject);
            GameManager.Instance.RemoveHealth();
            return;
        }
        
        Vector3 nextPosition = nextTransform.position;
        if (nextPosition == transform.position)
        {
            currentPathPoint++;
        }

        GameObject currentPath = Physics2D
            .OverlapPointAll(transform.position)
            .Select(c => c.gameObject)
            .FirstOrDefault(go => go.CompareTag("Path"));

        float speedModifier = currentPath != null ? currentPath.GetComponent<PathScript>().monsterSpeed : 1;
        
        Vector3 nextMove = Vector3.MoveTowards(transform.position, nextPosition, speed*speedModifier);
        if (RotateOnPaths)
        {
            transform.right = nextPosition - transform.position;
        }
        if (Physics2D.OverlapPointAll(nextMove).ToList().Exists(c => c.gameObject.CompareTag("Gate")))
        {
            return;
        }
        transform.position = nextMove;
        distanceTravelled+=speed;
    }

    Transform GetNextPosition()
    {
        if (currentPathPoint < path.Length)
        {
            return path[currentPathPoint];
        }
        return null;
    }
     
    public void ReduceHealth(float amount)
    {
            // Debug.Log("Reducing health" + amount);
        UpdateHealth(health - amount);
    }

    public void SetHealthPercentage(float newValue)
    {
        UpdateHealth(health*newValue);
    }
    
    public override void UpdateHealth(float newHealth)
    {
        if (newHealth < 0)
        {
            Destroy(gameObject);
            GameManager.Instance.AddMoney(prize);
            return;
        }   
        health = newHealth;
        // GetComponent<SpriteRenderer>().color = new Color(1, 1-(health/startingHealth)/2, 1-(health/startingHealth)/2);
        Color c = GetComponent<SpriteRenderer>().color;
        c.a = 0.75f + (health / startingHealth) / 4;
        GetComponent<SpriteRenderer>().color = c;
    }
}

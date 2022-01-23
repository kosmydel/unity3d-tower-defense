using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GateScript : ShopItem
{

    public float durationTime;
    
    public override void RecalculateRange()
    {
        
    }

    public void Awake()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    public override bool PlaceObject()
    {
        Vector2 position = transform.position;
        bool on = Utils.IsOnPath(position);
        bool top = Utils.IsOnPath(position + Vector2.up);
        bool right = Utils.IsOnPath(position + Vector2.right);
        bool bottom = Utils.IsOnPath(position + Vector2.down);
        bool left = Utils.IsOnPath(position + Vector2.left);
        if (on && (top && bottom) || (right && left))
        {
            GetComponent<Collider2D>().enabled = true;
            StartCoroutine(RemoveAfterDelay());
            return true;  
        }
        return false;
    }

    IEnumerator RemoveAfterDelay()
    {
        yield return new WaitForSeconds(durationTime);
        Destroy(gameObject);
    }
    public override bool SetPosition(Vector2 position)
    {
        bool on = Utils.IsOnPath(position);
        bool top = Utils.IsOnPath(position + Vector2.up);
        bool right = Utils.IsOnPath(position + Vector2.right);
        bool bottom = Utils.IsOnPath(position + Vector2.down);
        bool left = Utils.IsOnPath(position + Vector2.left);
        if(on)
        {
            if (top && bottom)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
                GetComponent<SpriteRenderer>().color = Color.white;
                transform.position = new Vector2(Mathf.Round(position.x+0.5f)-0.5f, position.y);
                return true;
            }
            if (right && left)
            {
                transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
                GetComponent<SpriteRenderer>().color = Color.white;
                transform.position = new Vector2(position.x, Mathf.Round(position.y+0.5f)-0.5f);
                return true;
            }
        }

        transform.position = position;
        return false;
    }
}

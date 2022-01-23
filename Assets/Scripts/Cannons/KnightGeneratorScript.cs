using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightGeneratorScript : ShopItem
{
    public override bool PlaceObject()
    {
        Vector2 position = transform.position;
        bool on = Utils.IsOnPath(position);
        bool top = Utils.IsOnPath(position + Vector2.up);
        bool right = Utils.IsOnPath(position + Vector2.right);
        bool bottom = Utils.IsOnPath(position + Vector2.down);
        bool left = Utils.IsOnPath(position + Vector2.left);
        if (!on && (top || right || bottom || left))
        {
            return true;
        }

        return false;
    }

    public override bool SetPosition(Vector2 position)
    {
        bool on = Utils.IsOnPath(position);
        bool top = Utils.IsOnPath(position + Vector2.up);
        bool right = Utils.IsOnPath(position + Vector2.right);
        bool bottom = Utils.IsOnPath(position + Vector2.down);
        bool left = Utils.IsOnPath(position + Vector2.left);

        if(!on && (top || right || bottom || left)) 
        {
            if (top)
            {
                transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
            }else if (right)
            {
                transform.rotation = Quaternion.AngleAxis(-90, Vector3.forward);
            }else if (bottom)
            {
                transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
            }else if (left)
            {
                transform.rotation = Quaternion.AngleAxis(90, Vector3.forward);
            }
            transform.position = new Vector2(Mathf.Round(position.x+0.5f)-0.5f, Mathf.Round(position.y+0.5f)-0.5f);
            return true;
        }
        transform.position = position;
        return false;
    }


    public override void RecalculateRange()
    {
        
    }
}

using System.Collections.Generic;
using System.Linq;
using Cannons;
using UnityEngine;

public class BasicCannon : ShopItem
{
    List <GameObject> currentCollisions = new List <GameObject> ();
    
    public float damage;
    public bool isSplashDamage, isDistantDamage;
    [Tooltip("Only works when isDistantDamage is true!")]
    public GameObject bullet;
    public float range;
    private float _rangeModifier = 1;
    public float damageInterval;
    public int upgradePrice;
    public GameObject upgradeTower;

    public Animation anim;

    [HideInInspector]
    public float totalDamage = 0;
    
    private Color _defaultColor;
    public Color collisionColor = Color.red;

    private bool _isMouseHover = false;
    private bool _isSelectedForUpgrade = false;
    private int radiusSegments = 50;

    private LineRenderer _lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DoDamage", damageInterval, damageInterval);
        RecalculateRange();
        RefreshRangeVisibility();
    }

    void Awake()
    {
        _lineRenderer = gameObject.GetComponent<LineRenderer>();
        _defaultColor = _lineRenderer.startColor;
    }

    void DoDamage()
    {
        if (!activated) return;

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, GetActualRange());
        List<GameObject> monsters = hitColliders.Select(collider2D1 => collider2D1.gameObject).ToList()
            .FindAll(coll => coll.CompareTag("Monster"))
            .OrderByDescending(gObj => gObj.GetComponent<MonsterMovingScript>().distanceTravelled).ToList();
        if (monsters.Count > 0 && isSplashDamage)
        {
            // GetComponent<Animator>().Play("Explode");
        }
        foreach (var monster in monsters)
        {
            if (!isSplashDamage || isDistantDamage)
            {
                transform.up = monster.transform.position - transform.position;
            }
            if (isDistantDamage)
            {
                GameObject spawnedBullet = Instantiate(bullet, transform.position, transform.rotation);
                spawnedBullet.GetComponent<BulletScript>().SetTarget(monster.transform.position);
                return;
            }
            monster.SendMessage("ReduceHealth", damage);
            totalDamage += damage;
            if (!isSplashDamage)
            {
                return;
            }
        }
    }

    void RefreshRangeVisibility()
    {
        if (_isMouseHover || _isSelectedForUpgrade)
        {
            _lineRenderer.enabled = true;
        }
        else
        {
            if (activated)
            {
                _lineRenderer.enabled = false;
            }   
        }
    }

    public override void RecalculateRange()
    {
        
        // Sprawdzam, czy jestem na górce
        Collider2D[] hitColliders = Physics2D.OverlapPointAll(transform.position);
        GameObject mountain = hitColliders.Select(collider2D1 => collider2D1.gameObject).ToList()
            .Find(coll => coll.CompareTag("Mountain"));
        if (mountain != null)
        {
            _rangeModifier = mountain.GetComponent<MountainScript>().rangeBoost;
        }
        else
        {
            _rangeModifier = 1f;
        }

        float x = 0;
        float y = 0;
        float z = -1f;

        float offsetX = transform.position.x;
        float offsetY = transform.position.y;

        _lineRenderer.positionCount = radiusSegments+1;
        _lineRenderer.useWorldSpace = true;
        for (int i = 0; i < (radiusSegments + 1); i++)
        {
            x = Mathf.Sin (Mathf.PI/radiusSegments*(2*i)) * (range * _rangeModifier) + offsetX;
            y = Mathf.Cos (Mathf.PI/radiusSegments*(2*i)) * (range * _rangeModifier) + offsetY;
                   
            _lineRenderer.SetPosition (i,new Vector3(x,y,z) );
        }
    }

    public float GetActualRange()
    {
        return range * _rangeModifier;
    }
    
    public override bool PlaceObject()
    {
        if (currentCollisions.Count == 0)
        {
            activated = true;
            RefreshRangeVisibility();
            return true;
        }

        return false;
    }

    public void CheckCollisions()
    {
        if(activated) return;
        
        if (currentCollisions.Count > 0)
        {
            SetLineColor(collisionColor);
        }
        else
        {
            SetLineColor(_defaultColor);
        }
    }

    private void SetLineColor(Color c)
    {
        _lineRenderer.endColor = c; 
        _lineRenderer.startColor = c; 
    } 
    
    void OnTriggerEnter2D (Collider2D col) {
        if (col.gameObject.CompareTag("Path") || col.gameObject.CompareTag("Cannon") || col.gameObject.CompareTag("Environment"))
        {
            currentCollisions.Add (col.gameObject);
            CheckCollisions();
        }
    }
 
    void OnTriggerExit2D (Collider2D col) {
        if (col.gameObject.CompareTag("Path") || col.gameObject.CompareTag("Cannon") || col.gameObject.CompareTag("Environment"))
        {
            currentCollisions.Remove (col.gameObject);   
            CheckCollisions();
        }
    }

    private void OnMouseDown()
    {
        if (activated)
        {
            GameManager.Instance.SelectTower(gameObject);
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("Showing..");
        _isMouseHover = true;
        RefreshRangeVisibility();
    }

    private void OnMouseExit()
    {
        _isMouseHover = false;
        RefreshRangeVisibility();
    }

    public void SetSelected(bool selected)
    {
        this._isSelectedForUpgrade = selected;
        RefreshRangeVisibility();
    }
    
}

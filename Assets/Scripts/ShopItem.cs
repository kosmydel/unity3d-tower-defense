using UnityEngine;

public abstract class ShopItem : MonoBehaviour
{

    public int price;
    public bool activated;
    public string displayName = "NAME";

    public abstract void RecalculateRange();
    public abstract bool PlaceObject();

    public virtual bool SetPosition(Vector2 position)
    {
        transform.position = position;
        return true;
    }

    public void CancelObject()
    {
        GameManager.Instance.AddMoney(this.price);
        Destroy(gameObject);
    }

}

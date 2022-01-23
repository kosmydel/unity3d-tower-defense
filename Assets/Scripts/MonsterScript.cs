using UnityEngine;

public abstract class MonsterScript : MonoBehaviour
{
    
    public float startingHealth = 20;
    public float speed = 4f;
    public int prize;
    public bool isWaterPath = false;
    public abstract void UpdateHealth(float newHealth);
    
}
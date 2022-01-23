using System.Linq;
using UnityEngine;

public class Utils : MonoBehaviour
{
    
    public static bool IsOnPath(Vector3 position)
    {
        return Physics2D.OverlapPointAll(position).ToList().Exists(c => c.gameObject.CompareTag("Path"));
    }
}
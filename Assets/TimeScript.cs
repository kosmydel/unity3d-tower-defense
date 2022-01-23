using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeScript : MonoBehaviour
{
    public float doubledSpeed = 1.75f;
    public Sprite play, doublePlay;

    public void ButtonPress()
    {
        Time.timeScale = Time.timeScale > 1f ? 1.0f : doubledSpeed;
        GetComponent<Image>().sprite = Time.timeScale > 1f ? doublePlay : play;
    }
    
}

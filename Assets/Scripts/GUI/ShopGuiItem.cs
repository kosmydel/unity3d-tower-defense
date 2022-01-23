using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UI;
using UnityEngine.XR;

public class ShopGuiItem : MonoBehaviour
{

    public GameObject item;
    public Button button;
    public Text labelText;
    public TextMeshProUGUI priceText;
    [Tooltip("Minimal time between buying another item")]
    public float buyTimeout;
    private float _timeLeft;

    private void Start()
    {
        GameManager.Instance.moneyUpdatedEvent.AddListener(HandleMoneyUpdated);
        _timeLeft = buyTimeout;
    }

    private void Awake()
    {
        ShopItem shopItem = item.GetComponent<ShopItem>();
        GetComponentInChildren<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
        GetComponentInChildren<Image>().color = item.GetComponent<SpriteRenderer>().color;
        priceText.text = shopItem.price + "$";
        labelText.text = shopItem.displayName;
    }

    private void FixedUpdate()
    {
        if (_timeLeft > 0)
        {
            Image image = GetComponentInChildren<Image>();
            _timeLeft -= Time.fixedDeltaTime;
            image.fillAmount = 1 - _timeLeft / buyTimeout;
        }
        else
        {
            if (!button.interactable)
            {
                HandleMoneyUpdated();
            }
        }
    }

    public void HandleMoneyUpdated()
    {
        int money = GameManager.Instance.money;
        if (money >= item.GetComponent<ShopItem>().price && _timeLeft <= 0)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }
    
    public void SelectItem()
    {
        ShopItem shopItem = item.GetComponent<ShopItem>();
        int currentMoney = GameManager.Instance.money;
        if (currentMoney >= shopItem.price)
        {
            GameManager.Instance.SetMoney(currentMoney - shopItem.price);
            GameObject newObject = Instantiate(item, transform.position, Quaternion.identity);
            GameManager.Instance.itemToBuy = newObject;
            _timeLeft = buyTimeout;
        }
    }

}

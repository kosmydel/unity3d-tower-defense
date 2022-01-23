using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanelScript : MonoBehaviour
{

    [HideInInspector] private GameObject _selectedTowerToUpgrade;
    public Image cannonIcon;
    public Text cannonName;
    public TextMeshProUGUI rangeText, damageText, totalDamageText, upgradePrice;
    public Button upgradeButton;

    private void Start()
    {
        GameManager.Instance.moneyUpdatedEvent.AddListener(HandleMoneyUpdate);
    }

    private void Awake()
    {
        InvokeRepeating("UpdateTotalDamage", 2f, 2f);
    }

    public void UpdateItems(GameObject basicCannon)
    {

        if (_selectedTowerToUpgrade != null)
        {
            _selectedTowerToUpgrade.GetComponent<BasicCannon>().SetSelected(false);
        }
        
        _selectedTowerToUpgrade = basicCannon;
        cannonIcon.sprite = _selectedTowerToUpgrade.GetComponent<SpriteRenderer>().sprite;
        BasicCannon basicCannonScript = _selectedTowerToUpgrade.GetComponent<BasicCannon>();
        basicCannonScript.SetSelected(true);
        cannonName.text = basicCannonScript.displayName;
        
        String rangeTextValue = "Range: " + basicCannonScript.range;
        String damageTextValue = "Dmg: " + basicCannonScript.damage + "/"+basicCannonScript.damageInterval+"s";
        UpdateTotalDamage();
        HandleMoneyUpdate();
        if (basicCannonScript.upgradeTower != null)
        {
            BasicCannon upgradedScript = basicCannonScript.upgradeTower.GetComponent<BasicCannon>();
            upgradePrice.text = basicCannonScript.upgradePrice + "$";
            
            string perSecondDamage = "";
            if (upgradedScript.damageInterval - basicCannonScript.damageInterval != 0)
            {
                perSecondDamage = "/"+(upgradedScript.damageInterval - basicCannonScript.damageInterval).ToString("F1")+"s";   
            }
            
            string dmgBonus = (upgradedScript.damage-basicCannonScript.damage).ToString("F1");
            damageTextValue += " (<color=green>+"+dmgBonus+perSecondDamage+"</color>)";
            
            if (upgradedScript.range-basicCannonScript.range != 0)
            {
                rangeTextValue += " (<color=green>+"+(upgradedScript.range-basicCannonScript.range).ToString("F1")+"</color>)";
            }
        }
        else
        {
            upgradePrice.text ="MAX";
        }

        rangeText.text = rangeTextValue;
        damageText.text = damageTextValue;
    }

    public void UpdateTotalDamage()
    {
        if(_selectedTowerToUpgrade == null) return;
        BasicCannon basicCannonScript = _selectedTowerToUpgrade.GetComponent<BasicCannon>();
        totalDamageText.text = "Total damage: " + basicCannonScript.totalDamage;
    }
    
    private void Update()
    {
        if (_selectedTowerToUpgrade != null && Input.GetMouseButtonDown(1))
        {
            _selectedTowerToUpgrade.GetComponent<BasicCannon>().SetSelected(false);
            _selectedTowerToUpgrade = null;
            GameManager.Instance.ShowShop();
        }
    }

    public void BuyItem()
    {
        BasicCannon basicCannonScript = _selectedTowerToUpgrade.GetComponent<BasicCannon>();
        if (basicCannonScript.upgradeTower == null)
        {
            return;
        }
        int money = GameManager.Instance.money;
        int cost = basicCannonScript.upgradePrice;
        if (money >= cost)
        {
            GameManager.Instance.AddMoney(-cost);
            GameObject gm = Instantiate(basicCannonScript.upgradeTower, _selectedTowerToUpgrade.transform.position,
                _selectedTowerToUpgrade.transform.rotation);
            gm.GetComponent<BasicCannon>().activated = true;
            gm.GetComponent<BasicCannon>().SetSelected(true);
            gm.GetComponent<BasicCannon>().totalDamage = basicCannonScript.totalDamage;
            Destroy(_selectedTowerToUpgrade);
            UpdateItems(gm);
        }
    }

    public void RemoveCannon()
    {
        Destroy(_selectedTowerToUpgrade);
        _selectedTowerToUpgrade = null;
        GameManager.Instance.ShowShop();
    }

    public void HandleMoneyUpdate()
    {
        if(_selectedTowerToUpgrade == null) return;
        
        BasicCannon basicCannonScript = _selectedTowerToUpgrade.GetComponent<BasicCannon>();
        if (basicCannonScript.upgradeTower != null && GameManager.Instance.money >= basicCannonScript.upgradePrice)
        {
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeButton.interactable = false;
        }
    }
    
}

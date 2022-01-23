using System.Collections;
using JsonLoader;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    [HideInInspector] public int spawnedMonsters = 0, wave = -1, waveStep = 0, waveMonsters = 0, delay = 0;
    [HideInInspector]
    public int money, health;
    public GameObject[] monsterPrefabs;
    public Transform[] path, waterPath;
    public Transform spawnLocation;

    public Color errorColor = Color.red;
        
    public Text moneyText, waveText, healthText;
    public GameObject chatBox;
    public CanvasGroup shopPanel, upgradePanel;
    
    [HideInInspector]
    public GameObject itemToBuy;

    public GameObject pauseMenu, gameOverPanel;
    public Transform monsterParent;

    private Level _currentLevel;
    
    private Vector3 mousePosition;

    public UnityEvent moneyUpdatedEvent;
    void Start()
    {
        SetMoneyText();
        UpdateHealthText();
        InvokeRepeating("SpawnMonster", 1, 0.1f);
        StartCoroutine(ShowMessage("Let the game begin!"));
        Time.timeScale = 1;
    }

    private void Awake()
    {
        if (moneyUpdatedEvent == null)
        {
            moneyUpdatedEvent = new UnityEvent();
        }
        _instance = this;
        _currentLevel = LoadLevelData("LevelEasy.json");
        // _currentLevel = LoadLevelData("LevelDemo.json");
        money = _currentLevel.startingMoney;
        health = _currentLevel.startingHealth;
        Debug.Log("Loaded level: " + _currentLevel.name + ", total waves: " + _currentLevel.waves.Length);
        UpdateWaveText();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameOverPanel.activeSelf)
            {
                gameOverPanel.SetActive(false);
            }
            if (Time.timeScale > 0)
            {
                Time.timeScale = 0;
                SetPauseMenuVisibility(true);
            }
            else
            {
                Time.timeScale = 1;
                SetPauseMenuVisibility(false);
            }
        }
        if (itemToBuy != null) {
            mousePosition = Input.mousePosition;
            mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            bool isCorrect = itemToBuy.GetComponent<ShopItem>().
                SetPosition(Vector2.Lerp(transform.position, mousePosition, 1));
            if (!isCorrect)
            {
                itemToBuy.GetComponent<SpriteRenderer>().color = errorColor;
            }
            else
            {
                itemToBuy.GetComponent<SpriteRenderer>().color = Color.white;
            }
            itemToBuy.SendMessage("RecalculateRange");
            if (Input.GetMouseButton(0))
            {
                if (itemToBuy.GetComponent<ShopItem>().PlaceObject())
                {
                    itemToBuy = null;
                }

            }

            if (Input.GetMouseButton(1))
            {
                itemToBuy.GetComponent<ShopItem>().CancelObject();
                itemToBuy = null;
            }
        }
    }

    void SpawnMonster()
    {
        WaveJson currentWave = GetCurrentWave();
        
        if(currentWave == null) return;

        Debug.Log($"Wave: {wave}, monsters: {waveStep} / {currentWave.monsters.Length}");
        
        if (waveStep >= currentWave.monsters.Length)
        {
            // If something is still alive -> return
            if (monsterParent.childCount > 0)
            {
                return;
            }
            wave++;
            UpdateWaveText();
            currentWave = GetCurrentWave();
            if (currentWave != null && currentWave.message != null)
            {
                StartCoroutine(ShowMessage(currentWave.message));
            }
            waveStep = 0;
            
            Debug.Log("Starting wave " + wave);
            return;
        }
        
        // Pomijamy krok, poniewaz delay jeszcze jest
        if (delay > 0)
        {
            delay--;
            return;
        }

        MonsterJson currentMonster = currentWave.monsters[waveStep];
        GameObject monster = currentMonster.monsterID < 0 ? null : monsterPrefabs[currentMonster.monsterID];
        if (waveMonsters < currentMonster.amount)
        {
            waveMonsters++;
            if(monster != null) 
                Instantiate(monster, spawnLocation.transform.position, Quaternion.identity, monsterParent);
            spawnedMonsters += 1;
            delay = currentMonster.delay;
            return;
        }

        waveMonsters = 0;
        waveStep += 1;
        Debug.Log("Wave: " + wave + ", step: " + waveStep + " delay: " + delay);
        
    }

    WaveJson GetCurrentWave()
    {
        if (wave >= 0 && wave < _currentLevel.waves.Length)
        {
            return _currentLevel.waves[wave];
        }

        return null;
    }

    public void AddMoney(int addedValue)
    {
        money += addedValue;
        SetMoneyText();
    }

    public void SetMoney(int value)
    {
        money = value;
        SetMoneyText();
    }
    
    public void SetMoneyText()
    {
        moneyUpdatedEvent.Invoke();
        if (moneyText != null)
        {
            moneyText.text = money.ToString();
        }
    }

    private void UpdateWaveText()
    {
        waveText.text = $"Wave: {wave+1}/{_currentLevel.waves.Length}";
    }
    
    private Level LoadLevelData(string levelName)
    {
 
        levelName = "Waves/" + levelName.Replace(".json", "");

        TextAsset textAsset = Resources.Load<TextAsset> (levelName);
        Level loadedLevel = JsonUtility.FromJson<Level>(textAsset.text);
        
        return loadedLevel;
    }

    public void SelectTower(GameObject basicCannon)
    {
        SetVisibility(upgradePanel, true);
        SetVisibility(shopPanel, false);
        
        upgradePanel.GetComponent<UpgradePanelScript>().UpdateItems(basicCannon);

    }
    
    public void ShowShop()
    {
        SetVisibility(upgradePanel, false);
        SetVisibility(shopPanel, true);
    }

    public void SetVisibility(CanvasGroup group, bool visibility)
    {
        group.alpha = (visibility ? 1f : 0f);
        group.blocksRaycasts = visibility;
        group.interactable = visibility;
    }

    public void RemoveHealth()
    {
        health -= 1;
        if (health < 1)
        {
            gameOverPanel.SetActive(true);
            Time.timeScale = 0;
        }
        UpdateHealthText();
    }
    public void UpdateHealthText()
    {
        healthText.text = health +"";
    }

    private void SetPauseMenuVisibility(bool visibility)
    {
        pauseMenu.SetActive(visibility);
    }

    IEnumerator ShowMessage(string message)
    {
        chatBox.GetComponentInChildren<TextMeshProUGUI>().text = message;
        chatBox.SetActive(true);
        yield return new WaitForSeconds(3f);
        chatBox.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Player player;
    public Text coinText;
    public Text timerText;
    public Image[] hearts;
    public Sprite isLife, nonLife, halfLife;
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    public TimeSettings timeSettings;
    float timer = 0f;
    public float countdawn;
    public GameObject invetory;
    public GameObject inventoryScript;
    public SoundEffector soundEffector;

    void Start()
    {
        if ((int)timeSettings == 2)
            timer = countdawn;

        Time.timeScale = 1;
    }

    void Update()
    {
        coinText.text = player.GetCoins().ToString();
        CheckHP();

        if ((int)timeSettings == 1)
        {
            timer += Time.deltaTime;
            timerText.text = timer.ToString("F2").Replace(",", ":");
        }
        else if ((int)timeSettings == 2)
        {
            timer -= Time.deltaTime;
            //timerText.text = timer.ToString("F2").Replace(",", ":");
            timerText.text = ((int)timer / 60).ToString() + ":" + ((int)timer -((int)timer / 60) * 60).ToString("D2");
            if (timer <= 0)
            {
                LoseGame();
            }
        }
        else
        {
            timerText.gameObject.SetActive(false);
        }
    }

    void CheckHP()
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if (player.GetHP() > i)
            {
                hearts[i].sprite = isLife;
            }
            else
                hearts[i].sprite = nonLife;
        }
    }
    
    public void PauseOn()
    {
        Time.timeScale = 0;
        player.enabled = false;
        pausePanel.SetActive(true);
    }

    public void PauseOff()
    {
        Time.timeScale = 1;
        player.enabled = true;
        pausePanel.SetActive(false);
    }

    public void FinishLevel()
    {
        soundEffector.Play_winSound();
        Time.timeScale = 0;
        player.enabled = false;
        winPanel.SetActive(true);

        if (!PlayerPrefs.HasKey("Lvl") || (PlayerPrefs.GetInt("Lvl") < SceneManager.GetActiveScene().buildIndex))
            PlayerPrefs.SetInt("Lvl", SceneManager.GetActiveScene().buildIndex);

        if(PlayerPrefs.HasKey("coins"))
        {
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + player.GetCoins());
        }
        else
        {
            PlayerPrefs.SetInt("coins", player.GetCoins());
        }

        invetory.SetActive(false);
        inventoryScript.GetComponent<Inventory>().RecountItems();
    }

    public void LoseGame()
    {
        Time.timeScale = 0;
        player.enabled = false;
        losePanel.SetActive(true);

        invetory.SetActive(false);
        //inventoryScript.GetComponent<Inventory>().RecountItems();
        Debug.Log(Time.timeScale + " lose");
    }

    public void ReloadGame()
    {
        Time.timeScale = 1;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log(Time.timeScale + " reload");
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Time.timeScale = 1;
        player.enabled = true;
    }
    
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
        player.enabled = true;
    }
}

public enum TimeSettings
{
    None,
    Stopwatch,
    Timer
}
    

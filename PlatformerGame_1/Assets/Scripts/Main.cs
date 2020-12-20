using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    public Player player;
    public Text coinText;
    public Image[] hearts;
    public Sprite isLife, nonLife, halfLife;
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;

    public void ReloadGame()
    {
        Time.timeScale = 1;
        player.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Update()
    {
        coinText.text = player.GetCoins().ToString();
        CheckHP();
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
        Time.timeScale = 0;
        player.enabled = false;
        winPanel.SetActive(true);
    }

    public void LoseGame()
    {
        Time.timeScale = 0;
        player.enabled = false;
        losePanel.SetActive(true);
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

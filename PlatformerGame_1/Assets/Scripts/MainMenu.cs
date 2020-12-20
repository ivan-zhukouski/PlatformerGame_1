using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button[] levels;

    public Text coinText;

    private void Start()
    {
        if(PlayerPrefs.HasKey("Lvl"))
        {
            for(int i = 0; i < levels.Length; i++)
            {
                if (i <= PlayerPrefs.GetInt("Lvl"))
                    levels[i].interactable = true;
                else
                    levels[i].interactable = false;
            }
        }
    }

    private void Update()
    {
        if (PlayerPrefs.HasKey("coins"))
            coinText.text = PlayerPrefs.GetInt("coins").ToString();
        else
            coinText.text = "0";
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void DeleteAllKeys()
    {
        PlayerPrefs.DeleteAll();
    }
}

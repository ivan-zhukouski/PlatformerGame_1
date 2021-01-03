using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button[] levels;

    public Text coinText, starText;
    public Slider mVolume, sVolume;
    public Text mVolumText, sVolumeText;

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
        if(!PlayerPrefs.HasKey("hp"))
        {
            PlayerPrefs.SetInt("hp", 0);
        }
        if (!PlayerPrefs.HasKey("blueGem"))
        {
            PlayerPrefs.SetInt("blueGem", 0);
        }
        if (!PlayerPrefs.HasKey("greenGem"))
        {
            PlayerPrefs.SetInt("greenGem", 0);
        }

        if(!PlayerPrefs.HasKey("mVolume"))
        {
            PlayerPrefs.SetInt("mVolume", 3);
        }
        if (!PlayerPrefs.HasKey("sVolume"))
        {
            PlayerPrefs.SetInt("sVolume", 5);
        }

        mVolume.value = (int)PlayerPrefs.GetInt("mVolume");
        sVolume.value = (int)PlayerPrefs.GetInt("sVolume");
    }

    private void Update()
    {
        PlayerPrefs.SetInt("mVolume", (int)mVolume.value);
        PlayerPrefs.SetInt("sVolume", (int)sVolume.value);

        mVolumText.text = PlayerPrefs.GetInt("mVolume").ToString();
        sVolumeText.text = PlayerPrefs.GetInt("sVolume").ToString();

        if (PlayerPrefs.HasKey("coins"))
            coinText.text = PlayerPrefs.GetInt("coins").ToString();
        else
            coinText.text = "0";

        if (PlayerPrefs.HasKey("Star"))
            starText.text = PlayerPrefs.GetInt("Star").ToString();
        else
            starText.text = "0";
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void DeleteAllKeys()
    {
        PlayerPrefs.DeleteAll();
    }

    public void Buy_hp(int coins)
    {
        if(PlayerPrefs.GetInt("coins") >= coins)
        {
            PlayerPrefs.SetInt("hp", PlayerPrefs.GetInt("hp") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - coins);
        }
    }

    public void Buy_blueGem(int coins)
    {
        if (PlayerPrefs.GetInt("coins") >= coins)
        {
            PlayerPrefs.SetInt("blueGem", PlayerPrefs.GetInt("blueGem") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - coins);
        }
    }

    public void Buy_greenGem(int coins)
    {   
        if (PlayerPrefs.GetInt("coins") >= coins)
        {
            PlayerPrefs.SetInt("greenGem", PlayerPrefs.GetInt("greenGem") + 1);
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - coins);
        }
    }


}

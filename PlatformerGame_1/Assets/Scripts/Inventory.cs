using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    int hp = 0, blueGem = 0, greenGem = 0, key = 0;
    public Sprite[] numbers;
    public Sprite is_hp, no_hp, is_blueGem, no_blueGem, is_greenGem, no_greenGem, is_key, no_key;
    public Image heartImg, blueGemImg, greenGemImg, keyImg;
    public Player player;

    void Start()
    {
       if(PlayerPrefs.GetInt("hp") > 0)
        {
            hp = PlayerPrefs.GetInt("hp");
            heartImg.sprite = is_hp;
            heartImg.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hp];
        }
        if (PlayerPrefs.GetInt("blueGem") > 0)
        {
            blueGem = PlayerPrefs.GetInt("blueGem");
            blueGemImg.sprite = is_blueGem;
            blueGemImg.transform.GetChild(0).GetComponent<Image>().sprite = numbers[blueGem];
        }
        if (PlayerPrefs.GetInt("greenGem") > 0)
        {
            greenGem = PlayerPrefs.GetInt("greenGem");
            greenGemImg.sprite = is_greenGem;
            greenGemImg.transform.GetChild(0).GetComponent<Image>().sprite = numbers[greenGem];
        }
    }

    public void Add_hp()
    {
        hp++;
        heartImg.sprite = is_hp;
        heartImg.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hp];
    }

    public void Add_blueGem()
    {
        blueGem++;
        blueGemImg.sprite = is_blueGem;
        blueGemImg.transform.GetChild(0).GetComponent<Image>().sprite = numbers[blueGem];
    }

    public void Add_greenGem()
    {
        greenGem++;
        greenGemImg.sprite = is_greenGem;
        greenGemImg.transform.GetChild(0).GetComponent<Image>().sprite = numbers[greenGem];
    }

    public void Add_key()
    {
        key++;
        keyImg.sprite = is_key;
    }

    public void Use_Hp()
    {
        if(hp > 0)
        {
            hp--;
            heartImg.transform.GetChild(0).GetComponent<Image>().sprite = numbers[hp];
            player.RecountHP(1);
            if(hp == 0)
            {
                heartImg.sprite = no_hp;
            }
        }
    }
    public void Use_GreenGem()
    {
        if (greenGem > 0 && !player.isGreenGem)
        {
            greenGem--;
            greenGemImg.transform.GetChild(0).GetComponent<Image>().sprite = numbers[greenGem];
            player.ActiveGreenGem();

            if (greenGem == 0)
            {
                greenGemImg.sprite = no_greenGem;
            }
        }
    }

    public void Use_BlueGem()
    {
        if (blueGem > 0 && !player.isBlueGem)
        {
            blueGem--;
            blueGemImg.transform.GetChild(0).GetComponent<Image>().sprite = numbers[blueGem];
            player.ActiveBlueGem();

            if (blueGem == 0)
            {
                blueGemImg.sprite = no_blueGem;
            }
        }
    }
    public void RecountItems()
    {
        PlayerPrefs.SetInt("hp", hp);
        PlayerPrefs.SetInt("blueGem", blueGem);
        PlayerPrefs.SetInt("greenGem", greenGem);
    }
}

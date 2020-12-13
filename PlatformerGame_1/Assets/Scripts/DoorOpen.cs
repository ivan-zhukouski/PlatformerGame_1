using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Sprite mid, top;
    public Transform nextDoor;
    public bool isOpen = false;
    bool canTP = false;

    public void Unlock()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = mid;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = top;
        isOpen = true;
    }

    public void Teleport (GameObject player)
    {
        if (isOpen)
        {
            player.transform.position = new Vector3(nextDoor.position.x, nextDoor.position.y, player.transform.position.z);
            isOpen = false;
        }  
    }
}

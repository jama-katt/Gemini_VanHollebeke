using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CubeScript : MonoBehaviour
{
    public AudioSource thumpS, deathS, orbS, openDoorS, closeDoorS;

    public LevelManager levelManager;
    public ButtonManager buttonManager;

    public GameObject deathEffect;

    public Tilemap wallsMap;
    public Tilemap objectsMap;

    public Tile button, spikes, win, orb;

    public bool dead = false;
    public bool readyToWin = false;
    public bool orbTrigger = false;
    bool isHoldingButton = false;
    bool moving = false;
    Vector3 movedirection = new Vector3();
    void Update()
    {
        
        if (!dead)
        {
            float inputx = Input.GetAxisRaw("Horizontal");
            float inputy = Input.GetAxisRaw("Vertical");
            if (!moving)
            {
                //Debug.Log(inputx + ", " + inputy);
                if (inputx != 0)
                {
                    if (inputx > 0)
                    {
                        movedirection = Vector3.right;
                    }
                    else
                    {
                        movedirection = Vector3.left;
                    }
                    moving = true;
                }
                else if (inputy != 0)
                {
                    if (inputy > 0)
                    {
                        movedirection = Vector3.up;
                    }
                    else
                    {
                        movedirection = Vector3.down;
                    }
                    moving = true;
                }
            }
            else
            {
                if (!orbTrigger)
                {
                    Vector3 destination = FindDestination();
                    //moving
                    Vector3Int oldSpot = Vector3Int.RoundToInt(transform.position);
                    transform.position = destination;
                    Vector3Int newSpot = Vector3Int.RoundToInt(transform.position);
                    if (oldSpot != newSpot)
                    {
                        thumpS.Play();
                    }

                    if (isHoldingButton && objectsMap.GetTile(newSpot) != button)
                    {
                        isHoldingButton = false;
                        int temp = buttonManager.getButtonId(oldSpot);
                        if (temp != -1)
                        {
                            buttonManager.CloseDoor(temp);
                            closeDoorS.Play();
                        }
                        else
                            Debug.Log("error, button not found");
                    }
                    if (readyToWin && objectsMap.GetTile(newSpot) != win)
                    {
                        readyToWin = false;
                    }
                    if (objectsMap.GetTile(newSpot) == button)
                    {
                        if (!isHoldingButton)
                        {
                            isHoldingButton = true;
                            int temp = buttonManager.getButtonId(newSpot);
                            if (temp != -1)
                            {
                                buttonManager.OpenDoor(temp);
                                openDoorS.Play();
                            }
                            else
                                Debug.Log("error, button not found");
                        }
                    }
                    else if (objectsMap.GetTile(newSpot) == spikes)
                    {
                        deathS.Play();
                        dead = true;
                        Transform sprite = transform.Find("Sprite");
                        Instantiate(deathEffect, sprite.transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
                        sprite.GetComponent<SpriteRenderer>().enabled = false;
                        levelManager.Lose();
                    }
                    else if (objectsMap.GetTile(newSpot) == win)
                    {
                        if (!readyToWin)
                        {
                            readyToWin = true;
                            levelManager.Win();
                        }
                    }
                    moving = false;
                }

            }
            if (inputx == 0f && inputy == 0f)
            {
                orbTrigger = false;
                moving = false;
            }
        }
    }

    Vector3 FindDestination()
    {

        int orbMulti = 1;
        bool answerfound = false;
        Vector3 answer = new Vector3();
        Vector3Int temp = Vector3Int.RoundToInt(transform.position);
        Vector3Int intDirection = Vector3Int.RoundToInt(movedirection);

        /*Debug.Log("position: " + transform.position);
        Debug.Log("temp: " + temp);
        Debug.Log("intDirection: " + intDirection);*/

        int failswitch = 0;
        while (!answerfound)
        {
            temp += intDirection;
            answerfound = wallsMap.HasTile(temp);

            if (wallsMap.GetTile(temp) == orb)
            {
                orbMulti = 0;
                orbTrigger = true;
                orbS.Play();
            }



            failswitch++;
            if (failswitch > 100)
            {
                break;
            }
        }

        answer = temp - (intDirection * orbMulti);
        return answer;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ButtonManager : MonoBehaviour
{
    public LevelManager levelManager;

    public Tilemap wallMap;
    public Tilemap objectMap;

    public Tile door0, door1, door2, door3;

    public int numButtons = 1;
    public Vector3Int[] buttonPositions;
    public Vector3Int[] buttonDoors;
    public float[] timers;
    public bool[] isOpen;
    public bool[] disabled;
    private void Start()
    {
    }

    public void disableButton(int num)
    {
        if (num <= numButtons - 1)
            disabled[num] = true;
    }

    public void enableButton(int num)
    {
        if (num <= numButtons - 1)
            disabled[num] = false;
    }
    public void OpenDoor(int id)
    {
        if (id <= numButtons && id >= 0)
        {
            wallMap = levelManager.getWalls().GetComponent<Tilemap>();
            objectMap = levelManager.getObjects().GetComponent<Tilemap>();
            isOpen[id] = true;
        }
    }

    public void CloseDoor(int id)
    {
        if (id <= numButtons && id >= 0)
        {
            wallMap = levelManager.getWalls().GetComponent<Tilemap>();
            objectMap = levelManager.getObjects().GetComponent<Tilemap>();
            isOpen[id] = false;
        }
    }

    public int getButtonId(Vector3Int pos)
    {
        int ans = 0;
        foreach (Vector3Int x in buttonPositions)
        {
            if (x == pos)
            {
                return ans;
            }
            ans++;
        }

        return -1;
    }

    private void Update()
    {
        for (int i = 0; i < numButtons; i++)
        {
            if (!disabled[i])
            {
                if (isOpen[i])
                {
                    if (timers[i] != 0.4f)
                    {
                        timers[i] += Time.deltaTime;
                        if (timers[i] > 0.4f)
                        {
                            timers[i] = 0.4f;
                        }
                    }
                }
                else
                {
                    if (timers[i] != 0)
                    {
                        timers[i] -= Time.deltaTime;
                        if (timers[i] < 0f)
                        {
                            timers[i] = 0f;
                        }
                    }

                }
                if (timers[i] == 0f)
                {
                    wallMap.SetTile(buttonDoors[i], door0);
                    objectMap.SetTile(buttonDoors[i], null);
                }
                else if (timers[i] > 0f && timers[i] <= 0.2f)
                {
                    wallMap.SetTile(buttonDoors[i], null);
                    objectMap.SetTile(buttonDoors[i], door1);
                }
                else if (timers[i] > 0.2f && timers[i] < 0.4f)
                {
                    wallMap.SetTile(buttonDoors[i], null);
                    objectMap.SetTile(buttonDoors[i], door2);

                }
                else if (timers[i] == 0.4f)
                {
                    wallMap.SetTile(buttonDoors[i], null);
                    objectMap.SetTile(buttonDoors[i], door3);
                }
            }
            

        }
    }
}

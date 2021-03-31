using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelManager : MonoBehaviour
{
    public AudioSource winRoundS;
    public AudioSource musicS;
    public ButtonManager buttonManager;

    public AudioClip song1, song2, song3;

    bool doCD = false;
    float movementCooldown = 0f;
    float textTimer = 2f;
    public Text middleText;
    public Text levelText;
    public Grid levelParent;
    public int totalLevels = 1;
    public int currentLevel = 1;
    Vector3 cube1spot;
    Vector3 cube2spot;

    private void Start()
    {
        currentLevel = 1;
        totalLevels = levelParent.transform.childCount;
        //Debug.Log("weehoo: " + totalLevels);
        cube1spot = levelParent.transform.GetChild(currentLevel - 1).Find("Cube1").position;
        cube2spot = levelParent.transform.GetChild(currentLevel - 1).Find("Cube2").position;
        buttonManager.enableButton(currentLevel - 1);
    }
    public void Win()
    {
        Transform cube1 = levelParent.transform.GetChild(currentLevel - 1).Find("Cube1");
        Transform cube2 = levelParent.transform.GetChild(currentLevel - 1).Find("Cube2");
        if (cube1.GetComponent<CubeScript>().readyToWin == true && cube2.GetComponent<CubeScript>().readyToWin == true)
        {
            winRoundS.Play();
            if (currentLevel == totalLevels)
            {
                middleText.text = ("You Win");
                textTimer += 2f;
                levelParent.transform.GetChild(currentLevel - 1).Find("Cube1").GetComponent<CubeScript>().dead = true;
                levelParent.transform.GetChild(currentLevel - 1).Find("Cube2").GetComponent<CubeScript>().dead = true;
            }
            else
            {
                buttonManager.disableButton(currentLevel - 1);
                currentLevel++;
                buttonManager.enableButton(currentLevel - 1);
                for (int i = 0; i < totalLevels; i++)
                {
                    levelParent.transform.GetChild(i).gameObject.SetActive(false);
                }
                levelParent.transform.GetChild(currentLevel - 1).gameObject.SetActive(true);
                cube1spot = levelParent.transform.GetChild(currentLevel - 1).Find("Cube1").position;
                cube2spot = levelParent.transform.GetChild(currentLevel - 1).Find("Cube2").position;
                levelParent.transform.GetChild(currentLevel - 1).Find("Cube1").GetComponent<CubeScript>().dead = true;
                levelParent.transform.GetChild(currentLevel - 1).Find("Cube2").GetComponent<CubeScript>().dead = true;
                movementCooldown += 0.25f;
                doCD = true;
                middleText.text = ("Level " + currentLevel);
                levelText.text = ("Level " + currentLevel);
                textTimer += 2f;
                if (currentLevel == 1)
                {
                    musicS.clip = song1;
                    musicS.volume = 0.1f;
                    musicS.Play();
                }
                else if (currentLevel == 2)
                {
                    musicS.clip = song2;
                    musicS.volume = 0.07f;
                    musicS.Play();
                }
                else if (currentLevel == 3)
                {
                    musicS.clip = song3;
                    musicS.volume = 0.05f;
                    musicS.Play();
                }
            }
        } 
    }

    public void Lose()
    {
        middleText.text = ("You Lose");
        textTimer += 2f;
        levelParent.transform.GetChild(currentLevel - 1).Find("Cube1").GetComponent<CubeScript>().dead = true;
        levelParent.transform.GetChild(currentLevel - 1).Find("Cube2").GetComponent<CubeScript>().dead = true;
    }

    public void Reset()
    {
        Transform cube1 = levelParent.transform.GetChild(currentLevel - 1).Find("Cube1");
        Transform cube2 = levelParent.transform.GetChild(currentLevel - 1).Find("Cube2");
        cube1.position = cube1spot;
        cube2.position = cube2spot;
        cube1.Find("Sprite").GetComponent<SpriteRenderer>().enabled = true;
        cube2.Find("Sprite").GetComponent<SpriteRenderer>().enabled = true;
        cube1.GetComponent<CubeScript>().dead = false;
        cube2.GetComponent<CubeScript>().dead = false;
        middleText.text = ("Level " + currentLevel);
        levelText.text = ("Level " + currentLevel);
        textTimer += 2f;
    }

    public Transform getWalls()
    {
        Transform temp = levelParent.transform.GetChild(currentLevel - 1).Find("TilemapWalls");
        return temp;
    }

    public Transform getObjects()
    {
        Transform temp = levelParent.transform.GetChild(currentLevel - 1).Find("TilemapObjects");
        return temp;
    }

    private void Update()
    {
        if (textTimer > 0f)
        {
            textTimer -= Time.deltaTime;
            if (textTimer <= 0f)
            {
                textTimer = 0f;
            }
            middleText.gameObject.SetActive(true);
        }
        else
        {
            middleText.gameObject.SetActive(false);
        }

        if (movementCooldown > 0f)
        {
            movementCooldown -= Time.deltaTime;
            if (movementCooldown <= 0f)
            {
                movementCooldown = 0f;
            }
        }
        else
        {
            if (doCD)
            {
                levelParent.transform.GetChild(currentLevel - 1).Find("Cube1").GetComponent<CubeScript>().dead = false;
                levelParent.transform.GetChild(currentLevel - 1).Find("Cube2").GetComponent<CubeScript>().dead = false;
                doCD = false;
            }
            
        }
    }
}

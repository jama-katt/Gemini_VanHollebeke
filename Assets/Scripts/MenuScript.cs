using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public AudioSource blipS;

    public GameObject creditPanel;
    public GameObject instructionPanel;
    public void Play()
    {
        blipS.Play();
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        blipS.Play();
        Application.Quit();
    }

    public void Instructions()
    {
        blipS.Play();
        instructionPanel.SetActive(!instructionPanel.activeSelf);
    }

    public void Credits()
    {
        blipS.Play();
        creditPanel.SetActive(!creditPanel.activeSelf);
    }
}

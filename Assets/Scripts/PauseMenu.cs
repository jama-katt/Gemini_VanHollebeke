using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public AudioSource blipS;

    public GameObject menu;
    bool active = false;
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        blipS.Play();
        active = !active;
        menu.SetActive(active);
        if (active)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void Exit()
    {
        Toggle();
        blipS.Play();
        SceneManager.LoadScene(0);
    }
}

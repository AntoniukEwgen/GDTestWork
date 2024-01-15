using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject Panel; 

    void Start()
    {
        Time.timeScale = 0; 
        Panel.SetActive(true); 
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            ResumeGame();
        }
    }

    void ResumeGame()
    {
        Time.timeScale = 1; 
        Panel.SetActive(false);
    }
}

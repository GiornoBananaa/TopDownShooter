using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField] private Button StartButton;
    [SerializeField] private Button ContinueButton;
    [SerializeField] private Button[] LevelButtons;

    private void Start()
    {
        StartButton.onClick.AddListener(StartNewGame);
        ContinueButton.onClick.AddListener(Continue);

        int level = PlayerPrefs.GetInt("Level");
        if (level == 0)
        {
            StartButton.gameObject.SetActive(true);
            ContinueButton.gameObject.SetActive(false);
        }
        else
        {
            StartButton.gameObject.SetActive(false);
            ContinueButton.gameObject.SetActive(true);
        }

        for(int i = 0; i < level && i < 2; i++)
        {
            LevelButtons[i].GetComponent<Image>().color = Color.green;
            LevelButtons[i].enabled = true;
        }
    }

    public void StartNewGame()
    {
        PlayerPrefs.SetInt("Level",1);
        SceneManager.LoadScene(1);
    }

    public void Continue()
    {
        int level = PlayerPrefs.GetInt("Level");
        if (level > 2) level = 2;
        SceneManager.LoadScene(level);
    }
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}

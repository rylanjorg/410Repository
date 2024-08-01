using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuCanvas; // Reference to the main menu canvas
    public GameObject difficultyCanvas; // Reference to the difficulty canvas


    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(true);

        if (difficultyCanvas != null)
            difficultyCanvas.SetActive(false);
    }
    // Function to set difficulty canvas to active and main menu canvas to inactive
    public void DifficultySelect()
    {
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(false);

        if (difficultyCanvas != null)
            difficultyCanvas.SetActive(true);
    }

    public void BackToMainMenu()
    {
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(true);

        if (difficultyCanvas != null)
            difficultyCanvas.SetActive(false);
    }

    public void DifficultyEasy()
    {
        PlayerPrefs.SetInt("DifficultyLevel", 0);
        Cursor.visible = false;
        SceneManager.LoadScene("Level_0");
    }

    public void DifficultyMedium()
    {
        PlayerPrefs.SetInt("DifficultyLevel", 1);
        Cursor.visible = false;
        SceneManager.LoadScene("Level_0");
    }

    public void DifficultyHard()
    {
        PlayerPrefs.SetInt("DifficultyLevel", 2);
        Cursor.visible = false;
        SceneManager.LoadScene("Level_0");
    }

    public void Quit()
    {
        Application.Quit();
    }
}

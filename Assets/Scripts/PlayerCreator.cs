using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerCreator : MonoBehaviour
{
    public static PlayerCreator instance;
    public string namePlayer;
    public Color colorPlayer;

    public InputField input;
    void Start()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;

        input = FindObjectOfType<InputField>();
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        input.keyboardType = TouchScreenKeyboardType.Default;
    }

    public void PrintName()
    {
        namePlayer = input.text;
    }

    public void GetColor(int index)
    {
        if (index == 0)
            colorPlayer = Color.green;
        if (index == 1)
            colorPlayer = Color.yellow;
        if (index == 2)
            colorPlayer = Color.magenta;
        if (index == 3)
            colorPlayer = Color.blue;
    }
    public void StartGame()
    {
        if (namePlayer.Length > 0 && colorPlayer != Color.clear)
            SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}

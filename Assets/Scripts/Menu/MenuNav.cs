using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuNav : MonoBehaviour
{
    //public AIStrategy strategy = AIStrategy.FavorLoss;

    public void OnClick_TitleScreen()
    {
        MenuManager.OpenMenu(Menu.TitleScreen, gameObject);
    }

    public void OnClick_MainMenu()
    {
        MenuManager.OpenMenu(Menu.MainMenu, gameObject);
    }

    public void OnClick_Info()
    {
        MenuManager.OpenMenu(Menu.Info, gameObject);
    }

    public void OnClick_Credits()
    {
        MenuManager.OpenMenu(Menu.Credits, gameObject);
    }

    public void OnClick_Play()
    {
        SceneManager.LoadScene("Main");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MenuManager
{
    public static bool IsInitialised { get; private set;}
    public static GameObject TitleScreen, MainMenu, Credits, Info, Levels1to4, Levels5to8, Levels9to12;

    public static void Init()
    {
        GameObject menuManager = GameObject.Find("MenuManager");
        TitleScreen = menuManager.transform.Find("TitleScreen").gameObject;
        MainMenu = menuManager.transform.Find("MainMenu").gameObject;
        Credits = menuManager.transform.Find("Credits").gameObject;
        Info = menuManager.transform.Find("Info").gameObject;
        IsInitialised = true;
    }

    public static void OpenMenu(Menu menu, GameObject callingMenu)
    {
        if(!IsInitialised)
            Init();
        switch (menu)
        {
            case Menu.TitleScreen:
                TitleScreen.SetActive(true);
                break;
            case Menu.MainMenu:
                MainMenu.SetActive(true);
                break;
            case Menu.Credits:
                Credits.SetActive(true);
                break;
            case Menu.Info:
                Info.SetActive(true);
                break;
        }

        callingMenu.SetActive(false);
    }
}

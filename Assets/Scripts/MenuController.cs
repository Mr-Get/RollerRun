using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    GameObject mainMenuPanel;
    GameObject highscorePanel;
    GameObject settingsPanel;

    // Use this for initialization
    void Start()
    {
        //Find the MenuObjects
        mainMenuPanel = GameObject.Find("/MenuCanvas/MainMenuPanel");
        highscorePanel = GameObject.Find("/MenuCanvas/HighscorePanel");
        settingsPanel = GameObject.Find("/MenuCanvas/SettingsPanel");

        //Alle bis auf MainMenu deaktivieren
        highscorePanel.SetActive(false);
        settingsPanel.SetActive(false);

        //Highscore Liste initialisieren
        List<HighscoreClass> highscoreList = getHighScoreList();
        for (int i = 0; i < 10; i++)
        {
            highscoreList.Add(new HighscoreClass { name = "---", score = 0 });
        }
        highscoreList = highscoreList.OrderBy(d => d.score).Take(10).ToList();
        setHighScoreList(highscoreList);
    }

    public void setHighScoreList(List<HighscoreClass> highscoreList)
    {
        List<string> outputArray = new List<string>();
        foreach (HighscoreClass h in highscoreList)
        {
            outputArray.Add(h.name + '`' + h.score);
        }
        PlayerPrefsX.SetStringArray("Highscore", outputArray.ToArray());
    }

    public List<HighscoreClass> getHighScoreList()
    {
        List<HighscoreClass> highscoreList = new List<HighscoreClass>();
        foreach (String d in new List<string>(PlayerPrefsX.GetStringArray("Highscore").ToList()))
        {
            highscoreList.Add(new HighscoreClass { name = d.Split('`')[0], score = Int32.Parse(d.Split('`')[1]) });
        }
        return highscoreList;
    }

    public HighscoreClass getHighScoreElement(int elementID)
    {
        return getHighScoreList()[elementID];
    }

    public void onClickStartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void onClickQuit()
    {
        Application.Quit();
    }

    public void onClickHighscore()
    {
        highscorePanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    public void onClickHighscoreBack()
    {
        highscorePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}

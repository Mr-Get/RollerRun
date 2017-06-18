using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VariableScript : MonoBehaviour
{

    private List<GameObject> lastRows;
    private List<GameObject> saveRowsPrefabs;
    private List<GameObject> unsaveRowsPrefabs;
    private List<GameObject> noItemRowPrefabs;
    private List<GameObject> itemRowPrefabs;
    private int nowSaveRows;
    public int minSaveRows;
    public int maxSaveRows;
    private int nowNoItemRows;
    public int minNoItemRows;
    public int maxNoItemRows;

    //Variables for Scroe
    private Text scoreText;
    private int score;

    public Text gameOverField;


    void Start()
    {
        List<GameObject> newRowsPrefabs = new List<GameObject>();
        GameObject[] createRowsPrefab = Resources.LoadAll<GameObject>("RowsPrefabs");
        //Convert GameObject[] Array to List<GameObject>
        foreach (GameObject PrefabItem in createRowsPrefab) { newRowsPrefabs.Add(PrefabItem); }

        List<GameObject> newItemRowsPrefabs = new List<GameObject>();
        GameObject[] createItemPrefabs = Resources.LoadAll<GameObject>("ItemPrefabs");
        //Convert GameObject[] Array to List<GameObject>
        foreach (GameObject PrefabItem in createItemPrefabs) { newItemRowsPrefabs.Add(PrefabItem); }

        //Liste filtern nach allen Objekten, die keine Blöcke an denen man sterben kann haben.
        saveRowsPrefabs = newRowsPrefabs.FindAll(delegate (GameObject b)
        {
            return !(b.GetComponent<PreFabPositions>().leftPos ||
            b.GetComponent<PreFabPositions>().midPos ||
            b.GetComponent<PreFabPositions>().rightPos);
        });

        //Liste filtern nach allen Objekten, die Blöcke an denen man sterben kann haben.
        unsaveRowsPrefabs = newRowsPrefabs.FindAll(delegate (GameObject b)
        {
            return b.GetComponent<PreFabPositions>().leftPos ||
            b.GetComponent<PreFabPositions>().midPos ||
            b.GetComponent<PreFabPositions>().rightPos;
        });

        //Liste filtern nach allen Objekten, die keine Items haben.
        noItemRowPrefabs = newItemRowsPrefabs.FindAll(delegate (GameObject b)
        {
            return !(b.GetComponent<ItemPreFabPositions>().leftPos ||
            b.GetComponent<ItemPreFabPositions>().midPos ||
            b.GetComponent<ItemPreFabPositions>().rightPos);
        });

        //Liste filtern nach allen Objekten, die Items haben.
        itemRowPrefabs = newItemRowsPrefabs.FindAll(delegate (GameObject b)
        {
            return b.GetComponent<ItemPreFabPositions>().leftPos ||
            b.GetComponent<ItemPreFabPositions>().midPos ||
            b.GetComponent<ItemPreFabPositions>().rightPos;
        });

        lastRows = new List<GameObject>();
        for (int i = 2; i <= 20; i = i + 2)
        {
            Instantiate(createRowsPrefab[0], new Vector3(0, 0, i), this.gameObject.transform.rotation);
        }

        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        score = 0;
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void saveScore(string newName)
    {
        List<HighscoreClass> highscoreList = getHighScoreList();
        highscoreList.Add(new HighscoreClass { name = newName.Replace('`', '´'), score = getScore() });
        highscoreList = highscoreList.OrderBy(d => d.score).Take(10).ToList();
        setHighScoreList(highscoreList);
    }

    public void gameOver()
    {
        GameObject.Find("GameOverCanvas").SetActive(true);
    }

    public void gameOverLoadMenu()
    {
        saveScore(gameOverField.text.ToString());
        SceneManager.LoadScene("Online");
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

    public void addRow(GameObject row)
    {
        this.lastRows.Add(row);
    }

    public void removeRow(GameObject row)
    {
        lastRows.Remove(row);
    }

    public List<GameObject> getLastRows()
    {
        return lastRows;
    }

    public List<GameObject> getUnsaveRowsPrefabs()
    {
        return unsaveRowsPrefabs;
    }
    public List<GameObject> getSaveRowsPrefabs()
    {
        return saveRowsPrefabs;
    }

    public List<GameObject> getNoItemRowPrefabs()
    {
        return noItemRowPrefabs;
    }

    public List<GameObject> getItemRowPrefabs()
    {
        return itemRowPrefabs;
    }

    public int getNowSaveRows()
    {
        return nowSaveRows;
    }

    public void resetNowSaveRows()
    {
        this.nowSaveRows = UnityEngine.Random.Range(minSaveRows, maxSaveRows);
    }

    public void reduceNowSaveRows()
    {
        this.nowSaveRows = this.nowSaveRows - 1;
    }

    public int getMinNoItemRows()
    {
        return minNoItemRows;
    }

    public int getMaxNoItemRows()
    {
        return maxNoItemRows;
    }

    public int getNowNoItemRows()
    {
        return this.nowNoItemRows;
    }

    public void resetNowNoItemRows()
    {
        this.nowNoItemRows = UnityEngine.Random.Range(minNoItemRows, maxNoItemRows);
    }

    public void reduceNowNoItemRows()
    {
        this.nowNoItemRows = this.nowNoItemRows - 1;
    }

    public int getMinSaveRows()
    {
        return minSaveRows;
    }

    public int getMaxSaveRows()
    {
        return maxSaveRows;
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + score;
    }

    public void addScore(int score)
    {
        this.score += score;
        UpdateScore();
    }

    public int getScore()
    {
        return score;
    }
}

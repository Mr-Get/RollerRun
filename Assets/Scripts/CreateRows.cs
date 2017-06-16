using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRows : MonoBehaviour
{

    private GameObject scriptContainer;
    private List<GameObject> unsaveRowsPrefabs;
    private List<GameObject> saveRowsPrefabs;
    private List<GameObject> lastRows;
    private List<GameObject> noItemRowPrefabs;
    private List<GameObject> itemRowPrefabs;
    private List<GameObject> generateableItemRows;
    private List<GameObject> generateableNoItemRows;
    private int nowSaveRows;
    private int nowNoItemRows;
    private int generatedSave;
    private int generatedUnsave;
    private int itemGeneriereAb;
    private int itemWahrscheinlichkeit;

    // Use this for initialization
    void Start()
    {
        scriptContainer = GameObject.Find("ScriptContainer");
        unsaveRowsPrefabs = scriptContainer.GetComponent<VariableScript>().getUnsaveRowsPrefabs();
        saveRowsPrefabs = scriptContainer.GetComponent<VariableScript>().getSaveRowsPrefabs();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name.IndexOf("GameBall") > -1)
        {
            generatedSave = -1;
            generatedUnsave = -1;
            Vector3 newPos = new Vector3(0, 0, this.gameObject.transform.position.z + 20);
            CreateRow(newPos);
            CreateItem(newPos);
        }
    }

    private void CreateRow(Vector3 newPos)
    {
        lastRows = scriptContainer.GetComponent<VariableScript>().getLastRows();
        nowSaveRows = scriptContainer.GetComponent<VariableScript>().getNowSaveRows();

        if (nowSaveRows == 0)
        {
            //Generate unsaveRow
            int unsaveRowNumb = Random.Range(0, unsaveRowsPrefabs.Count);
            Instantiate(unsaveRowsPrefabs[unsaveRowNumb], newPos, this.gameObject.transform.rotation);
            scriptContainer.GetComponent<VariableScript>().resetNowSaveRows();
            generatedUnsave = unsaveRowNumb;
        }
        else
        {
            //Generate saveRow
            int saveRowNumb = Random.Range(0, saveRowsPrefabs.Count);
            Instantiate(saveRowsPrefabs[saveRowNumb], newPos, this.gameObject.transform.rotation);
            scriptContainer.GetComponent<VariableScript>().reduceNowSaveRows();
            generatedSave = saveRowNumb;
        }
    }

    private void CreateItem(Vector3 newPos)
    {
        noItemRowPrefabs = scriptContainer.GetComponent<VariableScript>().getNoItemRowPrefabs();
        itemRowPrefabs = scriptContainer.GetComponent<VariableScript>().getItemRowPrefabs();
        nowNoItemRows = scriptContainer.GetComponent<VariableScript>().getNowNoItemRows();

        //Liste nach ItemRows, die an der Spielweite (Position des Game-Balls) generiert werden können, filtern.
        generateableItemRows = itemRowPrefabs.FindAll(delegate (GameObject b)
        {
            return b.GetComponent<ItemPreFabPositions>().getGenerierungAbWeite() < this.gameObject.transform.position.z;
        });

        //Zu jedem generierbaren Item-Prefab die Wahrscheinlichkeit in ein Array von INT zusammenfügen.
        int[] generateableItemRowsProbability = generateableItemRows.ConvertAll(delegate (GameObject b)
            {
                return b.GetComponent<ItemPreFabPositions>().wahrscheinlichkeit;
        }).ToArray();

        

        Debug.Log(string.Join(";", new List<int>(generateableItemRowsProbability).ConvertAll(i => i.ToString()).ToArray()));

        //Loaded Die (Zufallsgenerator mit Wahrscheinlichkeit zu jedem Item) mit der Wahrscheinlichkeitsliste instanzieren.
        LoadedDie randomGenerateableItemRowsDie = new LoadedDie(generateableItemRowsProbability);

        //SaveRow generiert
        if (generatedSave != -1)
        {
            if (nowNoItemRows == 0)
            {
                //Generate Row with Item
                int itemRow = randomGenerateableItemRowsDie.NextValue();
                Instantiate(generateableItemRows[itemRow], newPos, this.gameObject.transform.rotation);
                scriptContainer.GetComponent<VariableScript>().resetNowNoItemRows();
            }
            else
            {
                //Generate Row without Item
                int noItemRow = Random.Range(0, noItemRowPrefabs.Count);
                Instantiate(noItemRowPrefabs[noItemRow], newPos, this.gameObject.transform.rotation);
                scriptContainer.GetComponent<VariableScript>().reduceNowNoItemRows();
            }
        }
        //UnsaveRow generiert
        else if (generatedUnsave != -1)
        {
            if (nowNoItemRows == 0)
            {
                //Generate Row with Item
                int itemRow = randomGenerateableItemRowsDie.NextValue();
                while (true)
                {
                    if (checkItemPosition(itemRow, generatedUnsave, generateableItemRows))
                    {
                        break;
                    }
                    itemRow = randomGenerateableItemRowsDie.NextValue();
                }
                Instantiate(generateableItemRows[itemRow], newPos, this.gameObject.transform.rotation);
                scriptContainer.GetComponent<VariableScript>().resetNowNoItemRows();
            }
            else
            {
                //Generate Row without Item
                int noItemRow = Random.Range(0, noItemRowPrefabs.Count);
                Instantiate(noItemRowPrefabs[noItemRow], newPos, this.gameObject.transform.rotation);
                scriptContainer.GetComponent<VariableScript>().reduceNowNoItemRows();
            }
        }
        else
        {
            Debug.Log("Kein Item generierbar");
            Debug.Log("Save: " + generatedSave + "; Unsave: " + generatedUnsave);
        }
    }
    private bool checkItemPosition(int newItemPrefabNumb, int generatedRowPrefabNumb, List<GameObject> generateableItemRows)
    {
        bool leftItemPos = generateableItemRows[newItemPrefabNumb].GetComponent<ItemPreFabPositions>().leftPos;
        bool midItemPos = generateableItemRows[newItemPrefabNumb].GetComponent<ItemPreFabPositions>().midPos;
        bool rightItemPos = generateableItemRows[newItemPrefabNumb].GetComponent<ItemPreFabPositions>().rightPos;

        if (leftItemPos)
        {
            if (unsaveRowsPrefabs[generatedRowPrefabNumb].GetComponent<PreFabPositions>().leftPos)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else if (midItemPos)
        {
            if (unsaveRowsPrefabs[generatedRowPrefabNumb].GetComponent<PreFabPositions>().midPos)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (unsaveRowsPrefabs[generatedRowPrefabNumb].GetComponent<PreFabPositions>().rightPos)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

    }
}

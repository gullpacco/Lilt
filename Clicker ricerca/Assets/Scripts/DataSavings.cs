using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class DataSavings : MonoBehaviour {


    Research[] res = new Research[7];
    ResearchData[] resDat = new ResearchData[7];

    public static DataSavings instance;
    float score;
    string path = "/playerInfo.dat";
    bool reset;
    void Awake()
    {
        instance = this;
        res = FindObjectsOfType<Research>();
    }

    // Use this for initialization
    void Start () {
        Load();

    }

    // Update is called once per frame
    void Update () {
        score = GameController.instance.GetCure();
	}

    public void SetCureStats(int position, Research res)
    {

    } 

    public void Save()
    {
        if (!reset)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + path, FileMode.OpenOrCreate);
            PlayerData data = new PlayerData();

            //TODO
            for (int i = 0; i < res.Length; i++)
            {
                resDat[i] = res[i].Serialize();
            }
            data.res = resDat;
            data.score = score;
            bf.Serialize(file, data);
            file.Close();
        }
    }


    public void Erase()
    {
        BinaryFormatter bf = new BinaryFormatter();
        //FileStream file = File.Open(Application.persistentDataPath + path, FileMode.Open);
        File.Delete(Application.persistentDataPath + path);
        reset = true;
    }
    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + path,FileMode.Open);
            PlayerData data = (PlayerData) bf.Deserialize(file);
            file.Close();
            resDat = data.res;
            GameController.instance.SetCure(data.score);
            

            Research [] tempResearches = FindObjectsOfType<Research>();
            Debug.Log(tempResearches.Length + " length and " + resDat.Length );
           for (int p=0; p<tempResearches.Length; p++)
            {
                Debug.Log("counter: " + p);

                foreach (ResearchData r in resDat)
                {
                    Debug.Log("Name 1 " + tempResearches[p].name + " Name 2 " + r.name);
                    if (tempResearches[p].name == r.name)
                    {
                        Debug.Log("I nomi sono uguali");
                        tempResearches[p].Deserialize(r);

                        Debug.Log("Deserialized");
                    }
                }
            }
        }
    }

}

[Serializable]
class PlayerData
{
    public ResearchData[] res = new ResearchData[7];
    public float score;
}
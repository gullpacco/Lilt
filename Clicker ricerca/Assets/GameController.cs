using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    float totalCure;
    public Text totalCureText;
    public GameObject   menu,
                        research;
    bool open;


    void Awake()
    {
        if (PlayerPrefs.HasKey("TotalCure"))
            totalCure = PlayerPrefs.GetFloat("TotalCure");
        Screen.orientation = ScreenOrientation.Portrait;


    }
    // Use this for initialization
    void Start () {
      
    }

    // Update is called once per frame
    void Update () {
	
	}

    public void IncreaseCure(float cure)
    {
        totalCure += cure;
        totalCureText.text = "Cure Points: " + totalCure.ToString("F2") + "§";
    }


    public bool DecreaseCure(float cure)
    {
        if (cure <= totalCure)
        {
            totalCure -= cure;
            totalCureText.text = "Cure Points: " + totalCure.ToString("F2") + "§";

            return true;

        }

        else
            return false;

    }

    public float GetCure()
    {
        return totalCure;
    }


    void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("TotalCure", totalCure);

    }

    public void Reset()
    {
        totalCure = 0;
        PlayerPrefs.SetFloat("TotalCure", totalCure);

    }


    public void OpenMenu()
    {
        if (open)
            Close();
        else
            Open();
    }

    void Open()
    {
        research.transform.position = new Vector3(research.transform.position.x - 11.65f, research.transform.position.y, research.transform.position.z);
        menu.transform.position = new Vector3(menu.transform.position.x +9.65f, menu.transform.position.y, menu.transform.position.z);
        open = true;
        
    }

    void Close()
    {
        research.transform.position = new Vector3(research.transform.position.x + 11.65f, research.transform.position.y, research.transform.position.z);

        menu.transform.position = new Vector3(menu.transform.position.x - 9.65f, menu.transform.position.y, menu.transform.position.z);
        open = false;

    }

    public void Quit() {

        Application.Quit();
    }

}


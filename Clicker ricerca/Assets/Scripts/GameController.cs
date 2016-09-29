using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    float totalCure;
    public Text totalCureText;
    public GameObject   menu,
                        research,
                        managers,
                        upgrades;
    bool open;
    int state = 0; /* 0 search, 1 menu, 2 managers, 3 upgrades*/
    public static GameController instance;

    void Awake()
    {
        if (PlayerPrefs.HasKey("TotalCure"))
            totalCure = PlayerPrefs.GetFloat("TotalCure");
        Screen.orientation = ScreenOrientation.Portrait;
        instance = this;


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
        
        switch (state)
        {
            case 0:
                Open();
                break;

            case 1:
                Close();
                break;

            case 2:
                ManagersToMenu();
                break;

            case 3: UpgradesToMenu();
                break;

            default: break;


            
        } 
    }

    void Open()
    {
        research.transform.position = new Vector3(research.transform.position.x - 11.65f, research.transform.position.y, research.transform.position.z);
        menu.transform.position = new Vector3(menu.transform.position.x +9.65f, menu.transform.position.y, menu.transform.position.z);
        state = 1;
        
    }

    void Close()
    {
        research.transform.position = new Vector3(research.transform.position.x + 11.65f, research.transform.position.y, research.transform.position.z);

        menu.transform.position = new Vector3(menu.transform.position.x - 9.65f, menu.transform.position.y, menu.transform.position.z);
        state = 0;

    }

    void ManagersToMenu()
    {
        managers.transform.position = new Vector3(managers.transform.position.x - 11.65f, managers.transform.position.y, managers.transform.position.z);
        menu.transform.position = new Vector3(menu.transform.position.x + 9.65f, menu.transform.position.y, menu.transform.position.z);
        state = 1;
    }

    public void OpenManagers()
    {
        managers.transform.position = new Vector3(managers.transform.position.x + 11.65f, managers.transform.position.y, managers.transform.position.z);
        menu.transform.position = new Vector3(menu.transform.position.x - 9.65f, menu.transform.position.y, menu.transform.position.z);
        state = 2;
    }

    public void OpenUpgrades()
    {
        upgrades.transform.position = new Vector3(upgrades.transform.position.x + 11.65f, upgrades.transform.position.y, upgrades.transform.position.z);
        menu.transform.position = new Vector3(menu.transform.position.x - 9.65f, menu.transform.position.y, menu.transform.position.z);
        state = 3;

    }

    void UpgradesToMenu()
    {
        upgrades.transform.position = new Vector3(upgrades.transform.position.x - 11.65f, upgrades.transform.position.y, upgrades.transform.position.z);
        menu.transform.position = new Vector3(menu.transform.position.x + 9.65f, menu.transform.position.y, menu.transform.position.z);
        state = 1;
    }

    public void Quit() {

        Application.Quit();
    }


  
}


using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeButton : MonoBehaviour {

    Image img;
    Text name_text, cost_text;

    void Awake()
    {
        img = GetComponent<Image>();

    }

    // Use this for initialization
    void Start () {
        name_text = transform.GetChild(0).GetComponent<Text>();
        cost_text = transform.GetChild(1).GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetName(string text)
    {
        name_text.text = text;
    }

    public void SetCost(int cost)
    {
        cost_text.text = "Cost: " + cost + "§";
    }

    public void ChangeColour(Color col)
    {
        img.color = col;

    }
}

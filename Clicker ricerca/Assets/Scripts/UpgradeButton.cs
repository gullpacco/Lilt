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
        img = GetComponent<Image>();
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
        StartCoroutine(Delay(cost));
    }

    IEnumerator Delay(int cost)
    {
        yield return new WaitForEndOfFrame();
        cost_text.text = "Costo: "+ GameController.ConvertScore(cost) + " Din";

    }

    public void ChangeColour(Color col)
    {
        img.color = col;

    }
}

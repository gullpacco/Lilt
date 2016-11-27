using UnityEngine;
using System.Collections;

public class ManagerMenu : MonoBehaviour {

    ManagerMenu instance;
    GameObject[] managers;

    void Awake()
    {
        instance = this;
        managers = new GameObject[transform.childCount];
        for (int p = 0; p < managers.Length; p++)
        {
            managers[p] = transform.GetChild(p).gameObject;
        }
    }


	// Use this for initialization
	void Start () {

        managers = new GameObject[transform.childCount];
        for(int p=0; p<managers.Length; p++)
        {
            managers[p] = transform.GetChild(p).gameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void RemoveManager(string name)
    {
        Debug.Log(name);
        int toRemove = -1;
        if (managers.Length > 1) {

            for(int k=0; k<managers.Length; k++)
            {

                if (managers[k].name.Contains(name))
                {
                    toRemove = k;
                    break;
                }
            }

            GameObject[] tmpArray = new GameObject[managers.Length - 1];
            for (int p =managers.Length; p >toRemove+1 ; p--){
                
                Debug.Log(p);
                managers[p-1].transform.position = managers[p-2].transform.position;
                tmpArray[p-2] = managers[p-1];
            }

            for(int j=toRemove-1; j>=0; j--)
            {
                tmpArray[j] = managers[j];
            }
            //Destroy(managers[toRemove]);
            managers[toRemove].SetActive(false);
            managers = tmpArray;
        }
        else
        {
            //Destroy(managers[0]);
            managers[toRemove].SetActive(false);

            managers = null;
        }
    }

    void DebugArray(GameObject[] arr)
    {
        for (int j = 0; j < managers.Length; j++)
        {
            Debug.Log(managers[j]);
        }
        for (int j = 0; j < arr.Length; j++)
        {
            Debug.Log(arr[j]);
        }
    }



}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Research : MonoBehaviour {

    public float    baseCure,
                    baseUpgradeCost,
                    baseCureTime,
                    baseUpgradeModifier,
                    baseUpgradeTimeModifier,
                    upgradeCostModifier,
                    unlockCost,
                    managerCost;
    public bool unlocked;
   float  currentCure,   
          cureTime,
          upgradeCost,
          upgradeModifier,
          upgradeTimeModifier;
    public string cureName;
    public Text gainText, timeText, levelText, upgradeText;
    public GameObject lockedText;
    public UpgradeButton upgrade_button;
    int bonusLimit = 25, level, purchased_upgrades;
    public int purchased_upgrade_cost;
    bool managed, loading;
    Image img;
    GameController gc;

    float timeLeft;

    void Awake()
    {

        gc = FindObjectOfType<GameController>();
        img = GetComponent<Image>();


        currentCure = baseCure;
        cureTime = baseCureTime;
        upgradeCost = baseUpgradeCost;
        upgradeTimeModifier = baseUpgradeTimeModifier;
        upgradeModifier = baseUpgradeModifier;
        if (PlayerPrefs.HasKey("Level" + cureName))
        { unlocked = true;
            level = PlayerPrefs.GetInt("Level" + cureName);
            InstantUpgrade(level);
        }
        
        if (PlayerPrefs.HasKey("Managed" + cureName))
            managed = true;

     

        

    }
    // Use this for initialization
    void Start () {
        name= name.Split('_')[0];
        if (unlocked)
        {
            levelText.text = "" + level;
            level = 1;
            lockedText.SetActive(false);
            upgradeText.text = upgradeCost.ToString("F2") + "§";
        }

        if (managed)
        {
            float time = 0;
            int multiples = 0;
            float difference = 0;
            float previousTime = 0;

            System.DateTime now = System.DateTime.Now;
            time += now.Second + now.Minute * 60 + now.Hour * 60 * 60 + now.DayOfYear * 60 * 60 * 24 + now.Year * 60 * 60 * 24 * 365;
            if (PlayerPrefs.HasKey("LastAccess"))
            {
                previousTime = PlayerPrefs.GetFloat("LastAccess");
                multiples = (int)((time - previousTime) / cureTime);
                InstantIncrease(multiples);

                difference = (time - previousTime) - (multiples * cureTime);
                StartCoroutine(Increase(difference));
                timeLeft = difference;
            }
            else timeLeft = 0;

        }
        else timeLeft = 0;

        if (PlayerPrefs.HasKey("Purchased_Upgrades"))
        {
            for(int i=0; i<PlayerPrefs.GetInt("Purchased_Upgrades"); i++)
            {
                InstantPurchaseUpgrade();
            }
        }
      
        gainText.text = "Gain: " + currentCure.ToString("F2");
       

        if (!unlocked)
        {
            Debug.Log("hey");
            img.color = new Color(0.1f, 0.1f, 0.1f);
        }
        else img.color = new Color(.1f, .8f, .1f);

        InvokeRepeating("RecalculateTime", 0, 1);
        if (timeLeft == 0)
            timeLeft = cureTime;
        upgrade_button.SetName(name + " Research x 3");
        upgrade_button.SetCost(purchased_upgrade_cost);


    }

    // Update is called once per frame
    void Update () {

        if (managed)
        {
            Cure();
        }

        if (!unlocked)
        {
            if (gc.GetCure() > unlockCost)
            {
                img.color = new Color(0.1f, 0.1f, 0.5f);

            }
            else img.color = new Color(0.1f, 0.1f, 0.1f);

        }
        // RecalculateTime();
        CheckUpgrades();
    }


    void CheckUpgrades()
    {
        if (gc.GetCure() > purchased_upgrade_cost)
        {
            upgrade_button.ChangeColour(new Color(0.3f, 0.3f, 0.6f));
        }
        else upgrade_button.ChangeColour(new Color(0.3f, 0.3f, 0.3f));
    }

    void RecalculateTime()
    {
      
        string timeLeftText = "Time: " ;
        //float whatIsLeft;
        int difference;
        bool showSeconds =true;

        if (loading)
        {
            timeLeft--;
        }
        Debug.Log(timeLeft);
        float tmpTime = timeLeft;


        if (timeLeft >= 60 * 60 * 24 * 365)
        { timeLeftText += "??????"; }
        else {
            if (timeLeft > 60 * 60 * 24)
            {
                difference = (int)tmpTime /( 60 * 60 * 24);
                timeLeftText += difference + "d ";
                tmpTime = tmpTime - difference *60*60*24;
              //  showSeconds = false;
            }

            if (tmpTime >= 60 * 60/* || (tmpTime <= 60 * 60 && tmpTime >=60)*/)
            {
                //if (tmpTime <= 60 * 60 && tmpTime >= 60)
                //{
                //    timeLeftText += "0h ";

                //}

                difference = (int)tmpTime /(60 * 60);
                timeLeftText += difference + "h ";
                tmpTime = tmpTime - difference *60* 60;

            }
            if (tmpTime >= 60  )
            {
                difference = (int)tmpTime / 60 ;
                timeLeftText += difference + "m ";
                tmpTime = tmpTime - difference * 60;
            }

            if (tmpTime > 0 && showSeconds)
            {
               
                timeLeftText +=(int) tmpTime + "s ";
                
            }

         

        }
        if (timeLeft <= 0)
        {
            timeLeftText += "0s";
           // timeLeft = cureTime;
            

        }
        timeText.text = timeLeftText;
    }

   public void Cure()
    {
        if (unlocked)
        {
            if (!loading)
            {
                StartCoroutine(Increase(cureTime));
                loading = true;
                img.color = new Color(0.8f, 0.1f, 0.1f);

            }
        }

        else
        {
            if (gc.DecreaseCure(unlockCost))
            { unlocked = true;
                lockedText.SetActive(false);
                img.color = new Color(.1f, .8f, .1f);
                level = 1;
               // PlayerPrefs.SetInt("Level" + cureName, 1);
                levelText.text = "" + level;

            }



        }

    }

    IEnumerator Increase(float timer)
    {
        yield return new WaitForSeconds(timer);
        gc.IncreaseCure(currentCure);
        loading = false;
        img.color = new Color(.1f, .8f, .1f);
        timeLeft = cureTime;
        RecalculateTime();

        StopAllCoroutines();
        if (managed)
        {
            float time = 0;
            System.DateTime now = System.DateTime.Now;
            time += now.Second + now.Minute * 60 + now.Hour * 60 * 60 + now.DayOfYear * 60 * 60 * 24 + now.Year * 60 * 60 * 24 * 365;
            PlayerPrefs.SetFloat("LastCoroutineEnded", time);

        }

    }

    //Instante Resource increase on game start

    void InstantIncrease(int multiples)
    {
        gc.IncreaseCure(currentCure*multiples);

    }

    //Upgrades with cost
    public void Upgrade()
    {
        if (unlocked)
        {
            if (gc.DecreaseCure(upgradeCost))
            {
                CalculateUpgrade();
            }
        }

    }


    //Instant Upgrade on game start
    void InstantUpgrade(int count)
    {
       // level = 2;
        Debug.Log(count);
        for (int i = 2; i < count; i++)
        {


            CalculateUpgrade();
        }
    }

    void CalculateUpgrade()
    {
        currentCure *= upgradeModifier;
        upgradeCost *= upgradeCostModifier;
        // cureTime /= upgradeTimeModifier;
        gainText.text = "Gain: " + currentCure.ToString("F2");
        level++;
        levelText.text = "" + level;
        upgradeText.text = upgradeCost.ToString("F2") + "§";


        if (level >= bonusLimit)
        {
            cureTime /= 2;
            bonusLimit *= 2;
        }
    }

    //Activates automatic purchase of resource
    public void BuyAuto()
    {
        if (unlocked && !managed)
        {
            if (gc.DecreaseCure(managerCost))
            {
                managed = true;
                PlayerPrefs.SetInt("Managed" + cureName, 1);

            }
        }
}


    //Closing game
    void OnApplicationQuit()
    {
        float time=0;
        System.DateTime now = System.DateTime.Now;
        time += now.Second + now.Minute * 60 + now.Hour * 60 * 60 + now.DayOfYear * 60 * 60 * 24 + now.Year * 60 * 60 * 24 * 365;
        PlayerPrefs.SetFloat("LastAccess", time);
        PlayerPrefs.SetInt("Level" + cureName, level); ;

    }

    //Purchasing Upgrade from upgrade menu
    public void PurchaseUpgrade()
    {
        if (gc.DecreaseCure(purchased_upgrade_cost))
        {
            purchased_upgrades++;
            currentCure *= 3;
            purchased_upgrade_cost*= purchased_upgrade_cost;
            upgrade_button.SetCost(purchased_upgrade_cost);
            PlayerPrefs.SetInt("Purchased_Upgrades", purchased_upgrades);
        }
    }

    void InstantPurchaseUpgrade()
    {
        purchased_upgrades++;
        currentCure *= 3;
        purchased_upgrade_cost *= purchased_upgrade_cost;
        upgrade_button.SetCost(purchased_upgrade_cost);
    }

    //Resetting PlayerPrefs
    public void RefreshPrefs()
    {
        PlayerPrefs.DeleteAll();
        gc.Reset();
    }

}

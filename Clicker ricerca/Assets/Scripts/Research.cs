using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class Research : MonoBehaviour {

    public int baseCure,
                    baseUpgradeCost,
                    baseCureTime,
                    managerCost,
    purchased_upgrade_cost;
    bool recovered;
    public float
                    upgradeCostModifier;
                   

    public bool unlocked;
   float  currentCure,   
          cureTime,
          upgradeCost,
          cureModifiers=1;
    public Text gainText, timeText, levelText, upgradeText;
    public GameObject lockedText, managerAlert;
    public UpgradeButton upgrade_button;
    int bonusLimit = 25, level =1, purchased_upgrades, unlockCost;
    bool managed, loaFong, reset;
    int [] timeValues;
    Image img;
    GameController gc;
    ManagerMenu mM;



    float timeLeft;

    void Awake()
    {
        base.name = base.name.Split('_')[0];


        gc = FindObjectOfType<GameController>();
        img = GetComponent<Image>();
        unlockCost = baseCure;
        currentCure = baseCure;
        cureTime = baseCureTime;
        upgradeCost = baseUpgradeCost;

      

        //if (PlayerPrefs.HasKey("Managed" + name))
        //    managed = true;





    }
    // Use this for initialization
    void Start () {
        //  DataRecovery();
        upgrade_button.SetCost(purchased_upgrade_cost);
        //{ CancelInvoke("RecalculateTime");
        //    InvokeRepeating("RecalculateTime", 0, 1);
        //}


    }

    // Update is called once per frame
    void Update () {
        if (!recovered)
            DataRecovery();
            if (managed)
        {
            Cure();
        }

        if (!unlocked)
        {
            if (gc.GetCure() > unlockCost)
            {
                img.color = new Color(0.1f, 0.6f, 0.3f);

            }
            else img.color = new Color(0.1f, 0.1f, 0.1f);

        }

        if (!unlocked)
        {
            gainText.color = new Color(1, 1, 1, 0);
            timeText.color = new Color(1, 1, 1, 0);

        }
        else
        {
            gainText.color = new Color(1, 1, 1, 1);
            timeText.color = new Color(1, 1, 1, 1);
        }

        // RecalculateTime();
        CheckUpgrades();
        gainText.text = "Guadagno: " + GameController.ConvertScore(CalculateCure());

    }


    void CheckUpgrades()
    {
        if (gc.GetCure() > purchased_upgrade_cost)
        {
              upgrade_button.ChangeColour(new Color(1f, 1f, 1f));
        }
        else { upgrade_button.ChangeColour(new Color(0f, 0f, 0f));
        }
    }

    void RecalculateTime()
    {
      

        string timeLeftText = "Tempo: " ;
        //float whatIsLeft;
        int difference;
        bool showSeconds =true;

        if (loaFong)
        {
            timeLeft--;
        }
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
            timeLeftText += "0s ";
           // timeLeft = cureTime;
            

        }
        timeText.text = timeLeftText;
        if (managed)
            timeText.text = timeLeftText + " (Manager)";
    }

   public void Cure()
    {
        if (unlocked)
        {
            if (!loaFong)
            {
                StartCoroutine(Increase(cureTime));
                loaFong = true;
                img.color = new Color(1f, 0.3f, 0f);

            }
        }

        else
        {
            if (gc.DecreaseCure(unlockCost))
            {
                UnLock();
            }



        }

    }

    void UnLock()
    {

        unlocked = true;
        lockedText.SetActive(false);
        img.color = new Color(1f, 1f, 1f);
        levelText.text = "LV " + GameController.ConvertScore(level);
    //    PlayerPrefs.SetInt("Level" + name, level);

    //    PlayerPrefs.SetInt(name + "unlock", 1);

    }

    IEnumerator Increase(float timer)
    {
        yield return new WaitForSeconds(timer);
        gc.IncreaseCure(CalculateCure());
        loaFong = false;
        if(!managed)
        img.color = new Color(1f, 1f, 1f);
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
        gc.IncreaseCure(CalculateCure()*multiples);

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

        for (int i = 1; i < count; i++)
        {


            CalculateUpgrade();
        }
    }

    float CalculateCure()
    {
        return currentCure * cureModifiers;
    }

    void CalculateUpgrade()
    {
        currentCure+=baseCure;
        upgradeCost *= upgradeCostModifier;
        gainText.text = "Guadagno: " + GameController.ConvertScore(CalculateCure());
        level++;
        
     //   PlayerPrefs.SetInt("Level" + name, level);

        levelText.text = "LV " + GameController.ConvertScore(level);
        upgradeText.text = GameController.ConvertScore(upgradeCost) +"\nFon" ;


        if (level >= bonusLimit)
        {
            if (cureTime > 0.5f)
            {
                cureTime /= 2;
            }
            else cureModifiers *= 2;
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
              
                mM.RemoveManager(name);
                managerAlert.SetActive(true);

                Invoke("CloseAlert", 3);
                GameController.instance.OpenMenu();
                GameController.instance.OpenMenu();

                //     PlayerPrefs.SetInt("Managed" + name, 1);

            }
        }
}

    void CloseAlert()
    {
        managerAlert.SetActive(false);
    }


    //Closing game
    void OnApplicationQuit()
    {
        float time=0;
        System.DateTime now = System.DateTime.Now;
        time += now.Second + now.Minute * 60 + now.Hour * 60 * 60 + now.DayOfYear * 60 * 60 * 24 + now.Year * 60 * 60 * 24 * 365;
        PlayerPrefs.SetInt("LastSecond", now.Second);
        PlayerPrefs.SetInt("LastMinute", now.Minute);
        PlayerPrefs.SetInt("LastHour", now.Hour);
        PlayerPrefs.SetInt("LastDay", now.DayOfYear);
        PlayerPrefs.SetInt("LastYear", now.Year);

        if (reset) {
            DataSavings.instance.Erase();
            Time.timeScale = 0;
            RefreshPrefs(); }

    }

    //Purchasing Upgrade from upgrade menu
    public void PurchaseUpgrade()
    {
        if (gc.DecreaseCure(purchased_upgrade_cost))
        {
            purchased_upgrades++;
            cureModifiers *= 3;
            purchased_upgrade_cost*= 10;
            upgrade_button.SetCost(purchased_upgrade_cost);
      //      PlayerPrefs.SetInt("Purchased_Upgrades", purchased_upgrades);
        }
    }

    void InstantPurchaseUpgrade()
    {
        purchased_upgrades++;
        cureModifiers *= 3;
        purchased_upgrade_cost *= 10;
        upgrade_button.SetCost(purchased_upgrade_cost);
    }

    //Resetting PlayerPrefs
    void RefreshPrefs()
    {
        PlayerPrefs.DeleteAll();
        reset = true;
        gc.Reset();
    }

    public ResearchData Serialize()
    {
        return new ResearchData(level, purchased_upgrades, managed, unlocked, name) ;
    }

    public void Deserialize(ResearchData rd)
    {
        level = rd.level;
        purchased_upgrades = rd.upgrades;
        managed = rd.managed;
        unlocked = rd.unlocked;
        DataRecovery();
    }

    public void DataRecovery()
    {
        int tmpLv = level;
        level = 1;
        InstantUpgrade(tmpLv);

        if (unlocked)
            UnLock();

        upgradeText.text = GameController.ConvertScore(upgradeCost)+"\nFon";

        mM = FindObjectOfType<ManagerMenu>();

        if (managed)
        {
            int multiples = 0;
            float difference = 0;
            mM.RemoveManager(name);
            System.DateTime now = System.DateTime.Now;
            if (PlayerPrefs.HasKey("LastSecond"))
            {
                int tmpTime = 0;
                int tmpValue = PlayerPrefs.GetInt("LastYear");

                if (now.Year != tmpValue)
                {
                    tmpTime += 60 * 60 * 24 * 365 * (now.Year - tmpValue);

                }

                tmpValue = PlayerPrefs.GetInt("LastDay");
                if (now.DayOfYear != tmpValue)
                {
                    tmpTime += 60 * 60 * 24 * (now.DayOfYear - tmpValue);

                }

                tmpValue = PlayerPrefs.GetInt("LastHour");

                if (now.Hour != tmpValue)
                {

                    tmpTime += 60 * 60 * (now.Hour - tmpValue);

                }

                tmpValue = PlayerPrefs.GetInt("LastMinute");

                if (now.Minute != tmpValue)
                {

                    tmpTime += 60 * (now.Minute - tmpValue);

                }

                tmpValue = PlayerPrefs.GetInt("LastSecond");

                tmpTime += (now.Second - tmpValue);





                multiples = (int)(tmpTime / cureTime);
                InstantIncrease(multiples);

                difference = (tmpTime) - (multiples * cureTime);
                StartCoroutine(Increase(difference));
                timeLeft = difference;
            }
            else timeLeft = 0;

        }
        else timeLeft = 0;

        //if (PlayerPrefs.HasKey("Purchased_Upgrades"))
        //{
        //    for(int i=0; i<PlayerPrefs.GetInt("Purchased_Upgrades"); i++)
        //    {
        //        InstantPurchaseUpgrade();
        //    }
        //}

        int upgradesCounter = purchased_upgrades;
        purchased_upgrades = 0;
        for (int i = 0; i < upgradesCounter; i++)
        {
            InstantPurchaseUpgrade();
        }

        gainText.text = "Guadagno: " + GameController.ConvertScore(CalculateCure());


        if (!unlocked)
        {
            img.color = new Color(0.1f, 0.1f, 0.1f);
            gainText.color = new Color(1, 1, 1, 0);
            timeText.color = new Color(1, 1, 1, 0);

        }
        else { img.color = new Color(1f, 1f, 1f);
            gainText.color = new Color(1, 1, 1, 1);
            timeText.color = new Color(1, 1, 1, 1);
        }

        InvokeRepeating("RecalculateTime", 0, 1);
        recovered = true;
        if (timeLeft == 0)
            timeLeft = cureTime;
      //  upgrade_button.SetName(base.name + " Research x 3");
       // upgrade_button.SetCost(purchased_upgrade_cost);

    }
}

[Serializable]
public class ResearchData
{
    public int level;
    public int upgrades;
    public  bool managed;
    public bool unlocked;
    public string name;

    public ResearchData(int level, int upgrades, bool managed, bool unlocked, string name)
    {
        this.level = level;
        this.upgrades = upgrades;
        this.managed = managed;
        this.unlocked = unlocked;
        this.name = name;
    }

    

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine;

public class LOCO : MonoBehaviour
{
    public float Fire;
    public int MaxTemp;
    public Slider Termometr;
    public bool Gem;
    public float TermoIsolation = 1;
    

    public int FiremanCount = 1;
    public Animator[] FireMan;
    public Animator LOCOwagon;
    public Animator LOCODrill;

    public int MaxCoal;
    public int Coal;
    public int MaxFood;
    public float Food;
    public int MaxStone;
    public float Stone;
    public Slider CoalSlider;
    public Slider FoodSlider;
    public Slider StoneSlider;
    public Text CoalText;
    public Text FoodText;
    public Text StoneText;
    public Text Distance;
    public float SetDistance;
    public float BGHeight;

    public ParticleSystem[] GroundParticles;

    public GameObject Cam;
    public Vector2 TargetCamPos;
    public float TargetCamSize;
    public float SpeedCam;
    public bool InGame;
    public bool BildMode;
    public bool DelMode;

    public GameObject[] WagonPrefab;
    public List<GameObject> Wagons;

    public int Money;
    public Text[] MoneyText;

    public int Humans;

    public Text[] ChangingPrices;

    public string FileName = "Assets/LOCO/Save.txt";

    public GameObject[] PauseInterface; // 0- pansel 1- ON button 2- OFF Button 
    public GameObject[] InterfaceMenu; // 0-Start 1-Game
    public GameObject ForGame;
    public bool ExitBool;
    private float SpeedExit;

    void Start()
    {
        Load(); 
    }
    void Update()
    {
        Cam.transform.position = new Vector3 (Mathf.Lerp (TargetCamPos.x,Cam.transform.position.x,SpeedCam), Mathf.Lerp(TargetCamPos.y, Cam.transform.position.y, SpeedCam), -10);
        Cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(TargetCamSize, Cam.GetComponent<Camera>().orthographicSize, SpeedCam);
        if (InGame == true)
        {
            TargetCamPos = new Vector2(0, 0);
            if (Cam.GetComponent<Camera>().orthographicSize < 0.1f) { TargetCamSize = 0.75f; }

           

            if (Fire > 0 && Gem == false) { SetDistance += Fire / 2 * Time.deltaTime * 10;Stone += (Fire / 2 * Time.deltaTime * Random.Range(5f, 20f)); }  //движетс€ если гема нема
            if (Fire > 0) { Fire -= Time.deltaTime * Wagons.Count * TermoIsolation; }
            if (Fire < 0) { Fire = 0; }
            Termometr.maxValue = MaxTemp;
            Termometr.minValue = -(MaxTemp / 2);
            Termometr.value = Mathf.Lerp (Termometr.value,Fire,0.5f);
            if (Fire >= MaxTemp) { Fire = MaxTemp / 4; }

            Distance.text = "вы праехал≥: " + (int)SetDistance + "ћ";

            StoneSlider.maxValue = MaxStone;
            if (Stone > MaxStone) { Stone = MaxStone; }
            StoneSlider.value = Stone;
            StoneText.text = (int)Stone + "/" + MaxStone;

            if (Coal > MaxCoal) { Coal = MaxCoal; }
            CoalSlider.maxValue = MaxCoal;
            CoalSlider.value = Coal;
            CoalText.text = Coal + "/" + MaxCoal;

            FoodSlider.maxValue = MaxFood;
            FoodSlider.value = Food;
            FoodText.text = (int)Food + "/" + MaxFood;
            Food -= Time.deltaTime * (Humans + FiremanCount);
            if (Food <= 0) { ToMenu(); Food = 0; }

            

            if (Input.GetMouseButtonDown(0) && Coal >= FiremanCount && ExitBool == false)
            {
                for (int i = 0; i < FiremanCount; i++)
                {
                    FireMan[i].SetInteger("Coal", Coal);
                    FireMan[i].SetTrigger("Push");
                }
                Fire += 0.5f * FiremanCount;
                Coal -= 1 * FiremanCount;
            }

            LOCOwagon.SetFloat("Speed", Fire + 0.1f);
            LOCODrill.SetFloat("Speed", Fire * 2);

            //тут про партиклы земли
            if (Gem == false) { for (int i = 0; i < GroundParticles.Length; i++) { GroundParticles[i].startSpeed = Fire * 10; GroundParticles[i].emissionRate = Fire * 20; } }

            if (ExitBool == true)
            {
                if (Fire > 0) { Fire -= Time.deltaTime; }
                if (Fire <= 0)
                {
                    Fire = 0;
                    for (int i=1;i < Wagons.Count; i++)
                    {
                        Wagons[i].transform.position = Vector2.Lerp (Wagons[i].transform.position, new Vector2(Wagons[0].transform.position.x - i, 0),0.5f );
                    }
                    gameObject.transform.position = new Vector2(gameObject.transform.position.x - SpeedExit, 0);
                    SpeedExit += Time.deltaTime/3;
                }

                if (gameObject.transform.position.x < -3)
                {
                    TargetCamSize = 0.01f;
                    if (Cam.GetComponent<Camera>().orthographicSize < 0.15f) { ToMenuFinal(); }
                }
            }
        }
        if (InGame == false)
        {
            if (BildMode == false)
            {
                TargetCamPos = new Vector2(0, -0.3f);
                if (Cam.GetComponent<Camera>().orthographicSize < 0.1f) { TargetCamSize = 0.75f; }
                DelMode = false;
            }
            else
            {
                TargetCamPos = new Vector2( Mathf.Lerp(Wagons[0].transform.position.x, Wagons[Wagons.Count - 1].transform.position.x, 0.5f),-0.3f); 
                TargetCamSize = (0.75f * Wagons.Count) ;
                for (int i =1;i<Wagons.Count; i++)
                {
                    Wagons[i].transform.position = new Vector2(Mathf.Lerp(Wagons[i - 1].transform.position.x - 1, Wagons[i].transform.position.x, 0.75f),0);
                }
                if (DelMode == true)
                {
                    Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    GameObject WagonToDell = null;
                    for (int i =1;i< Wagons.Count; i++)
                    {
                        if (Vector2.Distance(Wagons[i].transform.position,MousePos) < 0.45f)
                        {
                            Wagons[i].GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 0.5f);WagonToDell = Wagons[i];
                        }
                        else { Wagons[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1); }
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        Wagons.Remove(WagonToDell);
                        if (WagonToDell != null && Vector2.Distance(WagonToDell.transform.position, MousePos) < 0.45f) { Destroy(WagonToDell); }
                        DelMode = false;
                    }
                }
            }
            for (int i = 0; i < MoneyText.Length; i++) { MoneyText[i].text = ""+Money; }


        }
    }
    public void ToGame()
    {
        InGame = true;
        CalculateWagons();
        NoPause();
        Coal = MaxCoal;
        Food = MaxFood;
        Stone = 0;
        SetDistance = 0;
        BGHeight = Random.Range(-10f, 10f); 
        TargetCamSize = 0.01f;
    }
    public void ToMenu()
    {
        PauseInterface[0].SetActive(false); PauseInterface[1].SetActive(true); PauseInterface[2].SetActive(false);
        InterfaceMenu[1].SetActive(false);
        ExitBool = true;
        
    }
    public void ToMenuFinal()
    {
        Money += (int)Stone;
        InGame = false;
        LOCOwagon.SetFloat("Speed", Fire + 0.1f);
        LOCODrill.SetFloat("Speed", Fire * 2);

        InterfaceMenu[0].SetActive(true); 
        ForGame.SetActive(false);

        for (int i = 0; i < Wagons.Count; i++) { Wagons[i].transform.position = new Vector2(-i, 0); }
        
        ExitBool = false;
        SpeedExit = 0;
        Save();
    }
    public void Pause() { Time.timeScale = 0;}
    public void NoPause() { Time.timeScale = 1; }


    public void ToBildMode() { BildMode = true; }
    public void ExitBildMode() { BildMode = false; TargetCamSize = 0.75f;Save();  }
    public void ChangeDelMode() { DelMode = true; }

    public void CalculateWagons()
    {
        MaxCoal = 100;
        MaxFood = 100;
        MaxStone = 350;
        Humans = FiremanCount;
        //стартовые показатели LOCO

        for(int i = 1; i < Wagons.Count; i++)
        {
            if (Wagons[i].GetComponent<Wagon>().WagonType == 0) { MaxStone += 1500; }
            if (Wagons[i].GetComponent<Wagon>().WagonType == 1) { Wagons[i].GetComponent<Wagon>().CoalToCalculate = MaxCoal; MaxCoal += 600; }
            if (Wagons[i].GetComponent<Wagon>().WagonType == 2) { MaxFood += 1000; }
            if (Wagons[i].GetComponent<Wagon>().WagonType == 3) { Humans += 6; }
            if (Wagons[i].GetComponent<Wagon>().WagonType == 4) { Humans += 1; }
        }
    }
    public void HideFireman()
    {
        if (Money >= 15000 && FiremanCount < 3)
        {
            Money -= 15000;
            FireMan[FiremanCount].gameObject.SetActive(true);  
            FiremanCount += 1;
        }
    }
    public void UpdateTermoIsolation()
    {
        if (Money >= ((1 - (TermoIsolation - 0.1f))* 10000 ) && TermoIsolation > 0.1f)
        {
            Money -= (int)((1 - (TermoIsolation - 0.1f)) * 10000) ;
            TermoIsolation -= 0.1f;
            ChangingPrices[0].text = "(" + (int)((1 - (TermoIsolation - 0.1f)) * 10000) + ")";
        }
    }
    public void UpdateMaxTemp()
    {
        if (Money >= (MaxTemp * 200))
        {
            Money -= MaxTemp * 200;
            MaxTemp += 2;
            ChangingPrices[1].text = "("+(MaxTemp * 200)+")";
        }
    }
    public void CreateMilitaryWagon()
    {
        if (Money >= 3000 && BildMode == true)
        {
            Money -= 3000;
            Instantiate(WagonPrefab[0], new Vector2(-Wagons.Count - 0.5f, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
        if (BildMode == false)
        {
            Instantiate(WagonPrefab[0], new Vector2(-Wagons.Count, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
        
    }
    public void CreateSkladWagon()
    {
        if (Money >= 1500 && BildMode == true)
        {
            Money -= 1500;
            Instantiate(WagonPrefab[1], new Vector2(-Wagons.Count - 0.5f, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
        if (BildMode == false)
        {
            Instantiate(WagonPrefab[1], new Vector2(-Wagons.Count, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
    }
    public void CreateUglyWagon()
    {
        if (Money >= 5500 && BildMode == true)
        {
            Money -= 5500;
            Instantiate(WagonPrefab[4], new Vector2(-Wagons.Count - 0.5f, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].GetComponent<Wagon>().loco = gameObject.GetComponent<LOCO>(); 
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
        if (BildMode == false)
        {
            Instantiate(WagonPrefab[4], new Vector2(-Wagons.Count, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].GetComponent<Wagon>().loco = gameObject.GetComponent<LOCO>();
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
    }
    public void CreateKuhniaWagon()
    {
        if (Money >= 2500 && BildMode == true)
        {
            Money -= 2500;
            Instantiate(WagonPrefab[2], new Vector2(-Wagons.Count - 0.5f, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
        if (BildMode == false)
        {
            Instantiate(WagonPrefab[2], new Vector2(-Wagons.Count, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
    }
    public void CreateMedWagon()
    {
        if (Money >= 2500 && BildMode == true)
        {
            Money -= 2500;
            Instantiate(WagonPrefab[3], new Vector2(-Wagons.Count - 0.5f, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
        if (BildMode == false)
        {
            Instantiate(WagonPrefab[3], new Vector2(-Wagons.Count, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
    }
    public void CreateArheologWagon()
    {
        if (Money >= 12500 && BildMode == true)
        {
            Money -= 12500;
            Instantiate(WagonPrefab[5], new Vector2(-Wagons.Count - 0.5f, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
        if (BildMode == false)
        {
            Instantiate(WagonPrefab[5], new Vector2(-Wagons.Count, 0), gameObject.transform.rotation);
            Wagons.Add(GameObject.FindGameObjectWithTag("Wagon"));
            Wagons[Wagons.Count - 1].tag = "Untagged";
        }
    }
    public void Save()
    {
        StreamWriter Save = new StreamWriter(FileName);
        Save.WriteLine(Money);
        Save.WriteLine(TermoIsolation);
        Save.WriteLine(MaxTemp);
        Save.WriteLine(FiremanCount);
        Save.WriteLine(Wagons.Count - 1);
        for (int i = 1; i < Wagons.Count; i++)
        {
            Save.WriteLine(Wagons[i].GetComponent<Wagon>().WagonType);
        }
        Save.Close();
    }
    public void Load()
    {
        StreamReader Load = new StreamReader(FileName);
        if (Load != null)
        {
            Money = System.Convert.ToInt32(Load.ReadLine());
            TermoIsolation = System.Convert.ToSingle(Load.ReadLine());
            MaxTemp = System.Convert.ToInt32(Load.ReadLine());
            FiremanCount = System.Convert.ToInt32(Load.ReadLine());
            if (FiremanCount == 2) { FireMan[1].gameObject.SetActive(true); }
            if (FiremanCount == 3) { FireMan[1].gameObject.SetActive(true); FireMan[2].gameObject.SetActive(true); }

            int WagonCount = System.Convert.ToInt32(Load.ReadLine());
            for (int i = 0; i < WagonCount; i++)
            {
                int Wagon = System.Convert.ToInt32(Load.ReadLine());
                if (Wagon == 0) {  CreateSkladWagon(); }
                if (Wagon == 1) { CreateUglyWagon(); }
                if (Wagon == 2) { CreateKuhniaWagon(); }
                if (Wagon == 3) { CreateMilitaryWagon(); }
                if (Wagon == 4) { CreateMedWagon(); }
                if (Wagon == 5) { CreateArheologWagon(); }
            }
        }
    }
}

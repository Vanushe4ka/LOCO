using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wagon : MonoBehaviour
{
    public int WagonType;

    [Header("For UglyWagon")]
    public GameObject[] Ugly;
    public int CoalInWagon;
    public int CoalToCalculate;
    public LOCO loco;

    [Header("For ArheologWagon")]
    public float LuckLevel = 1;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (WagonType == 1)
        {
            CoalInWagon = loco.Coal - CoalToCalculate;

            for (int i =0; i < 6; i++)
            {
                if (Mathf.CeilToInt(CoalInWagon / 100f) <= i)
                {
                    if (Ugly[i].GetComponent<SpriteRenderer>().enabled == true)
                    {
                        Ugly[i].GetComponent<ParticleSystem>().Play();
                        Ugly[i].GetComponent<SpriteRenderer>().enabled = false;
                    }
                }
                else { Ugly[i].GetComponent<SpriteRenderer>().enabled = true; }
            }
        }
    }
}

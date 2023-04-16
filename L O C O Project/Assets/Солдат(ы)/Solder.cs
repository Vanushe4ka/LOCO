using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solder : MonoBehaviour
{
    public bool InWagon;
    public Animator Soldier;
    private void Start()
    {
        Soldier.SetFloat("Speed", Random.Range(0.3f, 0.6f));
        InWagon = true;
    }
    void Update()
    {
        
    }
    public void NextAnim()
    {
        Soldier.SetInteger("Anim", Random.Range(0, 2));
    }
}

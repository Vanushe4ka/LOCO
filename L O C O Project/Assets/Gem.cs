using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public LOCO Loco;
    public int MaxDistanceToSpawn;
    public int MinDistanceToSpawn;
    public int Type;
    public float DistanceToSpawn;
    private Vector2 StartPos;
    public int HP = 15;
    public float FrameDistence;

    
    void Start()
    {
        DistanceToSpawn = Random.Range(MinDistanceToSpawn, MaxDistanceToSpawn);
        StartPos = gameObject.transform.position;
        
    }
    void Update()
    {
        if (Type == 0)
        {
            if (DistanceToSpawn < Loco.SetDistance) { gameObject.transform.position = new Vector2(StartPos.x - (Loco.SetDistance - DistanceToSpawn) / 10, StartPos.y); }
            if (Vector2.Distance(gameObject.transform.position, Loco.gameObject.transform.position) < 1.2f)
            {
                Loco.Gem = true;
                if (Input.GetMouseButtonDown(0) && Loco.Coal > 0) { HP -= Loco.FiremanCount; Loco.SetDistance += 0.25f; }
            }
            if (HP <= 0)
            {
                Loco.Gem = false;
                HP = Random.Range(8, 16);
                DistanceToSpawn = Loco.SetDistance + Random.Range(MinDistanceToSpawn, MaxDistanceToSpawn);
                gameObject.transform.position = StartPos;
            }
        }
        if (Type == 1)
        {
            if (DistanceToSpawn < Loco.SetDistance) { gameObject.transform.position = new Vector2(StartPos.x - (Loco.SetDistance - DistanceToSpawn) / 10, StartPos.y); }
            if (Vector2.Distance(gameObject.transform.position, Loco.gameObject.transform.position) < 1.2f && Vector2.Distance(gameObject.transform.position, Loco.gameObject.transform.position) > 0.6f)
            {
                
                if (FrameDistence == 0) { FrameDistence = Loco.SetDistance; Loco.Fire = Loco.Fire / 3; }
                float deltaDistance = Loco.SetDistance - FrameDistence;
                if (deltaDistance >= 1) { Loco.Coal += (int)(deltaDistance * Random.Range(6f,12f)); FrameDistence = Loco.SetDistance; }
            }
            
            if (Vector2.Distance(gameObject.transform.position, Loco.gameObject.transform.position) < 0.15f)
            {
                DistanceToSpawn = Loco.SetDistance + Random.Range(MinDistanceToSpawn, MaxDistanceToSpawn);
                gameObject.transform.position = StartPos;
                FrameDistence = 0;
            }
        }
        
    }
}

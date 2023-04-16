using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BG : MonoBehaviour
{
    public LOCO Loco;
    private float x;
    public MeshRenderer Mesh;

    private Vector2 MeshOffset;
    // Start is called before the first frame update
    void Start()
    {
        MeshOffset = Mesh.sharedMaterial.mainTextureOffset;

    }
    private void OnDisable()
    {
        //Mesh.sharedMaterial.mainTextureOffset = MeshOffset; 
    }

    // Update is called once per frame
    void Update()
    {
        
        var offset = new Vector2(Loco.SetDistance/10, Loco.BGHeight);
        Mesh.sharedMaterial.mainTextureOffset = offset;
    }
}

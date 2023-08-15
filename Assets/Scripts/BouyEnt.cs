using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouyEnt : MonoBehaviour
{
    public float mass;
    public int ID;
    public Vector3 myPos;
    // Start is called before the first frame update
    void Start()
    {
        ID = EntityMgr.inst.AddBouy(this);
        myPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

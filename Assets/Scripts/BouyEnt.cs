using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouyEnt : MonoBehaviour
{
    public float mass;
    public int ID;
    // Start is called before the first frame update
    void Start()
    {
        ID = EntityMgr.inst.AddBouy(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

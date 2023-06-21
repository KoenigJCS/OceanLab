using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneEnt : MonoBehaviour
{
    public enum zoneType
    {
        Summon,
        Starter,
        Moveer,
        Ender
    }
    
    public zoneType myZoneType;
    public direction myDirection;
    public int myID;
    public Rect myArea;
    // Start is called before the first frame update
    void Start()
    {
        ZoneMgr.inst.AddZone(this);
        myArea.xMin=transform.position.x-(transform.localScale.x/2);
        myArea.xMax=transform.position.x+(transform.localScale.x/2);
        myArea.yMin=transform.position.z-(transform.localScale.z/2);
        myArea.yMax=transform.position.z+(transform.localScale.z/2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum direction
{
    West,
    East,
    AcrossN,
    AcrossS
}

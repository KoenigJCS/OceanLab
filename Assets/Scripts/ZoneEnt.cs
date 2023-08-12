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
    public Direction myDirection;
    public int myID;
    public Rect myArea;
    public Vector3 myPos;
    // Start is called before the first frame update
    void Start()
    {
        ZoneMgr.inst.AddZone(this);
        myArea.xMin=transform.position.x-(transform.localScale.x/2);
        myArea.xMax=transform.position.x+(transform.localScale.x/2);
        myArea.yMin=transform.position.z-(transform.localScale.z/2);
        myArea.yMax=transform.position.z+(transform.localScale.z/2);
        myPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public enum Direction
{
    West,
    East,
    AcrossN,
    AcrossS
}

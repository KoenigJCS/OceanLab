using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneMgr : MonoBehaviour
{
    public List<ZoneEnt> zoneEnts;
    public List<ZoneEnt> sumZoneEnts;
    public List<ZoneEnt> moveZoneEnts;
    ZoneEnt westEndZone;
    ZoneEnt eastEndZone;
    public static ZoneMgr inst;
    public List<GameObject> allBoats;
    public Transform moveables;

    void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddZone(ZoneEnt item)
    {
        zoneEnts.Add(item);
        switch (item.myZoneType)
        {
            case ZoneEnt.zoneType.Summon:
                sumZoneEnts.Add(item);
                break;
            case ZoneEnt.zoneType.Moveer:
                moveZoneEnts.Add(item);
                break;
            case ZoneEnt.zoneType.Ender:
                if(item.myDirection == direction.West)
                    westEndZone = item;
                else
                    eastEndZone = item;
                break;
            default:
                break;
        }
    } 

    public void SummonShip(int ammount)
    {
        for(int i = 0; i < ammount; i++)
        {
            SummonShip();
        }
    }
    public int shipDensity = 100;
    public void PopulateSummon()
    {
        for(int i = 0; i < shipDensity; i++)
        {
            ZoneEnt curZone = zoneEnts[Random.Range(0,zoneEnts.Count)];
            Vector3 newPos = new Vector3(Random.Range(curZone.myArea.xMin, curZone.myArea.xMax), 0, Random.Range(curZone.myArea.yMin, curZone.myArea.yMax));
            //My understanding of quaternions in not good enough, but this can be done much more efficently I think;
            Quaternion newRot = Random.rotation;
            newRot.x=0;
            newRot.z=0;
            GameObject newBoat = Instantiate(allBoats[Random.Range(0, allBoats.Count)], newPos, newRot, moveables);
            BoatEntity ent = newBoat.GetComponent<BoatEntity>();
            switch (curZone.myDirection)
            {
                case direction.West:
                    ent.myDirection = direction.West;
                    break;
                case direction.East:
                    ent.myDirection = direction.East;
                    break;
                case direction.AcrossN:
                    ent.myDirection = direction.AcrossN;
                    break;
                case direction.AcrossS:
                    ent.myDirection = direction.AcrossS;
                    break;
                default:
                    break;
            }
            ent.position=newPos;
            ent.moveState = 1;
            //ent.FindPath();
        }
    }

    public void SummonShip()
    {
        ZoneEnt curZone = sumZoneEnts[Random.Range(0,sumZoneEnts.Count)];
        Vector3 newPos = RandomPosInZone(curZone);
        //My understanding of quaternions in not good enough, but this can be done much more efficently I think;
        Quaternion newRot = Random.rotation;
        newRot.x=0;
        newRot.z=0;
        GameObject newBoat = Instantiate(allBoats[Random.Range(0, allBoats.Count)], newPos, newRot, moveables);
        BoatEntity ent = newBoat.GetComponent<BoatEntity>();
        switch (curZone.myDirection)
        {
            case direction.West:
                ent.myDirection = direction.West;
                break;
            case direction.East:
                ent.myDirection = direction.East;
                break;
            case direction.AcrossN:
                ent.myDirection = direction.AcrossN;
                break;
            case direction.AcrossS:
                ent.myDirection = direction.AcrossS;
                break;
            default:
                break;
        }
        ent.position=newPos;
        ent.moveState = 1;
        //ent.FindPath();
    }

    public Vector3 FindNextMover(Vector3 curPos, direction curDirection)
    {
        Vector3 desination = Vector3.zero;
        float minDelta = float.MaxValue;
        ZoneEnt closestZone = null;
        foreach(ZoneEnt zone in moveZoneEnts)
        {
            Vector3 delta = zone.transform.position - curPos;
            

            if(zone.myDirection != curDirection)
                continue;
            else if(curDirection == direction.West)
            {
                int deltaX = (int) (zone.myArea.xMin - curPos.x);
                if(delta.magnitude < minDelta && deltaX>100)
                {
                    minDelta = delta.magnitude;
                    closestZone = zone;
                }
            }
            else if(curDirection == direction.East)
            {
                int deltaX = (int) (curPos.x - zone.myArea.xMax);
                if(delta.magnitude < minDelta && deltaX>100)
                {
                    minDelta = delta.magnitude;
                    closestZone = zone;
                }
            }
            else if(curDirection == direction.AcrossN)
            {
                int deltaZ = (int) (zone.myArea.yMin - curPos.z);
                if(delta.magnitude < minDelta && deltaZ>100 && delta.magnitude < 500.0f)
                {
                    minDelta = delta.magnitude;
                    closestZone = zone;
                }
            }
            else if(curDirection == direction.AcrossS)
            {
                int deltaZ = (int) (curPos.z - zone.myArea.yMax);
                if(delta.magnitude < minDelta && deltaZ>100 && delta.magnitude < 500.0f)
                {
                    minDelta = delta.magnitude;
                    closestZone = zone;
                }
            }
            
        }
        if(closestZone != null)
        {
            desination = RandomPosInZone(closestZone);
        }
        return desination;
    }

    public Vector3 GetEnd(direction curDirection)
    {
        if(curDirection == direction.West)
            return RandomPosInZone(westEndZone);
        //else
        return RandomPosInZone(eastEndZone);
    }

    Vector3 RandomPosInZone(ZoneEnt zone)
    {
        Vector3 position = new Vector3(Random.Range(zone.myArea.xMin, zone.myArea.xMax), 0, Random.Range(zone.myArea.yMin, zone.myArea.yMax));
        return position;
    }
}

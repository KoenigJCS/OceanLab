using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
                if(item.myDirection == Direction.West)
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
            //GameObject newBoat = Instantiate(allBoats[1], newPos, newRot, moveables);
            BoatEntity ent = newBoat.GetComponent<BoatEntity>();
            switch (curZone.myDirection)
            {
                case Direction.West:
                    ent.myDirection = Direction.West;
                    break;
                case Direction.East:
                    ent.myDirection = Direction.East;
                    break;
                case Direction.AcrossN:
                    ent.myDirection = Direction.AcrossN;
                    break;
                case Direction.AcrossS:
                    ent.myDirection = Direction.AcrossS;
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
        //GameObject newBoat = Instantiate(allBoats[1], newPos, newRot, moveables);
        BoatEntity ent = newBoat.GetComponent<BoatEntity>();
        switch (curZone.myDirection)
        {
            case Direction.West:
                ent.myDirection = Direction.West;
                break;
            case Direction.East:
                ent.myDirection = Direction.East;
                break;
            case Direction.AcrossN:
                ent.myDirection = Direction.AcrossN;
                break;
            case Direction.AcrossS:
                ent.myDirection = Direction.AcrossS;
                break;
            default:
                break;
        }
        ent.position=newPos;
        ent.moveState = 1;
        //ent.FindPath();
    }
    public Vector3 FindNextMover(Vector3 curPos, Direction curDirection)
    {
        Vector3 desination = Vector3.zero;
        float minDelta = float.MaxValue;
        ZoneEnt closestZone = null;
        foreach(ZoneEnt zone in moveZoneEnts)
        {
            Vector3 delta = zone.myPos - curPos;

            if(zone.myDirection != curDirection)
                continue;
            else if(curDirection == Direction.West)
            {
                int deltaX = (int) (zone.myArea.xMin - curPos.x);
                if(delta.magnitude < minDelta && deltaX>100)
                {
                    minDelta = delta.magnitude;
                    closestZone = zone;
                }
            }
            else if(curDirection == Direction.East)
            {
                int deltaX = (int) (curPos.x - zone.myArea.xMax);
                if(delta.magnitude < minDelta && deltaX>100)
                {
                    minDelta = delta.magnitude;
                    closestZone = zone;
                }
            }
            else if(curDirection == Direction.AcrossN)
            {
                int deltaZ = (int) (zone.myArea.yMin - curPos.z);
                if(delta.magnitude < minDelta && deltaZ>100 && delta.magnitude < 500.0f)
                {
                    minDelta = delta.magnitude;
                    closestZone = zone;
                }
            }
            else if(curDirection == Direction.AcrossS)
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

    public Vector3 GetEnd(Direction curDirection)
    {
        if(curDirection == Direction.West)
            return RandomPosInZone(westEndZone);
        //else
        return RandomPosInZone(eastEndZone);
    }

    private static int _tracker = 0;
    private static ThreadLocal<System.Random> _random = new(() => {
        var seed = (int)(System.Environment.TickCount & 0xFFFFFF00 | (byte)(Interlocked.Increment(ref _tracker) % 255));
        var random = new System.Random(seed);
        return random;
    });

    Vector3 RandomPosInZone(ZoneEnt zone)
    {
        Vector3 position = new Vector3((_random.Value.Next() % (zone.myArea.xMax - zone.myArea.xMin)) + zone.myArea.xMin, 0, (_random.Value.Next() % (zone.myArea.yMax - zone.myArea.yMin)) + zone.myArea.yMin);
        return position;
    }
}

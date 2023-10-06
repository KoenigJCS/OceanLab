using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AjacencyScriptTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    int count = 1;
    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyUp(KeyCode.Backslash))
        {
            for(int i = 0;i<1;i++)
            {
                //Boat 4 way intersection
                
                // BoatEntity b1 = SummonShip(Vector3.forward*1000+(1500 * count * Vector3.left),Quaternion.Euler(0,180,0),3);
                // BoatEntity b2 = SummonShip(Vector3.left*1000+(1500 * count * Vector3.left),Quaternion.Euler(0,90,0),3);
                // BoatEntity b3 = SummonShip(Vector3.right*1000+(1500 * count * Vector3.left),Quaternion.Euler(0,-90,0),3);
                // BoatEntity b4 = SummonShip(Vector3.back*1000+(1500 * count * Vector3.left),Quaternion.Euler(0,0,0),3);

                // b1.Move(Vector3.back*2000+(1500 * count * Vector3.left));
                // b1.playerMove=true;
                // b1.gameObject.AddComponent<DataCollector>();
                // b1.GetComponent<DataCollector>().collisionTarget=b2;

                // b2.Move(Vector3.right*2000+(1500 * count * Vector3.left));
                // b2.playerMove=true;
                // b2.gameObject.AddComponent<DataCollector>();
                // b2.GetComponent<DataCollector>().collisionTarget=b3;

                // b3.Move(Vector3.left*2000+(1500 * count * Vector3.left));
                // b3.playerMove=true;
                // b3.gameObject.AddComponent<DataCollector>();
                // b3.GetComponent<DataCollector>().collisionTarget=b4;

                // b4.Move(Vector3.forward*2000+(1500 * count * Vector3.left));
                // b4.playerMove=true;


                // //Many Tiny Boats
                List<BoatEntity> tinyBoats = new();
                for(int o = 0;o<5;o++)
                {
                    for(int y = 0;y<5;y++)
                    {
                        tinyBoats.Add(SummonShip(100 * o * Vector3.forward+(100 * y * Vector3.right)+(1500 * count * Vector3.left),Quaternion.Euler(0,180,0),11));
                    }
                }

                List<BoatEntity> bigBoats = new();
                for(int o = 2;o<6;o++)
                {
                    bigBoats.Add(SummonShip(500 * o * Vector3.back+(1500 * count * Vector3.left),Quaternion.Euler(0,90,0),3));
                }

                foreach (BoatEntity bigBoat in bigBoats)
                {
                    bigBoat.playerMove=true;
                }

                foreach (BoatEntity tinyBoat in tinyBoats)
                {
                    for(int o = 2;o<6;o++)
                        tinyBoat.Move(o * 500 * Vector3.back +(1500 * count * Vector3.left)+Vector3.back*250);

                    tinyBoat.playerMove=true;
                }

                count++;
            }
        }
        if(Input.GetKeyUp(KeyCode.RightBracket))
        {
            DataMgr.inst.ShareData();
        }
    }

    public Transform moveables;

    public BoatEntity SummonShip(Vector3 newPos, Quaternion newRot,int boatNumb)
    {
                                                        //Random.Range(0, ZoneMgr.inst.allBoats.Count)
        GameObject newBoat = Instantiate(ZoneMgr.inst.allBoats[boatNumb], newPos, newRot, moveables);
        BoatEntity ent = newBoat.GetComponent<BoatEntity>();
        ent.zoneFlag=false;
        ent.position=newPos;
        ent.moveState = 1;
        return ent;
    }
}

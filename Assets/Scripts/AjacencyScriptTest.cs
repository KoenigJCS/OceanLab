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
                BoatEntity b1 = SummonShip(Vector3.forward*1000+(1500 * count * Vector3.left),Quaternion.Euler(0,180,0));
                BoatEntity b2 = SummonShip(Vector3.back*1000+(1500 * count * Vector3.left),Quaternion.Euler(0,0,0));

                b1.Move(Vector3.back*2000+(1500 * count * Vector3.left));
                b1.playerMove=true;
                b1.gameObject.AddComponent<DataCollector>();
                b1.GetComponent<DataCollector>().collisionTarget=b2;

                b2.Move(Vector3.forward*2000+(1500 * count * Vector3.left));
                b2.playerMove=true;
                //b2.gameObject.AddComponent<DataCollector>();
                //b2.GetComponent<DataCollector>().collisionTarget=b1;
                count++;
            }
        }
        if(Input.GetKeyUp(KeyCode.RightBracket))
        {
            DataMgr.inst.ShareData();
        }
    }

    public Transform moveables;

    public BoatEntity SummonShip(Vector3 newPos, Quaternion newRot)
    {
        //My understanding of quaternions in not good enough, but this can be done much more efficently I think;
        // Quaternion newRot = Random.rotation;
        // newRot.x=0;
        // newRot.z=0;
        Debug.Log("Starting Scenario!");
                                                        //Random.Range(0, ZoneMgr.inst.allBoats.Count)
        GameObject newBoat = Instantiate(ZoneMgr.inst.allBoats[3], newPos, newRot, moveables);
        BoatEntity ent = newBoat.GetComponent<BoatEntity>();
        ent.zoneFlag=false;
        ent.position=newPos;
        ent.moveState = 1;
        return ent;
    }
}

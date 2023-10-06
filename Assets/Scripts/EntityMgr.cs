using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EntityMgr : MonoBehaviour
{
    public List<BoatEntity> boatEntities;
    List<BoatEntity> curBoatEntities;
    public List<BoatEntity> pathlessBoats;
    public List<BoatEntity> potentialFieldBoats;
    public List<BouyEnt> bouyEntities;
    List<BouyEnt> curBouyEntities;
    // Start is called before the first frame update
    public static EntityMgr inst;
    public int gameSpeed =1;
    public UnityEngine.UI.Slider mySlider;
    void Awake()
    {
        inst = this;
    }
    Thread t1;
    public bool t1IsRunning = false;
    Thread t2;
    public bool t2IsRunning = false;

    int i1=0;
    int i2=0;

    void Start()
    {
        SetGameSpeed();
    }
    // Update is called once per frame
    private void Update()
    {
       if(pathlessBoats.Count>0 && !t1IsRunning)
       {
            t1 = new Thread(ThreadedPathing) {Name = "Thread 1"};
            t1IsRunning=true;
            t1.Start();
       }
       if(potentialFieldBoats.Count>0 && !t2IsRunning)
       {
            t2 = new Thread(ThreadedFields) {Name = "Thread 2"};
            t2IsRunning=true;
            curBoatEntities = new(boatEntities);
            curBouyEntities = new(bouyEntities);
            t2.Start();
       }
       if(CameraControlls.inst.optimizeShipTextureFlag)
       {
            CameraControlls.inst.optimizeShipTextureFlag=false;
            foreach (BoatEntity singleBoat in boatEntities)
            {
                if(singleBoat.GetComponent<TextureOptimizer>())
                {
                    singleBoat.GetComponent<TextureOptimizer>().CheckToOptimize();
                }
            }
       }
    }

    public void ThreadedFields()
    {
        lock(potentialFieldBoats)
        {
            foreach(BoatEntity curEnt in potentialFieldBoats)
            {
                Vector3 dif = Vector3.zero;
                Vector3 repelPotential = Vector3.zero;
                float magnitude;
                foreach (BoatEntity selectedEnt in curBoatEntities)
                {
                    if (selectedEnt.ID == curEnt.ID)
                        continue;

                    dif = selectedEnt.position - curEnt.position;
                    magnitude = dif.magnitude;
                    Vector3 crossDif=Vector3.Cross(dif,Vector3.up);
                    float closestDist = Utils.ClosestDistOfApproach(curEnt.position, curEnt.velocity, selectedEnt.position, selectedEnt.position);
                    if ((closestDist < AIMgr.inst.tooClose * selectedEnt.mass || curEnt.IsCBDR(selectedEnt)) && magnitude < AIMgr.inst.potentialDistanceMax)
                    {
                        //Idea 1 relatve angle fields
                        // float angleRatio = Mathf.Clamp(Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad*Mathf.Clamp(curEnt.heading-selectedEnt.heading,-120f,120f)))+.4f,.4f,1f);
                        // repelPotential += angleRatio* AIMgr.inst.aAvoidance * Mathf.Pow(magnitude, AIMgr.inst.eAvoidance) * selectedEnt.mass * dif.normalized;
                        // repelPotential += angleRatio* AIMgr.inst.aCrossAvoidance * Mathf.Pow(magnitude, AIMgr.inst.eCrossAvoidance) * selectedEnt.mass * crossDif.normalized;

                        //Idea 2 trig
                        repelPotential += AIMgr.inst.aAvoidance * Mathf.Pow(magnitude, AIMgr.inst.eAvoidance) * selectedEnt.mass * dif.normalized;
                        repelPotential += AIMgr.inst.aCrossAvoidance * Mathf.Pow(magnitude, AIMgr.inst.eCrossAvoidance) * selectedEnt.mass * crossDif.normalized;
                        repelPotential.x*= Mathf.Clamp(Mathf.Abs(Mathf.Sin(Mathf.Deg2Rad*selectedEnt.heading))+0.5f,0.5f,1f);
                        repelPotential.z*= Mathf.Clamp(Mathf.Abs(Mathf.Cos(Mathf.Deg2Rad*selectedEnt.heading))+0.5f,0.5f,1f);
                    }
                }

                foreach (BouyEnt selectedEnt in curBouyEntities)
                {
                    dif = selectedEnt.myPos -curEnt.position;
                    magnitude = dif.magnitude;
                    //float closestDist = Utils.ClosestDistOfApproach(position, velocity, ent.transform.position, ent.transform.position);
                    if (magnitude < AIMgr.inst.tooClose * selectedEnt.mass)
                        repelPotential += AIMgr.inst.aAvoidance * Mathf.Pow(magnitude, AIMgr.inst.eAvoidance) * selectedEnt.mass * dif.normalized;
                }
                curEnt.repelPotential = repelPotential;
            }
        }
        t2IsRunning=false;
    }

    public void ThreadedPathing()
    {
        List<BoatEntity> boatsToPath;
        lock(pathlessBoats)
        {
            boatsToPath = new List<BoatEntity>(pathlessBoats);
            pathlessBoats.Clear();
        }

        foreach (BoatEntity singleBoat in boatsToPath)
        {
            singleBoat.FindPath();
        }
    
        t1IsRunning=false;
    }

    public int AddBoat(BoatEntity item)
    {
        boatEntities.Add(item);
        return i1++;
    }

    public int AddBouy(BouyEnt item)
    {
        bouyEntities.Add(item);
        return i2++;
    }
    
    public void SetGameSpeed()
    {
        gameSpeed = (int) mySlider.value;
    }

    
}

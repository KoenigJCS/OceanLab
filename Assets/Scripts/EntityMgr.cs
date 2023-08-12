using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EntityMgr : MonoBehaviour
{
    public List<BoatEntity> boatEntities;
    public List<BoatEntity> pathlessBoats;
    public List<BouyEnt> bouyEntities;
    // Start is called before the first frame update
    public static EntityMgr inst;
    public int gameSpeed;
    public UnityEngine.UI.Slider mySlider;
    int i1 = 0;
    int i2 = 0;
    void Awake()
    {
        inst = this;
    }

    void start()
    {

    }
    Thread t1;
    bool t1IsRunning = false;
    // Update is called once per frame
    void Update()
    {
       if(pathlessBoats.Count>0 && !t1IsRunning)
       {
            t1 = new Thread(ThreadedPathing) {Name = "Thread 1"};
            t1IsRunning=true;
            t1.Start();
       }
    }

    void ThreadedPathing()
    {
        List<BoatEntity> boatsToPath;
        lock(pathlessBoats)
        {;
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

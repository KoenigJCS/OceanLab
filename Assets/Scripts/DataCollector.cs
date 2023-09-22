using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataCollector : MonoBehaviour
{
    public BoatEntity collisionTarget;
    public BoatEntity thisBoat;
    // Start is called before the first frame update
    void Start()
    {
        thisBoat = gameObject.GetComponent<BoatEntity>();
        DataMgr.inst.dataCollectors.Add(this);
    }
    public float minDist = float.MaxValue;
    // Update is called once per frame
    float magnitude;
    void Update()
    {
        magnitude = (thisBoat.position-collisionTarget.position).magnitude;
        if(magnitude<minDist)
            minDist=magnitude;
    }
}

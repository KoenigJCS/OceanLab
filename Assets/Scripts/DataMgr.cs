using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : MonoBehaviour
{
    public static DataMgr inst;
    void Awake()
    {
        inst = this;
    }

    public List<DataCollector> dataCollectors;

    void Start()
    {
        dataCollectors = new();
    }

    public void ShareData()
    {
        float min = float.MaxValue;
        float max = float.MinValue;
        float sum = 0.0f;
        string output = "";
        foreach (var dataPoint in dataCollectors)
        {
            if(dataPoint.minDist<min)
                min=dataPoint.minDist;
            if(dataPoint.minDist>max)
                max=dataPoint.minDist;
            sum+=dataPoint.minDist;
            //output += dataPoint.minDist + "\n";
        }
        output+="Min: "+min+"\nMax: "+max+"\nAvg: "+(sum/dataCollectors.Count); 
        Debug.Log(output);
    }
}

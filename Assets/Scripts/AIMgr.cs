using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMgr : MonoBehaviour
{
    [Header("Force Constants")]
    public float aAvoidance;
    public float eAvoidance;
    public float aAttraction;
    public float eAttraction;
    public static AIMgr inst;
    public float potentialDistanceMax; 
    public float tooClose;
    void Awake()
    {
        inst = this;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

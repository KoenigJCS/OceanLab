using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlls : MonoBehaviour
{
    public static CameraControlls inst;
    private void Awake() {
        inst = this;
    }

    void Start() 
    {
        
    }

    [Header("Nodes")]
    public GameObject RTSCamera;
    public GameObject YawNode; //Child of ^
    public GameObject PitchNode; //Child of ^
    public GameObject RollNode; //Child of ^
    [Header("Calibration")]
    public float cameraSenitiivity = 100;
    public float turnRate = 50;
    
    public Vector3 currentPitchEulerAngles= Vector3.zero;
    public Vector3 currentYawEulerAngles= Vector3.zero;
    public bool isRTSMode = true;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            YawNode.transform.Translate(Vector3.forward * Time.deltaTime * cameraSenitiivity);
        }
        if(Input.GetKey(KeyCode.S))
        {
            YawNode.transform.Translate(Vector3.back * Time.deltaTime * cameraSenitiivity);
        }

        if(Input.GetKey(KeyCode.A))
        {
            YawNode.transform.Translate(Vector3.left * Time.deltaTime * cameraSenitiivity);
        }
        if(Input.GetKey(KeyCode.D))
        {
            YawNode.transform.Translate(Vector3.right * Time.deltaTime * cameraSenitiivity);
        }

        if(Input.GetKey(KeyCode.R))
        {
            YawNode.transform.Translate(Vector3.up * Time.deltaTime * cameraSenitiivity);
        }
        if(Input.GetKey(KeyCode.F))
        {
            YawNode.transform.Translate(Vector3.down * Time.deltaTime * cameraSenitiivity);
        }

        currentYawEulerAngles = YawNode.transform.localEulerAngles;
        if(Input.GetKey(KeyCode.Q))
        {
            currentYawEulerAngles.y -= turnRate * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.E))
        {
            currentYawEulerAngles.y += turnRate * Time.deltaTime;
        }
        YawNode.transform.localEulerAngles = currentYawEulerAngles;

        currentPitchEulerAngles = PitchNode.transform.localEulerAngles;
        if(Input.GetKey(KeyCode.Z))
        {
            currentPitchEulerAngles.x -= turnRate * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.X))
        {
            currentPitchEulerAngles.x += turnRate * Time.deltaTime;
        }

        PitchNode.transform.localEulerAngles = currentPitchEulerAngles;

        if(Input.GetKeyUp(KeyCode.C))
        {
            if(isRTSMode && SelectionMgr.inst.selectedBoats.Count==1)
            {
                BoatEntity boat = SelectionMgr.inst.selectedBoats[0];
                YawNode.transform.SetParent(boat.myCameraNode.transform);
                YawNode.transform.localPosition = Vector3.zero;
                YawNode.transform.localEulerAngles = Vector3.zero;

                SelectionMgr.inst.selectedEntity = boat;
                boat.Stop();
                boat.moveState = 3;
                boat.desiredHeading=boat.heading;
                boat.desiredSpeed=boat.speed;
            }
            else
            {
                YawNode.transform.SetParent(RTSCamera.transform);
                YawNode.transform.localPosition = Vector3.zero;
                YawNode.transform.localEulerAngles = Vector3.zero;
            }
            isRTSMode = !isRTSMode;
        }
    }
}


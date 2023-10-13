using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public bool gyroMove = false;

    public bool optimizeShipTextureFlag = false;
    int frameDivider = 0;
    // Update is called once per frame
    void Update()
    {
        switch (ControlMgr.inst.curControlType)
        {
            case ControlType.KeyboardMouse:
            if(Input.GetKey(KeyCode.W))
            {
                YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.forward);
            }
            if(Input.GetKey(KeyCode.S))
            {
                YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.back);
            }

            if(Input.GetKey(KeyCode.A))
            {
                YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.left);
            }
            if(Input.GetKey(KeyCode.D))
            {
                YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.right);
            }

            if(Input.GetKey(KeyCode.R))
            {
                YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.up);
            }
            if(Input.GetKey(KeyCode.F))
            {
                YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.down);
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
            frameDivider++;
            if(frameDivider==20)
            {
                optimizeShipTextureFlag = true;
                frameDivider=0;
            }
            break;
            case ControlType.Controller:
            //Not Implemented
            break;
            case ControlType.SteamDeck:
            if(!gyroMove)
            {
                float xAxis = Input.GetAxis("Horizontal");
                YawNode.transform.Translate(xAxis*cameraSenitiivity * Time.deltaTime * Vector3.right);
                float yAxis = Input.GetAxis("Vertical");
                YawNode.transform.Translate(yAxis*cameraSenitiivity * Time.deltaTime * Vector3.forward);
                if(Input.GetKey(KeyCode.W))
                {
                    YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.forward);
                }
                if(Input.GetKey(KeyCode.S))
                {
                    YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.back);
                }

                if(Input.GetKey(KeyCode.R))
                {
                    YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.up);
                }
                if(Input.GetKey(KeyCode.F))
                {
                    YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.down);
                }
                if(Input.GetKey(KeyCode.B))
                {
                    YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.up);
                }
                if(Input.GetKey(KeyCode.A))
                {
                    YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * Vector3.down);
                }
            }
            else
            {
                currentYawEulerAngles = YawNode.transform.localEulerAngles;
                currentPitchEulerAngles = PitchNode.transform.localEulerAngles;

                currentYawEulerAngles.y += turnRate * Time.deltaTime * Input.GetAxis("Mouse X");
                currentPitchEulerAngles.x -= turnRate * Time.deltaTime * Input.GetAxis("Mouse Y");

                YawNode.transform.localEulerAngles = currentYawEulerAngles;
                PitchNode.transform.localEulerAngles = currentPitchEulerAngles;

                if(Input.GetKey(KeyCode.B))
                {
                    YawNode.transform.Translate(cameraSenitiivity * Time.deltaTime * RTSCamera.transform.forward);
                }
                if(Input.GetKey(KeyCode.A))
                {
                    YawNode.transform.Translate(-1*cameraSenitiivity * Time.deltaTime * RTSCamera.transform.forward);
                }
                
            }
            //228980
            if(Input.GetKeyDown(KeyCode.J))
            {
                gyroMove=true;
                Cursor.visible=false;
            }
            if(Input.GetKeyUp(KeyCode.J))
            {
                gyroMove=false;
                Cursor.visible=true;
            }
            if(Input.GetKeyDown(KeyCode.LeftShift))
            {
                cameraSenitiivity=200;
            }
            if(Input.GetKeyUp(KeyCode.LeftShift))
            {
                cameraSenitiivity=100;
            }

            if(Input.GetKeyUp(KeyCode.X))
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
            frameDivider++;
            if(frameDivider==20)
            {
                optimizeShipTextureFlag = true;
                frameDivider=0;
            }
            break;
        }
    }
}


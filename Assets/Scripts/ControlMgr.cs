using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMgr : MonoBehaviour
{
    public float deltaV;
    public static ControlMgr inst;
    public GameObject target;
    bool newTarget = false;
    RaycastHit hit;
    void Awake()
    {
        inst = this;
    }

    void start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Period))
        {
            ZoneMgr.inst.PopulateSummon();
        }

        if(Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
            int watermask = 1<<4;
            if(Physics.Raycast(ray, out hit, 6000f, watermask))
            {
                newTarget=true;
            }
        }
        for(int i = 0; i<SelectionMgr.inst.selectedIDs.Count; i++)
        {
            BoatEntity selectedEntity = EntityMgr.inst.boatEntities[SelectionMgr.inst.selectedIDs[i]];
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                selectedEntity.desiredHeading -=deltaV * Time.deltaTime * 5;
            }
            if(Input.GetKey(KeyCode.RightArrow))
            {
                selectedEntity.desiredHeading +=deltaV * Time.deltaTime * 5;
            }
            if(Input.GetKeyUp(KeyCode.UpArrow))
            {
                selectedEntity.desiredSpeed += deltaV;
            }
            if(Input.GetKeyUp(KeyCode.DownArrow))
            {
                selectedEntity.desiredSpeed -= deltaV;
            }
            if(Input.GetKeyUp(KeyCode.Space))
            {
                selectedEntity.desiredSpeed = 0;
            }
            
            if(newTarget && Input.GetKey(KeyCode.LeftControl))
            {
                selectedEntity.playerMove = true;
                selectedEntity.Move(hit.point);
            }
            else if(newTarget)
            {
                selectedEntity.Stop();
                selectedEntity.playerMove = true;
                selectedEntity.Move(hit.point);
            }
            
        }
        newTarget=false;        
    }

    public void CreatePoints(float px, float pz, int radius, Color color)
    {
        float x;
        //float y;
        float z;
        int segments = 10;
        List<Vector3> points = new List<Vector3>();
        float angle = 20f;

        for(int i = 0; i< (segments + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius + px;
            z = Mathf.Cos(Mathf.Deg2Rad * angle) * radius + pz;
            points.Add(new Vector3(x,30,z));
            angle += (360f / segments);
            
        }
        for(int i = 0; i<points.Count-1; i++) 
        {
            Debug.DrawLine(points[i], points[i+1], color);
        }
    }

    
}

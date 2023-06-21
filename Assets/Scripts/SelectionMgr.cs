using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionMgr : MonoBehaviour
{
    public static SelectionMgr inst;
    public int selectedIndex = 0;
    public BoatEntity selectedEntity;
    bool isSelecting = false;
    Vector3 mousePos1;
    public List<int> selectedIDs = new List<int>();
    private void Awake() {
        inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Tab))
        {
            SelectNextEnt();
        }

        //Selecting
        if(Input.GetMouseButtonDown(0))
        {
            UnselectAll();
            isSelecting = true;
            mousePos1 = Input.mousePosition;
        }
        if(Input.GetMouseButtonUp(0))
        {
            selectedIDs.Clear();
            foreach(BoatEntity ent in EntityMgr.inst.boatEntities)
            {
                if(InSelectedZone(ent.position))
                {
                    selectedIDs.Add(ent.ID);
                    ent.isSelected = true;
                }
            }
            mousePos1 = Input.mousePosition;
            isSelecting = false;
        }
    }

    void OnGUI() 
    {
        if(isSelecting)
        {
            Rect rect = Utils.GetScreenRect(mousePos1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    void SelectNextEnt()
    {
        selectedIndex = (selectedIndex >= EntityMgr.inst.boatEntities.Count - 1 ? 0 : selectedIndex + 1);
        selectedEntity = EntityMgr.inst.boatEntities[selectedIndex];
        UnselectAll();
        selectedEntity.isSelected = true;
        selectedIDs.Add(selectedEntity.ID);
    }

    void UnselectAll()
    {
        foreach(BoatEntity phx in EntityMgr.inst.boatEntities)
        {
            phx.isSelected = false;
        }
    }

    bool InSelectedZone(Vector3 position)
    {
        if(!isSelecting)
            return false;
        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds(camera, mousePos1, Input.mousePosition);

        return viewportBounds.Contains(camera.WorldToViewportPoint(position));
    }
}

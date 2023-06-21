using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : MonoBehaviour
{
    public List<BoatEntity> boatEntities;
    // Start is called before the first frame update
    public static EntityMgr inst;
    public int gameSpeed;
    public UnityEngine.UI.Slider mySlider;
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
       
    }

    public int AddBoat(BoatEntity item)
    {
        boatEntities.Add(item);
        return boatEntities.Count-1;
    }
    
    public void SetGameSpeed()
    {
        gameSpeed = (int) mySlider.value;
    }

    
}

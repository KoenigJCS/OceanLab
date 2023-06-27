using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : MonoBehaviour
{
    public List<BoatEntity> boatEntities;
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

    // Update is called once per frame
    void Update()
    {
       
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

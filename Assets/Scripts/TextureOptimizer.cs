using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TextureOptimizer : MonoBehaviour
{
    public GameObject hpTexture;
    public GameObject lpTexture;

    public void CheckToOptimize()
    {
        if((CameraControlls.inst.YawNode.transform.position-gameObject.transform.position).magnitude>=20000f)
        {
            if(hpTexture)
            {
                lpTexture.SetActive(false);
                hpTexture.SetActive(false);
            }
        }
        else if((CameraControlls.inst.YawNode.transform.position-gameObject.transform.position).magnitude>=5000f)
        {
            if(lpTexture)
            {
                lpTexture.SetActive(true);
                hpTexture.SetActive(false);
            }
        }
        else
        {
            if(hpTexture)
            {
                lpTexture.SetActive(false);
                hpTexture.SetActive(true);
            }
        }
    }
}

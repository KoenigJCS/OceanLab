using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMgr : MonoBehaviour
{
    bool menuOpen = false;
    bool micRunning = false;
    public GameObject menu;
    public GameObject micButton;
    public Whisper.Utils.MicrophoneRecord micManager;
    public Color micOn;
    public Color micOff;
    // Start is called before the first frame update
    void Start()
    {
        menu.SetActive(false);
        micButton.GetComponent<UnityEngine.UI.Image>().color = micOff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleMenu()
    {
        if(menuOpen)
            menu.SetActive(false);
        else
            menu.SetActive(true);

        menuOpen = !menuOpen;
    }

    public void ToggleMic()
    {
        if(micRunning)
        {
            micButton.GetComponent<UnityEngine.UI.Image>().color = micOff;
            //micManager.StopRecord();
        }
        else
        {
            micButton.GetComponent<UnityEngine.UI.Image>().color = micOn;
            //micManager.StartRecord();
        }

        micRunning = !micRunning;
    }
}

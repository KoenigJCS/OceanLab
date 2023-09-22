using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TTSMgr : MonoBehaviour
{
    // Start is called before the first frame update
    public Voxell.Speech.TTS.TextToSpeech textToSpeech;
    void Update() 
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            textToSpeech.Speak("Good Job");
        }
    }
}

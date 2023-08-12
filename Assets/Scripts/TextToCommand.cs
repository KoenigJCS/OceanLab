using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class TextToCommand : MonoBehaviour
{
    BoatEntity activeBoat;
    public static TextToCommand inst;
    void Awake()
    {
        inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static string StripPunctuation(string strip)
    {
        var sb = new StringBuilder();

        foreach (char c in strip)
        {
        if (!char.IsPunctuation(c))
            sb.Append(c);
        }

        strip = sb.ToString();
        return strip;
    }

    
    float currentHeading=0;
    float desiredHeading=0;
    float currentSpeed=0;
    float desiredSpeed=0;
    float hardcount=35;
    BoatEntity curBoat;
    
    public void TTC(string input)
    {
        curBoat=SelectionMgr.inst.selectedEntity;
        currentSpeed=curBoat.speed;
        currentHeading=curBoat.heading;
        input=input.ToLower();
        input=StripPunctuation(input);
        input=input.Trim();
        string[] myTokens = input.Split(' ');
        string last, token;
        string debugOut = "";
        bool flag;
        for(int i = 0; i < myTokens.Length; i++)
        {
            long numb = WordsToNumbers.ConvertToNumbers(myTokens[i], out flag);
            if(flag)
            {
                debugOut+=i+": "+numb+"\n";
                myTokens[i]=numb.ToString();
            }
            else
                debugOut+=i+": "+myTokens[i]+"\n";
        }
        Debug.Log(debugOut);
        for(int i = 0; i < myTokens.Length; i++)
        {
            last = myTokens[i];
            // Debug.Log(myTokens[i]);
            // Debug.Log(myTokens[i+1]);
            // Debug.Log(myTokens[i+2]);
            token = last;
            if(token == "right")
            {
                token=myTokens[++i];
                desiredHeading += float.Parse(token);
                
            }
            else if (token == "left")
            {
                token=myTokens[++i];
                desiredHeading -= float.Parse(token);
            }
            else if (token == "hard")
            {
                token=myTokens[++i];
                if(token == "right")
                {
                    desiredHeading+=hardcount;
                }
                else if(token == "left")
                {
                    desiredHeading-=hardcount;
                }
                token=myTokens[++i];
            }
            else if (token == "come")
            {
                token=myTokens[++i];
                if(token == "right")
                {                    
                    token=myTokens[++i];
                    if(token == "to")
                    {                        
                        token=myTokens[++i];
                        desiredHeading=float.Parse(token);
                    }
                }
                else if(token == "left")
                {                    
                    token=myTokens[++i];
                    if(token == "to")
                    {                        
                        token=myTokens[++i];
                        desiredHeading=float.Parse(token);
                    }
                }
            }
            else if(token == "rudder")
            {
                token=myTokens[++i];
                if(token == "amidships" || token == "mid" )
                {
                    desiredHeading=0;
                }
            }
            else if(token == "steady")
            {
                desiredHeading=currentHeading;
            }
            else if(token == "speed")
            {
                token=myTokens[++i];
                desiredSpeed=float.Parse(token);
            }
        }

        Debug.Log("CurrentHeading: "+currentHeading+"\n"
                +"DesiredtHeading: "+desiredHeading+"\n"
                +"CurrentSpeed: "+currentSpeed+"\n"
                +"DesiredSpeed: "+desiredSpeed+"\n");
        
        if(!CameraControlls.inst.isRTSMode)
        {
            curBoat.desiredSpeed=desiredSpeed;
            curBoat.desiredHeading=desiredHeading;
            ACTA.Narrator.speak("Aye Aye Captain!");
        }
        
        // string token;
        // while (tokenOutput.getline(token, INT32_MAX))
        // {

        // }
    }
}

//Taken from iterwebs
class WordsToNumbers  
{  
    private static Dictionary<string, long> numberTable = new Dictionary<string, long>{  
        {"zero",0},{"one",1},{"two",2},{"three",3},{"four",4},{"five",5},{"six",6},  
        {"seven",7},{"eight",8},{"nine",9},{"ten",10},{"eleven",11},{"twelve",12},  
        {"thirteen",13},{"fourteen",14},{"fifteen",15},{"sixteen",16},{"seventeen",17},  
        {"eighteen",18},{"nineteen",19},{"twenty",20},{"thirty",30},{"forty",40},  
        {"fifty",50},{"sixty",60},{"seventy",70},{"eighty",80},{"ninety",90},  
        {"hundred",100},{"thousand",1000},{"lakh",100000},{"million",1000000},  
        {"billion",1000000000},{"trillion",1000000000000},{"quadrillion",1000000000000000},  
        {"quintillion",1000000000000000000}  
    };  
  
    public static long ConvertToNumbers(string numberString, out bool flag)  
    {  
        var numbers = Regex.Matches(numberString, @"\w+").Cast<Match>()  
                .Select(m => m.Value.ToLowerInvariant())  
                .Where(v => numberTable.ContainsKey(v))  
                .Select(v => numberTable[v]);  
        long acc = 0, total = 0L;  

        if(numbers.Count<long>() > 0)
            flag=true;
        else
            flag=false;

        foreach (var n in numbers)  
        {  
            if (n >= 1000)  
            {  
                total += acc * n;  
                acc = 0;  
            }  
            else if (n >= 100)  
            {  
                acc *= n;  
            }  
            else acc += n;  
        }  
        return (total + acc) * (numberString.StartsWith("minus",  
                StringComparison.InvariantCultureIgnoreCase) ? -1 : 1);  
    }  
} 

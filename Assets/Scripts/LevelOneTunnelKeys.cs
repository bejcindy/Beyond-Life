using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelOneTunnelKeys : MonoBehaviour
{
    // Start is called before the first frame update
    public string[] firstGroup = new string[] { "QCEZ", "WXAD" };
    public string[] secondGroup = new string[] { "0K8;", "L9PI" };
    public string[] thirdGroup = new string[] { "IYJ7", "8HK6" };
    public string[] fourthGroup = new string[] { "/IP<", ">O;K"};

    public KeyCode tunnelCurrentKey;
    public int partIndex;
    public int keyIndex;
    public string[] currentGroup;
    public Char currentChar;

    public bool inCheck = false;

    public GameObject tunnelButton;

    void Start()
    {
        partIndex = 0;
        keyIndex = 0;
        getCurrentGroup();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        getCurrentGroup();
        if(partIndex <= 1 && keyIndex < 4)
        {
            KeyCode nkey;
            if (LevelOneKeys.chartoKeycode.TryGetValue(currentGroup[partIndex][keyIndex], out nkey))
            {
                tunnelCurrentKey = nkey;
            }
            currentChar = currentGroup[partIndex][keyIndex];
        }

        if (inCheck)
        {
            if (Input.GetKeyDown(tunnelCurrentKey))
            {
                if(partIndex < 1)
                {
                    if (keyIndex < 3)
                    {
                        keyIndex++;
                    }
                    else
                    {
                        partIndex++;
                        keyIndex = 0;
                    }
                }
                else
                {
                    if(keyIndex < 3)
                    {
                        keyIndex++;
                    }
                    else
                    {
                        inCheck = false;
                        
                    }
                }
            }
        }
        
    }

    void getCurrentGroup()
    {
        switch (LevelOneKeys.groupIndex)
        {
            case 0:
                currentGroup = firstGroup;
                break;
            case 1:
                currentGroup = secondGroup;
                break;
            case 2:
                currentGroup = thirdGroup;
                break;
            case 3:
                currentGroup = fourthGroup;
                break;
            default:
                currentGroup = firstGroup;
                break;
        }
    }

    public void resetIndex()
    {
        partIndex = 0;
        keyIndex = 0;
    }
    

    
}

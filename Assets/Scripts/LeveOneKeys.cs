using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeveOneKeys : MonoBehaviour
{
    public static string codeKeys = "AQWEDCXZ";
    public static string codeKeys1 = "0P;LKI89";
    public static string codeKeys2 = "78IKJHY6";
    public static string codeKeys3 = "KIOP;/><";
    public static string[] codeKeyGroup = new string[] { codeKeys, codeKeys1, codeKeys2, codeKeys3 };

    public static int groupIndex = 0;
    public static int codeIndex = 1;
    public static KeyCode currentKey;

    void Start()
    {
        currentKey = KeyCode.Q;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelOneKeys : MonoBehaviour
{
    public static string codeKeys = "AQWEDCXZ";
    public static string codeKeys1 = "0P;LKI89";
    public static string codeKeys2 = "78IKJHY6";
    public static string codeKeys3 = "KIOP;/><";
    public static string[] codeKeyGroup = new string[] { codeKeys, codeKeys1, codeKeys2, codeKeys3 };

    public static int groupIndex = 0;
    public static int codeIndex = 1;
    public static KeyCode currentKey;
    public static Char currentChar;

    public static bool repeatKeys = false;
    public static bool nextKeys = false;

    void Start()
    {
        currentKey = KeyCode.Q;
    }

    // Update is called once per frame
    void Update()
    {
        //setting keys 
        if (codeIndex < 8)
        {
            KeyCode nkey;
            if(chartoKeycode.TryGetValue(codeKeyGroup[groupIndex][codeIndex], out nkey)){
                Debug.Log(nkey);
                currentKey = nkey;
            }
            currentChar = codeKeyGroup[groupIndex][codeIndex];
        }
        Debug.Log(currentKey);


        //after message input

        if (repeatKeys)
        {
            repeatMessage();
        }
        if (nextKeys)
        {
            Debug.Log("going to next keys");
            nextMessage();
        }

    }



    public void repeatMessage()
    {
        int splitn = UnityEngine.Random.Range(1, 7);
        string[] mgroup = codeKeyGroup[groupIndex].Split(codeKeys[splitn]);
        string output = string.Format(codeKeyGroup[groupIndex][splitn] + "{0}{1}", mgroup[1], mgroup[0]);
        codeKeyGroup[groupIndex] = output;
        codeIndex = 0;
        Debug.Log(codeKeyGroup[groupIndex]);
        repeatKeys = false;
        codeIndex = 0;
    }


    public void nextMessage()
    {
        groupIndex++;

        nextKeys = false;
        codeIndex = 0;
    }

    public static Dictionary<char, KeyCode> chartoKeycode = new Dictionary<char, KeyCode>()
    {
      //-------------------------LOGICAL mappings-------------------------
  
      //Lower Case Letters
      {'A', KeyCode.A},
      {'B', KeyCode.B},
      {'C', KeyCode.C},
      {'D', KeyCode.D},
      {'E', KeyCode.E},
      {'F', KeyCode.F},
      {'G', KeyCode.G},
      {'H', KeyCode.H},
      {'I', KeyCode.I},
      {'J', KeyCode.J},
      {'K', KeyCode.K},
      {'L', KeyCode.L},
      {'M', KeyCode.M},
      {'N', KeyCode.N},
      {'O', KeyCode.O},
      {'P', KeyCode.P},
      {'Q', KeyCode.Q},
      {'R', KeyCode.R},
      {'S', KeyCode.S},
      {'T', KeyCode.T},
      {'U', KeyCode.U},
      {'V', KeyCode.V},
      {'W', KeyCode.W},
      {'X', KeyCode.X},
      {'Y', KeyCode.Y},
      {'Z', KeyCode.Z},
  
      //KeyPad Numbers
      {'1', KeyCode.Keypad1},
      {'2', KeyCode.Keypad2},
      {'3', KeyCode.Keypad3},
      {'4', KeyCode.Keypad4},
      {'5', KeyCode.Keypad5},
      {'6', KeyCode.Alpha6},
      {'7', KeyCode.Alpha7},
      {'8', KeyCode.Alpha8},
      {'9', KeyCode.Alpha9},
      {'0', KeyCode.Alpha0},
  
      //Other Symbols
      {'!', KeyCode.Exclaim}, //1
      {'"', KeyCode.DoubleQuote},
      {'#', KeyCode.Hash}, //3
      {'$', KeyCode.Dollar}, //4
      {'&', KeyCode.Ampersand}, //7
      {'\'', KeyCode.Quote}, //remember the special forward slash rule... this isnt wrong
      {'(', KeyCode.LeftParen}, //9
      {')', KeyCode.RightParen}, //0
      {'*', KeyCode.Asterisk}, //8
      {'+', KeyCode.Plus},
      {',', KeyCode.Comma},
      {'-', KeyCode.Minus},
      {'.', KeyCode.Period},
      {'/', KeyCode.Slash},
      {':', KeyCode.Colon},
      {';', KeyCode.Semicolon},
      {'<', KeyCode.Less},
      {'=', KeyCode.Equals},
      {'>', KeyCode.Greater},
      {'?', KeyCode.Question},
      {'@', KeyCode.At}, //2
      {'[', KeyCode.LeftBracket},
      {'\\', KeyCode.Backslash}, //remember the special forward slash rule... this isnt wrong
      {']', KeyCode.RightBracket},
      {'^', KeyCode.Caret}, //6
      {'_', KeyCode.Underscore},
      {'`', KeyCode.BackQuote},
  
      ////-------------------------NON-LOGICAL mappings-------------------------
  
      ////NOTE: all of these can easily be remapped to something that perhaps you find more useful
  
      ////---Mappings where the logical keycode was taken up by its counter part in either (the regular keybaord) or the (keypad)
  
      ////Alpha Numbers
      ////NOTE: we are using the UPPER CASE LETTERS Q -> P because they are nearest to the Alpha Numbers
      //{'Q', KeyCode.Alpha1},
      //{'W', KeyCode.Alpha2},
      //{'E', KeyCode.Alpha3},
      //{'R', KeyCode.Alpha4},
      //{'T', KeyCode.Alpha5},
      //{'Y', KeyCode.Alpha6},
      //{'U', KeyCode.Alpha7},
      //{'I', KeyCode.Alpha8},
      //{'O', KeyCode.Alpha9},
      //{'P', KeyCode.Alpha0},
  
      ////INACTIVE since I am using these characters else where
      //{'A', KeyCode.KeypadPeriod},
      //{'B', KeyCode.KeypadDivide},
      //{'C', KeyCode.KeypadMultiply},
      //{'D', KeyCode.KeypadMinus},
      //{'F', KeyCode.KeypadPlus},
      //{'G', KeyCode.KeypadEquals},
  
      ////-------------------------CHARACTER KEYS with NO KEYCODE-------------------------
  
      ////NOTE: you can map these to any of the OPEN KEYCODES below
  
      ///*
      ////Upper Case Letters (16)
      //{'H', -},
      //{'J', -},
      //{'K', -},
      //{'L', -},
      //{'M', -},
      //{'N', -},
      //{'S', -},
      //{'V', -},
      //{'X', -},
      //{'Z', -}
      //*/
  
      ////-------------------------KEYCODES with NO CHARACER KEY-------------------------
  
      ////-----KeyCodes without Logical Mappings
      ////-Anything above "KeyCode.Space" in Unity's Documentation (9 KeyCodes)
      ////-Anything between "KeyCode.UpArrow" and "KeyCode.F15" in Unity's Documentation (24 KeyCodes)
      ////-Anything Below "KeyCode.Numlock" in Unity's Documentation [(28 KeyCodes) + (9 * 20 = 180 JoyStickCodes) = 208 KeyCodes]
  
      ////-------------------------other-------------------------

      ////-----KeyCodes that are inaccesible for some reason
      ////{'~', KeyCode.tilde},
      ////{'{', KeyCode.LeftCurlyBrace}, 
      ////{'}', KeyCode.RightCurlyBrace}, 
      ////{'|', KeyCode.Line},   
      ////{'%', KeyCode.percent},
    };
}

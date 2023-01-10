using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelOneSecret : MonoBehaviour
{
    string secret = "SOUL";
    static int enterIndex = 0;

    public GameObject player;
    public Animator currentLight;
    public AudioSource lightAS;
    public AudioClip failedSound;
    public AudioClip messageCorrect;

    public bool messageEntered = false;
    public bool done = false;

    public static bool inputCorrect = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void checkAnswer(string answer)
    //{
    //    if(answer == Char.ToString(secret[LevelOneSecret.enterIndex]))
    //    {
    //        Debug.Log("answer is correct");
    //    }
    //    else
    //    {
    //        Debug.Log("answer is incorrect");
    //    }
    //}

    public void submitAnswer(string answer)
    {
        messageEntered = true;
        
        if (answer == Char.ToString(secret[LevelOneSecret.enterIndex]))
        {
            Debug.Log("answer is correct");
            lightAS.PlayOneShot(messageCorrect);
            LevelOneSecret.enterIndex++;
            done = true;
            inputCorrect = true;
            LevelOneKeys.nextKeys= true;
        }
        else
        {
            LevelOneKeys.codeIndex = 0;
            Debug.Log("answer is incorrect");
            LevelOneKeys.repeatKeys = true;
            StartCoroutine(failedCheck());
            
        }
    }

    IEnumerator failedCheck()
    {
        currentLight.SetBool("wrongMessage", true);
        lightAS.PlayOneShot(failedSound);
        yield return new WaitForSeconds(0.5f);
        currentLight.SetBool("wrongMessage", false);
        currentLight.SetBool("Checked", false);
        done = true;

    }
}

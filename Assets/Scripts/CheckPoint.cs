using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public GameObject player;
    public GameObject checkKey;
    public GameObject[] cpLights;
    public GameObject wakeUpLogic;

    public bool inCheck = false;


    public float pressCD = 3.0f;
    float pressCDVal;
    public float timerSpeed = 0.1f;
    public static int unpressedAmt = 0;


    [SerializeField] Material yellowMat;
    [SerializeField] Material redMat;
    // Start is called before the first frame update
    void Start()
    {
        pressCDVal = pressCD;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(unpressedAmt);
        if (inCheck)
        {
            if (!Input.GetKeyDown(KeyCode.E))
            {

                pressCD -= timerSpeed * Time.deltaTime;
                if(pressCD <=0)
                {
                    inCheck = false;
                    wakeUpLogic.SetActive(false);
                    checkKey.SetActive(false);
                    unpressedAmt += 1;
                    

                    changeLight();


                }
            }
            else
            {
                inCheck = false;
                pressCD = pressCDVal;
                
            }
        }
    }

    void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            //Debug.Log("found player");
            wakeUpLogic.SetActive(true);
            checkKey.SetActive(true);
            inCheck = true;
        }

    }

    void changeLight()
    {
        switch (unpressedAmt)
        {
            case 0: break;
            case 1:
                foreach(GameObject light in cpLights)
                {
                    light.GetComponent<Renderer>().material = yellowMat;
                }
                
                break;
            case 2:
                foreach (GameObject light in cpLights)
                {
                    light.GetComponent<Renderer>().material = redMat;
                }
                GetComponent<BoxCollider>().enabled = false;
                wakeUpLogic.SetActive(true);
                wakeUpLogic.GetComponent<WakeUp>().offTrack = true;
                break;
            default:
                break;
        }
    }
}

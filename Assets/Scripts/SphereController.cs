using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SphereController : MonoBehaviour
{
    float completeTime = 20f;
    float speed;
    float radius;
    float currentAngle;
    float x, z;
    float controledSpeed;
    Vector3 zeroSameY;

    GameObject startPos;
    int hitCounter;
    bool finishedCircle;
    GameObject player;
    bool nearPlayer;

    float minDist = 3f;
    float dist;
    float leftDist, rightDist;
    int multiplier;
    int dirMultiplier;
    float speedPreset;

    Vector3 offset = new Vector3(0, 1, 0);

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (Random.Range(0, 100) < 50)
        {
            dirMultiplier = -1;
        }
        else
        {
            dirMultiplier = 1;
        }
        speedPreset = Random.Range(5f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (nearPlayer)
        {
            //controledSpeed = speedPreset * 20;
            //if (player.transform.childCount < 1)
            //{
                transform.parent = player.transform;
                transform.localPosition = offset;
                controledSpeed = 0;
                
            //}
        }
        else
        {
            controledSpeed = speedPreset;
            transform.RotateAround(Vector3.zero, Vector3.up, dirMultiplier * controledSpeed * Time.deltaTime);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("hit");
        if (other.CompareTag("BelowCheckPoint")) 
        {
            hitCounter++;
            if (!startPos)
            {
                startPos = other.gameObject;
            }
            if (other.gameObject == startPos && hitCounter > 1)
            {
                //move to center
                finishedCircle = true;
            }
        }
        if (other.CompareTag("Core"))
        {
            Die();
        }
        if (other.CompareTag("Player"))
        {
            if (player.transform.childCount == 0)
            {
                nearPlayer = true;
            }
            //nearPlayer = true;
            //Debug.Log("here");
        }
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

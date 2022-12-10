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

    Vector3 offset = new Vector3(0, 2, 0);

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
        //transform.rotation=transform.LookAt
        //dodge player
        if (Vector3.Distance(transform.position, player.transform.position) < minDist||Mathf.Approximately(Vector3.Distance(transform.position, player.transform.position),minDist))
        {
            nearPlayer = true;

            Vector3 direction = (transform.position - player.transform.position).normalized;
            Vector3 directionZeroY = new Vector3(direction.x, 0, direction.y);
            
            transform.position = player.transform.position + direction * minDist;
            Vector3 sameYPos = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);

            transform.RotateAround(sameYPos, multiplier* Vector3.up, controledSpeed * Time.deltaTime);
            
        }
        else
        {
            nearPlayer = false;
        }

        transform.LookAt(Vector3.zero);
        //decide go left or right to player
        //player cann't push the ball off the ground before it goes full circle
        RaycastHit leftH;
        if (Physics.Raycast(transform.position, -transform.forward, out leftH, Mathf.Infinity))
        {
            leftDist = leftH.distance;
        }
        RaycastHit rightH;
        if (Physics.Raycast(transform.position, transform.forward , out rightH, Mathf.Infinity))
        {
            rightDist = rightH.distance;
        }
        if (leftDist < rightDist)
        {
            multiplier = -1;
        }
        else
        {
            multiplier = 1;
        }
        //Debug.Log(leftDist + "left hit "+leftH.collider.gameObject +"; "+ rightDist+ "right hit " + rightH.collider.gameObject);

        if (nearPlayer)
        {
            //controledSpeed = speedPreset * 20;
            if (player.transform.childCount < 1)
            {
                transform.parent = player.transform;
                transform.localPosition = offset;
            }
            
        }
        else
        {
            controledSpeed = speedPreset;
        }

        if (!finishedCircle)
        {
            if (!nearPlayer)
            {
                transform.RotateAround(Vector3.zero, Vector3.up, dirMultiplier * controledSpeed * Time.deltaTime);
            }
        }
        else
        {
            Debug.Log("completed a full circle");
            //raycast down, if no ground below then move y axis; otherwise only move x and z
            RaycastHit h;
            if (Physics.Raycast(transform.position, -Vector3.up, out h, 3))
            {
                zeroSameY = new Vector3(0, transform.position.y, 0);
                transform.position = Vector3.MoveTowards(transform.position, zeroSameY, controledSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, controledSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
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
    }
    IEnumerator Die()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}

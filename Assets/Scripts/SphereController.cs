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

    float minDist = 5f;
    float dist;
    float leftDist, rightDist;
    int multiplier;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

            transform.RotateAround(player.transform.position, multiplier* Vector3.up, controledSpeed * Time.deltaTime);
            
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
            controledSpeed = 50f;
        }
        else
        {
            controledSpeed = 10f;
        }

        if (!finishedCircle)
        {
            if (!nearPlayer)
            {
                transform.RotateAround(Vector3.zero, Vector3.up, controledSpeed * Time.deltaTime);
            }
        }
        else
        {
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
        Debug.Log("hit");
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float t = 5f;
    public Transform player;
    public bool waitForCurve;
    public Transform targetPos;

    public Vector3 offset;

    Vector3 originalOffestY;
    Vector3 originalOffset;
    public float horizontal;
    public float vertical;
    public bool reset;
    public float camSpeed;
    public float rotSpeed;
    public float regularRSpeed;
    public bool jumping;

    public Vector3 previousPos;

    bool stop;
    bool toohigh;

    float verticalMove;

    // Start is called before the first frame update
    void Start()
    {

        originalOffestY = new Vector3(0, offset.y, 0);
        originalOffset = offset;
        if (player)
        {
            transform.position = player.position + offset;
            offset = transform.position - player.transform.position;
        }
        //transform.position = player.position + offset;
        stop = false;
        toohigh = false;
        //offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        verticalMove = Input.mousePosition.y;
        //Vector3 realTarget = player.position + player.transform.up * verticalMove*0.01f;
        Vector3 realTarget = targetPos.position + player.transform.up * verticalMove * 0.01f-player.transform.up*5f;
        Debug.Log(realTarget);
        if (!player)
        {
            return;
        }
        if (!waitForCurve)
        {
            //transform.position = Vector3.Lerp(transform.position, targetPos.position, Time.deltaTime * camSpeed);
            transform.position = Vector3.Lerp(transform.position, realTarget, Time.deltaTime * camSpeed);
            //transform.rotation = targetPos.rotation;
            //transform.LookAt(realTarget);
            transform.LookAt(player);

        }
        else
        {
            transform.LookAt(realTarget);
        }
    }
}

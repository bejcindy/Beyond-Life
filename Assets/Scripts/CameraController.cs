using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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

    Vector3 camLookOffset;
    Camera cam;

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
        camLookOffset = new Vector3(0, 1, 0);
        cam = GetComponent<Camera>();
        cam.fieldOfView = 50;
        //offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //verticalMove = Mathf.Clamp(-(Input.mousePosition.y - Screen.height / 2), -300, 300);

        //Vector3 realTarget = player.position + player.transform.up * verticalMove*0.01f;
        //Vector3 realTarget = targetPos.position + player.transform.up * verticalMove * 0.01f;
        //Vector3 realTarget = targetPos.position;
        //Debug.Log(realTarget);
        //Debug.Log(verticalMove);

        //if (!waitForCurve)
        //{
        //transform.position = Vector3.Lerp(transform.position, targetPos.position, Time.deltaTime * camSpeed);

        verticalMove = Mathf.Clamp(-(Input.mousePosition.y - Screen.height *.75f), -15, 300);
        transform.localPosition = new Vector3(0, 1f+verticalMove * 0.01f, -5- verticalMove * 0.03f);

        //transform.position = Vector3.Lerp(transform.position, realTarget, Time.deltaTime * camSpeed);
        //Debug.Log(realTarget);
        //transform.rotation = targetPos.rotation;
        //transform.LookAt(realTarget);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.position), .1f * Time.deltaTime);



        transform.LookAt(player.position+camLookOffset,player.transform.up);

        //}
        //else
        //{
        //    //transform.LookAt(realTarget);
        //    transform.LookAt(player);
        //}
    }
}

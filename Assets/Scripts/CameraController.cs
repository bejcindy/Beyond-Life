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

    public bool wakeUp;

    //bool wokeUp;

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

    public float camUpperLimit = 500f;
    public float camLowerLimit = -15f;

    bool stop;
    bool toohigh;

    float verticalMove;
    float timer;
    Vector3 moveTarget;

    Vector3 camLookOffset;
    Camera cam;

    float camMoveSpeed = 20f;
    float camRotSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {

        originalOffestY = new Vector3(0, offset.y, 0);
        originalOffset = offset;
        //if (player)
        //{
        //    transform.position = player.position + offset;
        //    offset = transform.position - player.transform.position;
        //}
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
        
        verticalMove = Mathf.Clamp(-(Input.mousePosition.y - Screen.height *.75f), camLowerLimit, camUpperLimit);

        moveTarget = new Vector3(0, 1f + verticalMove * 0.01f, Mathf.Clamp( (-5 - verticalMove * 0.03f),-15,-3));
        //moveTarget = new Vector3(0, 1f + verticalMove * 0.01f, -5 - verticalMove * 0.03f);
        //Debug.Log(-5 - verticalMove * 0.03f);

        //Debug.Log(transform.localPosition);

        //if (wakeUp)
        //{
        //    if (timer < 3)
        //    {
        //        timer += Time.deltaTime;
        transform.localPosition = Vector3.Lerp(transform.localPosition, moveTarget, Time.deltaTime * camMoveSpeed);
                Vector3 relativePos = (player.position + camLookOffset) - transform.position;
                Quaternion toRotation = Quaternion.LookRotation(relativePos);
                //transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, 10f * Time.deltaTime);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, camRotSpeed * Time.deltaTime);
                
        //    }
        //    else
        //    {
        //        timer = 3;
        //        transform.localPosition = new Vector3(0, 1f + verticalMove * 0.01f, -5 - verticalMove * 0.03f);
        //        //transform.localPosition = Vector3.Lerp(transform.localPosition, moveTarget, Time.deltaTime * 5f);
        //        transform.LookAt(player.position + camLookOffset, player.transform.up);
        //    }
        //    //transform.localPosition = Vector3.Lerp(transform.localPosition, moveTarget, Time.deltaTime*5f);
        //    //transform.localPosition = new Vector3(0, 1f + verticalMove * 0.01f, -5 - verticalMove * 0.03f);
        //}
        //timer += Time.deltaTime;
        //transform.localPosition = Vector3.Lerp(transform.localPosition, moveTarget, Time.deltaTime * 10f);
        //Vector3 relativePos = (player.position + camLookOffset) - transform.position;
        //Quaternion toRotation = Quaternion.LookRotation(relativePos);
        //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        //transform.localPosition = new Vector3(0, 1f+verticalMove * 0.01f, -5- verticalMove * 0.03f);


        //transform.LookAt(player.position+camLookOffset,player.transform.up);

    }
}

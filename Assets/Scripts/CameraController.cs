using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public Transform targetPos;
    public Vector3 offset;
    public bool wakeUp;
    public float camUpperLimit = 500f;
    public float camLowerLimit = -15f;
    public bool Level1End;

    float verticalMove;
    Vector3 moveTarget;
    public Vector3 camLookOffset = new Vector3(0, .5f, 0);
    Camera cam;
    float camMoveSpeed = 100f;
    float camRotSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        //camLookOffset = new Vector3(0, 1, 0);
        cam = GetComponent<Camera>();
        cam.fieldOfView = 50;
    }

    // Update is called once per frame
    void Update()
    {
        
        verticalMove = Mathf.Clamp(-(Input.mousePosition.y - Screen.height *.75f), camLowerLimit, camUpperLimit);

        moveTarget = new Vector3(0, 1f + verticalMove * 0.01f, Mathf.Clamp( (-5 - verticalMove * 0.03f),-15,-3));

        if (!Level1End)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, moveTarget, Time.deltaTime * camMoveSpeed);
            Vector3 relativePos = (player.position + camLookOffset) - transform.position;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, camRotSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 topOfPlayer = new Vector3(0, 1.5f, 0);
            transform.localPosition = Vector3.Lerp(transform.localPosition, topOfPlayer, Time.deltaTime);
            
        }
    }
}

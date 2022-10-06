using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed;
    float x, z;
    float rotationX;
    public float lookSpeed = 5.0f;
    public float lookXLimit = 45.0f;

    Rigidbody rb;

    bool parallelToGround;
    Vector3 currentDirection;
    Vector3 zeroX, zeroY, zeroZ;
    bool zX, zY, zZ;
    bool activeAxisX;
    float rot;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        parallelToGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");
        Vector3 direction = (transform.forward * z + transform.right * x).normalized;
        zeroY = new Vector3(direction.x, 0, direction.z);
        zeroZ = new Vector3(direction.x, direction.y, 0);
        zeroX= new Vector3(0, direction.y, direction.z);

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);

        if (zX)
        {
            currentDirection = zeroX;
            //Debug.Log("x");
        }
        if (zY)
        {
            currentDirection = zeroY;
            //Debug.Log("y");
        }
        if (zZ)
        {
            currentDirection = zeroZ;
            //Debug.Log("z");
        }

        rb.velocity = currentDirection * speed;

        //Debug.Log(currentDirection);

        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Walkable"))
        {
            //改一下这个
            //可能要手动根据collision.gameobject.transform.up来给player+-90°
            transform.up = collision.gameObject.transform.up;
            //transform.rotation *= collision.gameObject.transform.rotation;
            Physics.gravity = collision.gameObject.transform.up * -9.81f;
            if (Mathf.Abs( collision.gameObject.transform.up.y)!=0)
            {
                zX = false;
                zY = true;
                zZ = false;
                
                activeAxisX = false;
                Debug.Log("yhere");
            }
            if(Mathf.Abs(collision.gameObject.transform.up.x) !=0)
            {
                zX = true;
                zY = false;
                zZ = false;
                activeAxisX = true;
                
                Debug.Log("xhere");
            }
            if(Mathf.Abs(collision.gameObject.transform.up.z) !=0)
            {
                zX = false;
                zY = false;
                zZ = true;
                activeAxisX = true;
                
                Debug.Log("zhere");
            }
            //Debug.Log("hit"+ collision.gameObject.transform.up);
        }
    }
}

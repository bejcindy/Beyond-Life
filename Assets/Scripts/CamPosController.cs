using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPosController : MonoBehaviour
{
    float verticalMove;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        verticalMove = Mathf.Clamp(-(Input.mousePosition.y - Screen.height / 2), -300, 300);
        transform.localPosition = new Vector3(0, verticalMove * 0.01f, -5);
    }
}

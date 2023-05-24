using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float speed;
    public Vector2 turn;
    public float cameraSensitivity;
    public Vector3 deltaMove;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
    }
    // Update is called once per frame
    void Update()
    {
        //keyboard input
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-speed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, -speed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));
        }

        //mouse input
        turn.x += Input.GetAxis("Mouse X") * cameraSensitivity;
        turn.y += Input.GetAxis("Mouse Y") * cameraSensitivity;
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}

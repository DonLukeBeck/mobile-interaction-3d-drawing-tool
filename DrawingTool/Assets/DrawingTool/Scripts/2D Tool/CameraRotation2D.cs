using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation2D : MonoBehaviour
{
    public Transform target; // The object to rotate around
    public float rotationSpeed = 1f;

    private Vector3 offset;

    private void Start()
    {
        // Calculate the initial offset between the camera and the target
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.RotateAround(target.position, Vector3.forward, rotationAmount);
            offset = Quaternion.Euler(0f, 0f, rotationAmount) * offset;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.RotateAround(target.position, Vector3.back, rotationAmount);
            offset = Quaternion.Euler(0f, 0f, -rotationAmount) * offset;
        }

        if (Input.GetKey(KeyCode.W))
        {
            transform.RotateAround(target.position, Vector3.right, rotationAmount);
            offset = Quaternion.Euler(rotationAmount, 0f, 0f) * offset;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.RotateAround(target.position, Vector3.left, rotationAmount);
            offset = Quaternion.Euler(-rotationAmount, 0f, 0f) * offset;
        }

        transform.position = target.position + offset;

        transform.LookAt(target);
    }
}

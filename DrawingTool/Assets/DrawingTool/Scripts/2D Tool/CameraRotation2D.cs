using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation2D : MonoBehaviour{
    public Transform target; // The object to rotate around
    public float rotationSpeed = 0;

    private Vector3 offset;
    private bool isDragging = false;
    private Vector3 lastMousePosition;

    private void Start()
    {
        // Calculate the initial offset between the camera and the target
        offset = transform.position - target.position;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isDragging = true;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
            float mouseX = mouseDelta.x * rotationSpeed;
            float mouseY = mouseDelta.y * rotationSpeed;

            Quaternion rotation = Quaternion.Euler(-mouseY, mouseX, 0f);
            offset = rotation * offset;

            lastMousePosition = Input.mousePosition;
        }

        transform.position = target.position + offset;

        transform.LookAt(target);
    }
}

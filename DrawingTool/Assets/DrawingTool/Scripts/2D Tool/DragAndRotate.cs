using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndRotate : MonoBehaviour
{
    private Vector3 mouseStartPosition;
    private Vector3 objectStartPosition;

    private void OnMouseDown()
    {
        mouseStartPosition = Input.mousePosition;
        objectStartPosition = transform.eulerAngles;
    }

    private void OnMouseDrag()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseOffset = currentMousePosition - mouseStartPosition;

        float rotationAmount = mouseOffset.x * 0.2f; // Adjust rotation speed as needed

        Vector3 newRotation = objectStartPosition + new Vector3(0f, rotationAmount, 0f);
        transform.eulerAngles = newRotation;
    }
}

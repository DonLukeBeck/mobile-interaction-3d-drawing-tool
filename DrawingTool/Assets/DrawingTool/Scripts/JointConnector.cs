using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointConnector : MonoBehaviour
{
    LineRenderer lineRenderer;

    public Transform origin;
    public Transform destination;

    void Start()
    {
        SetWidth(0.1f);
    }

    void Update()
    {
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, destination.position);
    }

    public void SetWidth(float width)
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Liner : MonoBehaviour
{
    LineRenderer lr;
    Color[] colors = { Color.red, Color.black };
    int curColor = 0;
    void Start()
    {
        lr = this.gameObject.AddComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.material.color = colors[curColor];
        lr.startWidth = 1f;
        lr.endWidth = 1f;
        lr.positionCount = 2;

        Vector3[] poses = new Vector3[2];
        poses[0] = new Vector3(0, 10f, 100f);
        poses[1] = new Vector3(5f, -3f, -4f);
        lr.SetPositions(poses);

        lr.useWorldSpace = false;

        MeshCollider meshCollider = this.gameObject.AddComponent<MeshCollider>();
        Mesh mesh = new Mesh();
        lr.BakeMesh(mesh, Camera.main, false);
        meshCollider.sharedMesh = mesh;


    }
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hitData_for_the_ray;
            if (Physics.Raycast(ray, out hitData_for_the_ray))
            {
                GameObject theGameObjectHitByRay = hitData_for_the_ray.collider.gameObject;
                {
                    if (theGameObjectHitByRay == this.gameObject)
                    {
                        curColor = (curColor + 1) % 2;
                        lr.material.color = colors[curColor];
                    }
                }
            }
        }
    }
}
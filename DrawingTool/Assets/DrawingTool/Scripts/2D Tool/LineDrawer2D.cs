using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer2D : MonoBehaviour
{
    List<Vector3> linePoints;
    float timer;
    public float timeDelay;

    GameObject newLine;
    LineRenderer drawLine;
    public float lineWidth;
    GestureController gestureController;
    DrawingTool drawingTool;
    // Start is called before the first frame update
    void Start()
    {
        linePoints = new List<Vector3>();
        timer = timeDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))

        {
            newLine = new GameObject();
            newLine.transform.parent = this.transform;
            drawLine = newLine.AddComponent<LineRenderer>();

            drawLine.material = new Material(Shader.Find("Sprites/Default"));
            drawLine.startWidth = lineWidth;
            drawLine.endWidth = lineWidth;
            drawLine.startColor = Color.black;
            drawLine.endColor = Color.black;
        }

        if (Input.GetMouseButton(1)) { 

            Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), GetMousePosition(), Color.red); 
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                linePoints.Add(GetMousePosition());
                drawLine.positionCount = linePoints.Count;
                drawLine.SetPositions(linePoints.ToArray());
                timer = timeDelay;
            }
        }

        if(Input.GetMouseButtonUp(1))
        {
            foreach(Vector3 point in linePoints)
            {
                Debug.Log(point);
            }
            GenerateMeshCollider();
            linePoints.Clear();
        }

    }
        
    Vector3 GetMousePosition() 
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 10; 
    }

    public void GenerateMeshCollider()
    {
        drawLine.useWorldSpace = false;
        MeshCollider collider = GetComponent<MeshCollider>();

        if (collider == null)
        {
            collider = gameObject.AddComponent<MeshCollider>();
        }

        Mesh mesh = new Mesh();
        drawLine.BakeMesh(mesh, true);
        collider.sharedMesh = mesh;
    }
}

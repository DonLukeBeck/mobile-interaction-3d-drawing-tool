using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
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
        gestureController = GameObject.Find("DrawingManager").GetComponent<GestureController>();
        drawingTool = GameObject.Find("DrawingManager").GetComponent<DrawingTool>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetMouseButtonDown(0) || (gestureController.gesture == "fist" && drawingTool.gestureControlled))
        {
            newLine = new GameObject();
            drawLine = newLine.AddComponent<LineRenderer>();
            drawLine.material = new Material(Shader.Find("Sprites/Default"));
            drawLine.startWidth = lineWidth;
            drawLine.endWidth = lineWidth;
            drawLine.startColor = Color.black;
            drawLine.endColor = Color.black;

        }


        if (Input.GetMouseButton(0) || (gestureController.gesture == "fist" && drawingTool.gestureControlled)) { 
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

        if(Input.GetMouseButtonUp(0) && (gestureController.gesture != "fist" && drawingTool.gestureControlled))
        {
            linePoints.Clear();
        }
    }

    Vector3 GetMousePosition() 
    {
        if (drawingTool.gestureControlled)
        {
            Debug.Log(Camera.main.ScreenToWorldPoint(gestureController.averagePosition));
            return Camera.main.ScreenToWorldPoint(new Vector3(gestureController.averagePosition.x, gestureController.averagePosition.y, -gestureController.averagePosition.z));
        } else { 
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return ray.origin + ray.direction * 10;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class LineDrawerGesture : MonoBehaviour
{
    private List<Vector3> _linePoints;
    private float _timer;
    public float timeDelay;

    GameObject _newLine;
    LineRenderer _drawLine;
    public float LineWidth;
    GestureController _gestureController;
    DrawingTool _drawingTool;

    // Start is called before the first frame update
    void Start()
    {
        _linePoints = new List<Vector3>();
        _timer = timeDelay;
        _gestureController = GameObject.Find("Manager").GetComponent<GestureController>();
        _drawingTool = GameObject.Find("Manager").GetComponent<DrawingTool>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (_gestureController.gesture == GestureController.Gestures.Fist && _drawingTool.GestureControlled))
        {
            _newLine = new GameObject();
            _newLine.transform.parent = this.transform;
            _drawLine = _newLine.AddComponent<LineRenderer>();

            _drawLine.material = new Material(Shader.Find("Sprites/Default"));
            _drawLine.startWidth = LineWidth;
            _drawLine.endWidth = LineWidth;
            _drawLine.startColor = Color.black;
            _drawLine.endColor = Color.black;
        }

        if (Input.GetMouseButton(0) || (_gestureController.gesture == GestureController.Gestures.Fist && _drawingTool.GestureControlled))
        {
            Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), GetMousePosition(), Color.red);
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _linePoints.Add(GetMousePosition());
                _drawLine.positionCount = _linePoints.Count;
                _drawLine.SetPositions(_linePoints.ToArray());
                _timer = timeDelay;
            }
        }

        if (Input.GetMouseButtonUp(0) || (_gestureController.gesture != GestureController.Gestures.Fist && _drawingTool.GestureControlled))
        {
            foreach (Vector3 point in _linePoints)
            {
                Debug.Log(point);
            }

            GenerateMeshCollider();
            _linePoints.Clear();
        }
    }

    Vector3 GetMousePosition()
    {
        if (_drawingTool.GestureControlled)
            return _drawingTool.HandPositions[0];
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * 10;
    }

    public void GenerateMeshCollider()
    {
        if (_drawLine == null) return;
        
        _drawLine.useWorldSpace = false;
        MeshCollider collider = GetComponent<MeshCollider>();

        if (collider == null)
        {
            collider = gameObject.AddComponent<MeshCollider>();
        }

        Mesh mesh = new Mesh();
        _drawLine.BakeMesh(mesh, true);
        collider.sharedMesh = mesh;
    }
}
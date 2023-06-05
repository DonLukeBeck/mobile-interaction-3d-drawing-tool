using System.Collections.Generic;
using UnityEngine;

public class BrushController : MonoBehaviour
{
    [SerializeField] private Transform _pointer;
    [SerializeField] private HandController _handController;
    [SerializeField] private GestureController _gestureController;
    [SerializeField] private GestureController.Gesture _drawGesture = GestureController.Gesture.Pointing;
    public float TimeDelay;
    public float LineWidth;

    private List<Vector3> _linePoints;
    private float _timer;
    private GameObject _newLine;
    private LineRenderer _drawLine;

    void Start()
    {
        _linePoints = new List<Vector3>();
        _timer = TimeDelay;
        if (_gestureController == null)
            _gestureController = GameObject.Find("Manager").GetComponent<GestureController>();
        if (_handController == null)
            _handController = GameObject.Find("Manager").GetComponent<HandController>();
        _pointer = GameObject.Find("Pointer").transform;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) ||
            (_gestureController.gesture == _drawGesture && _handController.GestureControlled))
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

        if (Input.GetMouseButton(0) ||
            (_gestureController.gesture == _drawGesture && _handController.GestureControlled))
        {
            Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), GetPointerPosition(), Color.red);
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _linePoints.Add(GetPointerPosition());
                _drawLine.positionCount = _linePoints.Count;
                _drawLine.SetPositions(_linePoints.ToArray());
                _timer = TimeDelay;
            }
        }

        if (Input.GetMouseButtonUp(0) ||
            (_gestureController.gesture != _drawGesture && _handController.GestureControlled))
        {
            foreach (Vector3 point in _linePoints)
            {
                //Debug.Log(point);
            }

            GenerateMeshCollider();
            _linePoints.Clear();
        }
    }

    Vector3 GetPointerPosition()
    {
        if (_handController.GestureControlled)
            return _pointer.position;

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
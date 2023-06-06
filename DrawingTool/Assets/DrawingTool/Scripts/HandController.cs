using System.Collections.Generic;
using Google.Protobuf.Collections;
using Mediapipe;
using UnityEngine;
using static Unity.Mathematics.math;

public class HandController : HandLandmarkUser
{
    [Header("General")] 
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _pointer;
    public bool GestureControlled = true;
    public int GestureIdx = 8;
    [SerializeField] private float SampleAlpha = 0.5f;
    public float[] PalmSize { get; private set; }
    [SerializeField] private float MouseDepth = 50;
    
    [Header("Depth")]
    public Vector2 StartRange = new (0, 1);
    public Vector2 EndRange = new (0, 10);
    public float DepthSampleAlpha = 0.5f;
    
    public Vector3[] HandPositions { get; private set; }
    private Vector3[] _prevHandPositions;
    private Vector3[] _newHandPositions;
    
    public float[] HandDepths { get; private set; }
    private float[] _newHandDepths;

    private bool _untracked = true;
    
    void Start()
    {
        if(_camera == null) _camera = GameObject.Find("MainCamera").GetComponent<Camera>();
        if(_pointer == null) _pointer = GameObject.Find("Pointer").transform;

        int maxNumHands = DrawingSettings.Instance.MaxNumHands;
        PalmSize = new float[maxNumHands];
        
        HandPositions = new Vector3[maxNumHands];
        _prevHandPositions = new Vector3[maxNumHands];
        _newHandPositions = new Vector3[maxNumHands];
        
        HandDepths = new float[maxNumHands];
        _newHandDepths = new float[maxNumHands];
        
        for (int i = 0; i < HandPositions.Length; i++)
        {
            HandPositions[i] = _pointer.position;
        }
    }

    private void Update()
    {
        for (int i = 0; i < HandPositions.Length; i++)
        {
            _prevHandPositions[i] = HandPositions[i];
            
            HandPositions[i].LowPassFilter(_newHandPositions[i], SampleAlpha);
            HandDepths[i].LowPassFilter(_newHandDepths[i], DepthSampleAlpha);
        }

        if (GestureControlled)
        {
            float u = HandPositions[0].x;
            float v = HandPositions[0].y;

            Vector3 p = _camera.ViewportToWorldPoint(new Vector3(u, v, HandDepths[0]));
            p.y *= -1;
            _pointer.position = p;
        }
        else
        {
            float u = Input.mousePosition.x / 1920;
            float v = Input.mousePosition.y / 1080;
            _pointer.position = _camera.ViewportToWorldPoint(new Vector3(u, v, MouseDepth));
        }
    }

    public override void ProcessHandLandmark(IList<NormalizedLandmarkList> handLandmarkLists,
        IList<ClassificationList> handedness = null)
    {
        if (handLandmarkLists == null) return;

        for (int i = 0; i < HandPositions.Length; i++)
        {
            var landmark = handLandmarkLists[i].Landmark;
            _newHandPositions[i].x = landmark[8].X;
            _newHandPositions[i].y = landmark[8].Y;
            
            PalmSize[i] = GetPalmSize(landmark);
            _newHandDepths[i] = remap(StartRange.x, StartRange.y, EndRange.x, EndRange.y, PalmSize[i]);
        }

        if (_untracked)
        {
            HandPositions[0] = _newHandPositions[0];
            _untracked = false;
        }
    }

    private readonly int[] _palmIdxs = { 0, 5, 17 };
    private float GetPalmSize(RepeatedField<NormalizedLandmark> landmark)
    {
        Vector3 _bbMin = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        Vector3 _bbMax = new Vector3(int.MinValue, int.MinValue, int.MinValue);

        foreach (var idx in _palmIdxs)
        {
            float x = landmark[idx].X;
            float y = landmark[idx].Y * -1;
            float z = landmark[idx].Z;

            if (_bbMin.x > x) _bbMin.x = x;
            if (_bbMax.x < x) _bbMax.x = x;
            if (_bbMin.y > y) _bbMin.y = y;
            if (_bbMax.y < y) _bbMax.y = y;
            if (_bbMin.z > z) _bbMin.z = z;
            if (_bbMax.z < z) _bbMax.z = z;
        }

        return Vector3.Distance(_bbMin, _bbMax);
    }
}
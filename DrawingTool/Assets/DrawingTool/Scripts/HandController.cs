using System;
using System.Collections.Generic;
using System.Drawing;
using Google.Protobuf.Collections;
using Mediapipe;
using UnityEngine;
using UnityEngine.Serialization;
using static Unity.Mathematics.math;

public class HandController : HandLandmarkUser
{
    [Header("General")] 
    [SerializeField] private GameObject _camera;
    [SerializeField] private Transform _pointer;
    public bool GestureControlled = true;
    public bool RenderHandSkeleton = true;
    public float[] PalmSize { get; private set; }
    
    [Header("Position Based")]
    public float HandScale = 10;
    public float MoveScale = 10;
    public float SampleAlpha = 0.5f;
    
    [Header("Velocity Based")]
    public bool UseVelocity = false;
    public float VelocityScale = 1;
    public float VelocitySampleAlpha = 0.5f;

    [Header("Depth")]
    public Vector2 StartRange = new Vector2(0, 1);
    public Vector2 EndRange = new Vector2(0, 10);
    public float DepthSampleAlpha = 0.5f;
    
    private HandSkeleton[] _handSkeletons;
    public Vector3[] HandPositions { get; private set; }
    private Vector3[] _prevHandPositions;
    private Vector3[] _newHandPositions;
    
    public float[] HandDepths { get; private set; }
    private float[] _newHandDepths;

    private bool _untracked = true;
    
    void Start()
    {
        if(_camera == null) _camera = GameObject.Find("MainCamera");
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

        _handSkeletons = new HandSkeleton[DrawingSettings.Instance.MaxNumHands];
        for (int i = 0; i < _handSkeletons.Length; i++)
        {
            _handSkeletons[i] = new HandSkeleton(HandScale * .2f, HandScale);
            _handSkeletons[i].IsActive = false;
        }
    }

    private void Update()
    {
        for (int i = 0; i < _handSkeletons.Length; i++)
        {
            _prevHandPositions[i] = HandPositions[i];
            
            HandPositions[i].LowPassFilter(_newHandPositions[i], UseVelocity ? VelocitySampleAlpha : SampleAlpha);
            HandDepths[i].LowPassFilter(_newHandDepths[i], DepthSampleAlpha);

            HandPositions[i].z = HandDepths[i];
        }

        if (UseVelocity)
        {
            Vector3 velocity = (HandPositions[0] - _prevHandPositions[0]) / Time.deltaTime;
            _pointer.position += velocity * VelocityScale;
        }
        else
        {
            _pointer.position = HandPositions[0] * MoveScale;
        }
    }

    public override void ProcessHandLandmark(IList<NormalizedLandmarkList> handLandmarkLists,
        IList<ClassificationList> handedness = null)
    {
        if (handLandmarkLists == null) return;

        for (int i = 0; i < _handSkeletons.Length; i++)
        {
            if (i > handLandmarkLists.Count - 1)
            {
                _handSkeletons[i].IsActive = false;
                continue;
            }

            if (RenderHandSkeleton)
                _handSkeletons[i].IsActive = true;

            var landmark = handLandmarkLists[i].Landmark;
            GetPalmProperties(landmark, out PalmSize[i], out _newHandPositions[i]);
            _newHandDepths[i] = remap(StartRange.x, StartRange.y, EndRange.x, EndRange.y, PalmSize[i]);

            if (!RenderHandSkeleton) continue;
            for (int o = 0; o < landmark.Count; o++)
            {
                float x = landmark[o].X * MoveScale;
                float y = landmark[o].Y * MoveScale * -1;
                float z = landmark[o].Z * MoveScale + HandDepths[i];

                _handSkeletons[i].Joints[o].transform.position = new Vector3(x, y, z);
            }
        }

        if (_untracked)
        {
            HandPositions[0] = _newHandPositions[0];
            _camera.transform.position = new Vector3(HandPositions[0].x, HandPositions[0].y + 3, _camera.transform.position.z);
            _untracked = false;
        }
        
    }

    private readonly int[] _palmIdxs = { 0, 5, 17 };
    private void GetPalmProperties(RepeatedField<NormalizedLandmark> landmark, out float size,
        out Vector3 averagePosition)
    {
        Vector3 _bbMin = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        Vector3 _bbMax = new Vector3(int.MinValue, int.MinValue, int.MinValue);

        averagePosition = new Vector3();
        foreach (var idx in _palmIdxs)
        {
            float x = landmark[idx].X;
            float y = landmark[idx].Y * -1;
            float z = landmark[idx].Z;

            averagePosition.x += x;
            averagePosition.y += y;
            averagePosition.z += z;

            if (_bbMin.x > x) _bbMin.x = x;
            if (_bbMax.x < x) _bbMax.x = x;
            if (_bbMin.y > y) _bbMin.y = y;
            if (_bbMax.y < y) _bbMax.y = y;
            if (_bbMin.z > z) _bbMin.z = z;
            if (_bbMax.z < z) _bbMax.z = z;
        }

        size = Vector3.Distance(_bbMin, _bbMax);
        averagePosition /= _palmIdxs.Length;
    }
}
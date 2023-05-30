using System;
using System.Collections.Generic;
using System.Drawing;
using Google.Protobuf.Collections;
using Mediapipe;
using UnityEngine;
using static Unity.Mathematics.math;

public class DrawingTool : HandLandmarkUser
{
    public float HandScale = 10;
    public float MoveScale = 100;
    public bool GestureControlled = true;
    public bool RenderHandSkeleton = true;
    [SerializeField] private GameObject _pointer;

    public Vector3[] AveragePosition { get; private set; }
    private HandSkeleton[] _handSkeletons;

    [SerializeField] private int _numSamples = 4;
    private float[,] _positionSamples;

    void Start()
    {
        _pointer = GameObject.Find("Pointer");
        AveragePosition = new Vector3[DrawingSettings.Instance.MaxNumHands];
        for (int i = 0; i < AveragePosition.Length; i++)
            AveragePosition[i] = new Vector3();

        _handSkeletons = new HandSkeleton[DrawingSettings.Instance.MaxNumHands];
        for (int i = 0; i < _handSkeletons.Length; i++)
        {
            _handSkeletons[i] = new HandSkeleton(HandScale * .2f, HandScale);
            _handSkeletons[i].IsActive = false;
        }

        _positionSamples = new float[DrawingSettings.Instance.MaxNumHands, _numSamples];
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
            ref var averagePos = ref AveragePosition[i];
            GetPalmProperties(landmark, out float size, out averagePos);
            float depth = remap(0, 1, MoveScale, -MoveScale, size);

            averagePos *= MoveScale;
            averagePos.z += depth;
            
            if(!RenderHandSkeleton) continue;

            for (int o = 0; o < landmark.Count; o++)
            {
                float x = landmark[o].X * MoveScale;
                float y = landmark[o].Y * MoveScale * -1;
                float z = landmark[o].Z * MoveScale + depth;

                _handSkeletons[i].Joints[o].transform.position = new Vector3(x, y, z);
            }
        }

        _pointer.transform.position = AveragePosition[0];
    }

    private readonly int[] _palmIdx = new int[] { 0, 5, 17 };
    private void GetPalmProperties(RepeatedField<NormalizedLandmark> landmark, out float size,
        out Vector3 averagePosition)
    {
        Vector3 _bbMin = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);
        Vector3 _bbMax = new Vector3(int.MinValue, int.MinValue, int.MinValue);

        averagePosition = new Vector3();
        foreach (var idx in _palmIdx)
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
        averagePosition /= _palmIdx.Length;
    }

    private void LowPassFilter()
    {
        
    }
}
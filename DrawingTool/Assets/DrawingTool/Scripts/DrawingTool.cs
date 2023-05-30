using System.Collections.Generic;
using Google.Protobuf.Collections;
using Mediapipe;
using UnityEngine;
using static Unity.Mathematics.math;

public class DrawingTool : HandLandmarkUser
{
    public float HandScale = 10;
    public float MoveScale = 100;
    public bool GestureControlled;

    public Vector3[] AveragePosition { get; private set; }
    private HandSkeleton[] _handSkeletons;

    void Start()
    {
        AveragePosition = new Vector3[DrawingSettings.Instance.MaxNumHands];
        for (int i = 0; i < AveragePosition.Length; i++)
            AveragePosition[i] = new Vector3();

        _handSkeletons = new HandSkeleton[DrawingSettings.Instance.MaxNumHands];
        for (int i = 0; i < _handSkeletons.Length; i++)
            _handSkeletons[i] = new HandSkeleton(HandScale * .2f, HandScale * .2f);
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
            _handSkeletons[i].IsActive = true;

            var landmark = handLandmarkLists[i].Landmark;
            float depth = remap(0, 1, MoveScale, -MoveScale, GetPalmSize(landmark));
            Debug.Log("Depth " + depth);
            
            AveragePosition[i] = new Vector3(0, 0, 0);
            for (int o = 0; o < landmark.Count; o++)
            {
                float x = landmark[o].X * MoveScale;
                float y = landmark[o].Y * MoveScale * -1;
                float z = landmark[o].Z * MoveScale + depth;

                _handSkeletons[i].Joints[o].transform.localPosition = new Vector3(x, y, z);
                AveragePosition[i] += new Vector3(x, y, z);
            }
            AveragePosition[i] /= landmark.Count;
        }
    }

    private readonly int[] _palmIdx = new int[] { 0, 5, 17 };
    private float GetPalmSize(RepeatedField<NormalizedLandmark> landmark)
    {
        Vector3 _bbMin = new Vector3(int.MinValue, int.MinValue, int.MinValue);
        Vector3 _bbMax = new Vector3(int.MaxValue, int.MaxValue, int.MaxValue);

        foreach (var idx in _palmIdx)
        {
            float x = landmark[idx].X;
            float y = landmark[idx].Y;
            float z = landmark[idx].Z;

            if (_bbMin.x < x) _bbMin.x = x;
            if (_bbMax.x > x) _bbMax.x = x;
            if (_bbMin.y < y) _bbMin.y = y;
            if (_bbMax.y > y) _bbMax.y = y;
            if (_bbMin.z < z) _bbMin.z = z;
            if (_bbMax.z > z) _bbMax.z = z;
        }
        return Vector3.Distance( _bbMin, _bbMax);
    }
}
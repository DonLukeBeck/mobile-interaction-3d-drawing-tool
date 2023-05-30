using System;
using System.Collections.Generic;
using Mediapipe;
using UnityEngine;
using UnityEngine.Serialization;

public class DrawingTool : HandLandmarkUser
{
    public int HandScale = 10;
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
            _handSkeletons[i] = new HandSkeleton();
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
            AveragePosition[i] = new Vector3(0, 0, 0);
            for (int o = 0; o < landmark.Count; o++)
            {
                float x = landmark[o].X * HandScale;
                float y = landmark[o].Y * HandScale * -1;
                float z = landmark[o].Z * HandScale;

                _handSkeletons[i].Joints[o].transform.localPosition = new Vector3(x, y, z);
                AveragePosition[i] += new Vector3(x, y, z);
            }

            //_handSkeletons[i].IsActive = true;
            AveragePosition[i] /= landmark.Count;
        }
    }
}
using System;
using System.Collections.Generic;
using Mediapipe;
using UnityEngine;

public class DrawingTool : HandLandmarkUser
{
    public int handScale = 10;
    public bool gestureControlled;
    public Vector3 averagePosition;

    private List<HandSkeleton> _handSkeletons = new();
    void Awake()
    {
        averagePosition = new Vector3(0, 0, 0);
        _handSkeletons.Add(new HandSkeleton());
    }
    
    public override void ProcessHandLandmark(IList<NormalizedLandmarkList> handLandmarkLists, IList<ClassificationList> handedness = null)
    {
        if (handLandmarkLists == null || handLandmarkLists.Count == 0) return;
        // We assume that we are tracking one hand
        
        for (int i = 0; i < handLandmarkLists[0].Landmark.Count; i++)
        {
            float x = handLandmarkLists[0].Landmark[i].X * handScale;
            float y = handLandmarkLists[0].Landmark[i].Y * handScale * -1;
            float z = handLandmarkLists[0].Landmark[i].Z * handScale;
        
            _handSkeletons[0].Joints[i].transform.localPosition = new Vector3(x, y, z);
            averagePosition += new Vector3(x, y, z);
        }
        averagePosition /= handLandmarkLists[0].Landmark.Count;
    }
}
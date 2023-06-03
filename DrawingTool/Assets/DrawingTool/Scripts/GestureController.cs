using System;
using UnityEngine;
using System.Collections.Generic;
using Mediapipe;

public class GestureController : HandLandmarkUser
{
    public static class Gestures
    {
        public static readonly string Open = "Open";
        public static readonly string Fist = "Fist";
        public static readonly string ThumbsUp = "ThumbsUp";
        public static readonly string Peace = "Peace";
    }
    
    [SerializeField] private SpriteRenderer _pointer;
    public float bentThreshold = 20;
    public float thumbBentThreshold = 5;
    public string gesture = "";
    
    private float previousSwitchTime;
    [SerializeField] private float gestureSwitchDelay;

    private Dictionary<string, Sprite> GestureIcons;

    private Vector3[] _thumbPositions = new Vector3[4];
    private Vector3[] _indexPositions = new Vector3[4];
    private Vector3[] _middlePositions = new Vector3[4];
    private Vector3[] _ringPositions = new Vector3[4];
    private Vector3[] _pinkyPositions = new Vector3[4];
    
    private void Start()
    {
        _pointer = GameObject.Find("Pointer").GetComponent<SpriteRenderer>();
        GestureIcons = new();
        GestureIcons.Add(Gestures.Open, Resources.Load<Sprite>("Sprites/hand_icon"));
        GestureIcons.Add(Gestures.Fist, Resources.Load<Sprite>("Sprites/draw_icon"));
        GestureIcons.Add(Gestures.ThumbsUp, Resources.Load<Sprite>("Sprites/draw_icon"));
        GestureIcons.Add(Gestures.Peace, Resources.Load<Sprite>("Sprites/draw_icon"));
        previousSwitchTime = Time.fixedTime;
    }

    public override void ProcessHandLandmark(IList<NormalizedLandmarkList> handLandmarkLists, IList<ClassificationList> handedness = null)
    {
        if (handLandmarkLists == null || handLandmarkLists.Count == 0) return;

        var hl0 = handLandmarkLists[0].Landmark;

        CopyPosition(ref _thumbPositions[0], hl0[1]);
        CopyPosition(ref _thumbPositions[1], hl0[2]);
        CopyPosition(ref _thumbPositions[2], hl0[3]);
        CopyPosition(ref _thumbPositions[3], hl0[4]);
        
        CopyPosition(ref _indexPositions[0], hl0[5]);
        CopyPosition(ref _indexPositions[1], hl0[6]);
        CopyPosition(ref _indexPositions[2], hl0[7]);
        CopyPosition(ref _indexPositions[3], hl0[8]);
        
        CopyPosition(ref _middlePositions[0], hl0[9]);
        CopyPosition(ref _middlePositions[1], hl0[10]);
        CopyPosition(ref _middlePositions[2], hl0[11]);
        CopyPosition(ref _middlePositions[3], hl0[12]);

        CopyPosition(ref _ringPositions[0], hl0[13]);
        CopyPosition(ref _ringPositions[1], hl0[14]);
        CopyPosition(ref _ringPositions[2], hl0[15]);
        CopyPosition(ref _ringPositions[3], hl0[16]);
        
        CopyPosition(ref _pinkyPositions[0], hl0[17]);
        CopyPosition(ref _pinkyPositions[1], hl0[18]);
        CopyPosition(ref _pinkyPositions[2], hl0[19]);
        CopyPosition(ref _pinkyPositions[3], hl0[20]);

        float thumbAngle = GetAverageAngle(_thumbPositions);
        float indexAngle = GetAverageAngle(_indexPositions);
        float middleAngle = GetAverageAngle(_middlePositions);
        float ringAngle = GetAverageAngle(_ringPositions);
        float pinkyAngle = GetAverageAngle(_pinkyPositions);

        bool thumbBent = thumbAngle > thumbBentThreshold;
        bool indexBent = indexAngle > bentThreshold;
        bool middleBent = middleAngle > bentThreshold;
        bool ringBent = ringAngle > bentThreshold;
        bool pinkyBent = pinkyAngle > bentThreshold;
        
        string previousGesture = gesture;
        string currGesture = GetGesture(thumbBent, indexBent, middleBent, ringBent, pinkyBent);
        if (previousGesture != currGesture && Time.fixedTime - previousSwitchTime > gestureSwitchDelay) {
            previousSwitchTime = Time.fixedTime;
            gesture = currGesture;
            _pointer.sprite = GestureIcons[gesture];
        }
    }

    private void CopyPosition(ref Vector3 position, NormalizedLandmark landmark)
    {
        position.x = landmark.X;
        position.y = landmark.Y;
        position.z = landmark.Z;
    }

    private string GetGesture(bool thumb, bool index, bool middle, bool ring, bool pinky)
    {
        if (thumb && index && middle && ring && pinky) return Gestures.Fist;
        if (index && middle && ring && pinky) return Gestures.ThumbsUp;
        if (ring && pinky) return Gestures.Peace;
        return Gestures.Open;
    }
    
    private float GetAverageAngle(Vector3[] positions)
    {
        float sum = 0;
        for (int z = 0; z < positions.Length - 3; z++)
        {
            sum += GetAngle(positions[z], positions[z + 1], positions[z + 2]);
        }
        sum /= positions.Length - 2;
        return sum;
    }

    private float GetAngle(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Vector3 AB = pointB - pointA;
        Vector3 BC = pointC - pointB;

        float angle = Vector3.Angle(AB, BC);

        return angle;
    }
}
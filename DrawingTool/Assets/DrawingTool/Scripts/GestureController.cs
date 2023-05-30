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
    
    public float bentThreshold = 20;
    public float thumbBentThreshold = 5;
    public string gesture = "";
    
    private Dictionary<string, Sprite> GestureIcons;
    [SerializeField] private SpriteRenderer _pointer;

    private void Start()
    {
        _pointer = GameObject.Find("Pointer").GetComponent<SpriteRenderer>();
        GestureIcons = new();
        GestureIcons.Add(Gestures.Open, Resources.Load<Sprite>("Sprites/hand_icon"));
        GestureIcons.Add(Gestures.Fist, Resources.Load<Sprite>("Sprites/draw_icon"));
        GestureIcons.Add(Gestures.ThumbsUp, Resources.Load<Sprite>("Sprites/draw_icon"));
        GestureIcons.Add(Gestures.Peace, Resources.Load<Sprite>("Sprites/draw_icon"));
    }

    public override void ProcessHandLandmark(IList<NormalizedLandmarkList> handLandmarkLists, IList<ClassificationList> handedness = null)
    {
        if (handLandmarkLists == null || handLandmarkLists.Count == 0) return;

        var hl0 = handLandmarkLists[0].Landmark;
        
        List<Vector3> thumbPositions = new List<Vector3>()
        {
            new Vector3(hl0[1].X, hl0[1].Y, hl0[1].Z),
            new Vector3(hl0[2].X, hl0[2].Y, hl0[2].Z),
            new Vector3(hl0[3].X, hl0[3].Y, hl0[3].Z),
            new Vector3(hl0[4].X, hl0[4].Y, hl0[4].Z)
        };

        List<Vector3> indexPositions = new List<Vector3>()
        {
            new Vector3(hl0[5].X, hl0[5].Y, hl0[5].Z),
            new Vector3(hl0[6].X, hl0[6].Y, hl0[6].Z),
            new Vector3(hl0[7].X, hl0[7].Y, hl0[7].Z),
            new Vector3(hl0[8].X, hl0[8].Y, hl0[8].Z)
        };

        List<Vector3> middlePositions = new List<Vector3>()
        {
            new Vector3(hl0[9].X, hl0[9].Y, hl0[9].Z),
            new Vector3(hl0[10].X, hl0[10].Y, hl0[10].Z),
            new Vector3(hl0[11].X, hl0[11].Y, hl0[11].Z),
            new Vector3(hl0[12].X, hl0[12].Y, hl0[12].Z)
        };

        List<Vector3> ringPositions = new List<Vector3>()
        {
            new Vector3(hl0[13].X, hl0[13].Y, hl0[13].Z),
            new Vector3(hl0[14].X, hl0[14].Y, hl0[14].Z),
            new Vector3(hl0[15].X, hl0[15].Y, hl0[15].Z),
            new Vector3(hl0[16].X, hl0[16].Y, hl0[16].Z)
        };

        List<Vector3> pinkyPositions = new List<Vector3>()
        {
            new Vector3(hl0[17].X, hl0[17].Y, hl0[17].Z),
            new Vector3(hl0[18].X, hl0[18].Y, hl0[18].Z),
            new Vector3(hl0[19].X, hl0[19].Y, hl0[19].Z),
            new Vector3(hl0[20].X, hl0[20].Y, hl0[20].Z)
        };

        float thumbAngle = GetAverageAngle(thumbPositions);
        float indexAngle = GetAverageAngle(indexPositions);
        float middleAngle = GetAverageAngle(middlePositions);
        float ringAngle = GetAverageAngle(ringPositions);
        float pinkyAngle = GetAverageAngle(pinkyPositions);

        bool thumbBent = thumbAngle > thumbBentThreshold;
        bool indexBent = indexAngle > bentThreshold;
        bool middleBent = middleAngle > bentThreshold;
        bool ringBent = ringAngle > bentThreshold;
        bool pinkyBent = pinkyAngle > bentThreshold;
        
        string previousGesture = gesture;
        gesture = GetGesture(thumbBent, indexBent, middleBent, ringBent, pinkyBent);
        if (previousGesture != gesture)
            _pointer.sprite = GestureIcons[gesture];
    }

    private string GetGesture(bool thumb, bool index, bool middle, bool ring, bool pinky)
    {
        if (thumb && index && middle && ring && pinky) return Gestures.Fist;
        if (index && middle && ring && pinky) return Gestures.ThumbsUp;
        if (ring && pinky) return Gestures.Peace;
        return Gestures.Open;
    }
    
    private float GetAverageAngle(List<Vector3> positions)
    {
        float sum = 0;
        for (int z = 0; z < positions.Count - 3; z++)
        {
            sum += GetAngle(positions[z], positions[z + 1], positions[z + 2]);
        }
        sum /= positions.Count - 2;
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
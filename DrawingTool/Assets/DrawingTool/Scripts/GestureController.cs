using System;
using UnityEngine;
using System.Collections.Generic;
using Mediapipe;
using UnityEngine.Serialization;

public class GestureController : HandLandmarkUser
{
    public enum Gesture
    {
        UnClear,
        Open,
        OK,
        ThumbsUp,
        Pointing
    }

    [SerializeField] private SpriteRenderer _icon;
    [SerializeField] private HandController _handController;
    public Gesture gesture = Gesture.UnClear;
    public Vector2 StraightFingerThreshold = new Vector2(20, 20);
    public Vector2 StraightThumbThreshold = new Vector2(5, 15);
    [Range(0, .4f)] public float isClosePercentage = .2f;

    private float previousSwitchTime;
    [SerializeField] private float gestureSwitchDelay;

    private Dictionary<Gesture, Sprite> GestureIcons;

    private Vector3[] _thumbPositions = new Vector3[4];
    private Vector3[] _indexPositions = new Vector3[4];
    private Vector3[] _middlePositions = new Vector3[4];
    private Vector3[] _ringPositions = new Vector3[4];
    private Vector3[] _pinkyPositions = new Vector3[4];

    private void Start()
    {
        if(_icon == null) _icon = GameObject.Find("Icon").GetComponent<SpriteRenderer>();
        _handController = GameObject.Find("Manager").GetComponent<HandController>();
        previousSwitchTime = Time.fixedTime;

        GestureIcons = new();
        GestureIcons.Add(Gesture.UnClear, Resources.Load<Sprite>("Sprites/hand_icon"));
        GestureIcons.Add(Gesture.Open, Resources.Load<Sprite>("Sprites/hand_icon"));
        GestureIcons.Add(Gesture.OK, Resources.Load<Sprite>("Sprites/draw_icon"));
        GestureIcons.Add(Gesture.Pointing, Resources.Load<Sprite>("Sprites/draw_icon"));
        GestureIcons.Add(Gesture.ThumbsUp, Resources.Load<Sprite>("Sprites/draw_icon"));
    }

    public override void ProcessHandLandmark(IList<NormalizedLandmarkList> handLandmarkLists,
        IList<ClassificationList> handedness = null)
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

        bool thumb = IsStraight(_thumbPositions, StraightThumbThreshold);
        bool index = IsStraight(_indexPositions, StraightFingerThreshold);
        bool middle = IsStraight(_middlePositions, StraightFingerThreshold);
        bool ring = IsStraight(_ringPositions, StraightFingerThreshold);
        bool pinky = IsStraight(_pinkyPositions, StraightFingerThreshold);
        bool ok = IsClose(_indexPositions[3], _thumbPositions[3], isClosePercentage);

        Gesture previousGesture = gesture;
        Gesture currGesture = GetGesture(thumb, index, middle, ring, pinky, ok);
        if (previousGesture != currGesture && Time.fixedTime - previousSwitchTime > gestureSwitchDelay)
        {
            previousSwitchTime = Time.fixedTime;
            gesture = currGesture;
            _icon.sprite = GestureIcons[gesture];
        }
    }

    private void CopyPosition(ref Vector3 position, NormalizedLandmark landmark)
    {
        position.x = landmark.X;
        position.y = landmark.Y;
        position.z = landmark.Z;
    }

    private Gesture GetGesture(bool thumb, bool index, bool middle, bool ring, bool pinky, bool ok)
    {
        if (ok && !index && !middle && !ring && !pinky) return Gesture.OK;
        if (index && !thumb && !middle && !ring && !pinky) return Gesture.Pointing;
        if (thumb && !middle && !ring && !pinky) return Gesture.ThumbsUp;
        if (index && middle && ring && pinky) return Gesture.Open;
        return Gesture.UnClear;
    }

    private bool IsStraight(Vector3[] positions, Vector2 threshold, bool log = false)
    {
        float angle = GetAngle(positions[0], positions[1], positions[2]);

        if (log)
        {
            float angle2 = GetAngle(positions[1], positions[2], positions[3]);
            Debug.Log(angle + " " + angle2);
        }

        if (angle > threshold.x) return false;

        angle = GetAngle(positions[1], positions[2], positions[3]);
        if (angle > threshold.y) return false;

        return true;
    }

    private float GetAngle(Vector3 pointA, Vector3 pointB, Vector3 pointC)
    {
        Vector3 AB = pointB - pointA;
        Vector3 BC = pointC - pointB;

        float angle = Vector3.Angle(AB, BC);

        return angle;
    }

    private bool IsClose(Vector3 pointA, Vector3 pointB, float percentage)
    {
        float d = Vector3.Distance(pointA, pointB);
        float palmSize = _handController.PalmSize[0];
        return d < palmSize * percentage;
    }
}
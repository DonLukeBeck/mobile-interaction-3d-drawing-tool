using System;
using System.Collections.Generic;
using Mediapipe.Unity.HandTracking;
using UnityEngine;

public class DrawingSettings : MonoBehaviour
{
    private static DrawingSettings _instance;

    public static DrawingSettings Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogError("Missing HandTrackingSettings instance");
            return _instance;
        }
    }

    public int MaxNumHands { get; private set; }

    private void Awake()
    {
        _instance = this;
        MaxNumHands = GameObject.Find("Solution").GetComponent<HandTrackingGraph>().maxNumHands;
    }

    public List<Tuple<int, int>> HandDescriptor { get; } = new List<Tuple<int, int>>()
    {
        new (0, 1), //   Thumb 1          0
        new (1, 2), //   Thumb 2          1
        new (2, 3), //   Thumb 3          2
        new (3, 4), //   Thumb 4          3
        new (0, 5), //   Palm 1           4
        new (5, 6), //   Index 1          5
        new (6, 7), //   Index 2          6
        new (7, 8), //   Index 3          7
        new (9, 10), //   Middle 1         8
        new (10, 11), //   Middle 2         9
        new (11, 12), //   Middle 3        10
        new (13, 14), //   Ring 1          11
        new (14, 15), //   Ring 2          12
        new (15, 16), //   Ring 3          13
        new (0, 17), //   Palm 2          14
        new (17, 18), //   Pinky 1         15
        new (18, 19), //   Pinky 2         16
        new (19, 20), //   Pinky 3         17
        new (5, 9), //   Index - Middle  18
        new (9, 13), //   Middle - Ring   19
        new (13, 17) //   Ring - Pinky    20
    };
}
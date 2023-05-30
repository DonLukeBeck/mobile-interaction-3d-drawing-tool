using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawingSettings : MonoBehaviour
{
    private static DrawingSettings _instance;

    public static DrawingSettings Instance
    {
        get
        {
            if (_instance is null) Debug.LogError("Missing HandTrackingSettings instance");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public int handTrackingCount => 1;

    public List<Tuple<int,int>> HandDescriptor { get; } = new List<Tuple<int, int>>()
    {
        new Tuple<int, int>( 0, 1 ),   //   Thumb 1          0
        new Tuple<int, int>( 1, 2 ),   //   Thumb 2          1
        new Tuple<int, int>( 2, 3 ),   //   Thumb 3          2
        new Tuple<int, int>( 3, 4 ),   //   Thumb 4          3
        new Tuple<int, int>( 0, 5 ),   //   Palm 1           4
        new Tuple<int, int>( 5, 6 ),   //   Index 1          5
        new Tuple<int, int>( 6, 7 ),   //   Index 2          6
        new Tuple<int, int>( 7, 8 ),   //   Index 3          7
        new Tuple<int, int>( 9, 10 ),  //   Middle 1         8
        new Tuple<int, int>( 10, 11 ), //   Middle 2         9
        new Tuple<int, int>( 11, 12 ), //   Middle 3        10
        new Tuple<int, int>( 13, 14 ), //   Ring 1          11
        new Tuple<int, int>( 14, 15 ), //   Ring 2          12
        new Tuple<int, int>( 15, 16 ), //   Ring 3          13
        new Tuple<int, int>( 0, 17 ),  //   Palm 2          14
        new Tuple<int, int>( 17, 18 ), //   Pinky 1         15
        new Tuple<int, int>( 18, 19 ), //   Pinky 2         16
        new Tuple<int, int>( 19, 20 ), //   Pinky 3         17
        new Tuple<int, int>( 5, 9 ),   //   Index - Middle  18
        new Tuple<int, int>( 9 , 13 ), //   Middle - Ring   19
        new Tuple<int, int>( 13, 17 )  //   Ring - Pinky    20
    };
}

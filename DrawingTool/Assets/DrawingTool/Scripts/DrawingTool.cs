using System;
using System.Collections.Generic;
using Mediapipe;
using UnityEngine;

public class DrawingTool : MonoBehaviour
{
    public int handScale = 10;
    
    private GameObject hand;
    private GameObject[] joints;
    private GameObject[] bones;

    public bool gestureControlled;

    public Vector3 averagePosition;
    
    List<Tuple<int,int>> handDescriptor = new List<Tuple<int, int>>()
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

    void Awake()
    {
        joints = new GameObject[handDescriptor.Count];
        bones = new GameObject[handDescriptor.Count];
        averagePosition = new Vector3(0, 0, 0);
    }

    void Start()
    {
        hand = new GameObject("Hand");
        for (int i = 0; i < handDescriptor.Count; i++)
        {
            joints[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Sphere"), hand.transform);
            joints[i].name = i.ToString();
        }

        for (int i = 0; i < handDescriptor.Count; i++)
        {
            bones[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Line"), hand.transform);
            JointConnector jc = bones[i].AddComponent<JointConnector>();
            int originIdx = handDescriptor[i].Item1;
            int destIdx = handDescriptor[i].Item2;
            jc.origin = joints[originIdx].transform;
            jc.destination = joints[destIdx].transform;
            bones[i].name = "Bone " + originIdx.ToString() + "-" + destIdx.ToString();
        }
    }

    public void UpdateJoints(IList<NormalizedLandmarkList> handLandmarkLists, IList<ClassificationList> handedness = null)
    {
        if (handLandmarkLists == null || handLandmarkLists.Count == 0) return;
        // We assume that we are tracking one hand
        
        for (int i = 0; i < handLandmarkLists[0].Landmark.Count; i++)
        {
            float x = handLandmarkLists[0].Landmark[i].X * handScale;
            float y = handLandmarkLists[0].Landmark[i].Y * handScale * -1;
            float z = handLandmarkLists[0].Landmark[i].Z * handScale;
        
            joints[i].transform.localPosition = new Vector3(x, y, z);
            averagePosition += new Vector3(x, y, z);
            Debug.Log("F" + new Vector3(x, y, z));
        }
        averagePosition /= handLandmarkLists[0].Landmark.Count;
    }
}
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
    
    List<Tuple<int,int>> handDescriptor = new List<Tuple<int, int>>()
    {
        new Tuple<int, int>( 0, 1 ), 
        new Tuple<int, int>( 1, 2 ), 
        new Tuple<int, int>( 2, 3 ), 
        new Tuple<int, int>( 3, 4 ),
        new Tuple<int, int>( 0, 5 ), 
        new Tuple<int, int>( 5, 6 ), 
        new Tuple<int, int>( 6, 7 ), 
        new Tuple<int, int>( 7, 8 ),
        new Tuple<int, int>( 9, 10 ), 
        new Tuple<int, int>( 10, 11 ) , 
        new Tuple<int, int>( 11, 12 ),
        new Tuple<int, int>( 13, 14 ), 
        new Tuple<int, int>( 14, 15 ), 
        new Tuple<int, int>( 15, 16 ),
        new Tuple<int, int>( 0, 17 ), 
        new Tuple<int, int>( 17, 18 ), 
        new Tuple<int, int>( 18, 19 ), 
        new Tuple<int, int>( 19, 20 ),
        new Tuple<int, int>( 5, 9 ) , 
        new Tuple<int, int>( 9 ,13 ),
        new Tuple<int, int>( 13, 17 )
    };

    void Awake()
    {
        joints = new GameObject[handDescriptor.Count];
        bones = new GameObject[handDescriptor.Count];
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
        }
        
    }
}
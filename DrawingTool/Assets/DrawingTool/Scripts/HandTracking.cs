using System;
using System.Collections.Generic;
using UnityEngine;

public class HandTracking : MonoBehaviour
{
    public UDPReceive udpReceive;

    GameObject hand;
    GameObject[] joints;
    GameObject[] bones;
    
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

    void Update()
    {
        UpdateJoints();
    }

    void UpdateJoints()
    {
        string data = udpReceive.data;

        data = data.Remove(0, 1);
        data = data.Remove(data.Length - 1, 1);
        print(data);
        string[] points = data.Split(',');
        print(points[0]);

        //0        1*3      2*3
        //x1,y1,z1,x2,y2,z2,x3,y3,z3

        for (int i = 0; i < handDescriptor.Count; i++)
        {
            float x = 7 - float.Parse(points[i * 3]) / 100;
            float y = float.Parse(points[i * 3 + 1]) / 100;
            float z = float.Parse(points[i * 3 + 2]) / 100;

            joints[i].transform.localPosition = new Vector3(x, y, z);
        }
    }
}
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

[System.Serializable]
public class GestureController : MonoBehaviour
{
    [SerializeField]
    private GameObject handLandmarks;

    private List<GameObject> points = new List<GameObject>();

    public float bentThreshold = 20;
    public float thumbBentThreshold = 5;
    public string gesture = "";

    private void Start()
    {
        FillChildren();
    }

    public void CheckGesture()
    {
        if (points.Count == 0)
        {
            if (!FillChildren()) return;
        }

        List<Vector3> thumbPositions = new List<Vector3>()
        {
            points[1].transform.localPosition,
            points[2].transform.localPosition,
            points[3].transform.localPosition,
            points[4].transform.localPosition
        };

        List<Vector3> indexPositions = new List<Vector3>()
        {
            points[5].transform.localPosition,
            points[6].transform.localPosition,
            points[7].transform.localPosition,
            points[8].transform.localPosition
        };

        List<Vector3> middlePositions = new List<Vector3>()
        {
            points[9].transform.localPosition,
            points[10].transform.localPosition,
            points[11].transform.localPosition,
            points[12].transform.localPosition
        };

        List<Vector3> ringPositions = new List<Vector3>()
        {
            points[13].transform.localPosition,
            points[14].transform.localPosition,
            points[15].transform.localPosition,
            points[16].transform.localPosition
        };

        List<Vector3> pinkyPositions = new List<Vector3>()
        {
            points[17].transform.localPosition,
            points[18].transform.localPosition,
            points[19].transform.localPosition,
            points[20].transform.localPosition
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

        gesture = GetGesture(thumbBent, indexBent, middleBent, ringBent, pinkyBent);
    }

    private string GetGesture(bool thumb, bool index, bool middle, bool ring, bool pinky)
    {
        if (thumb && index && middle && ring && pinky) return "fist";
        if (index && middle && ring && pinky) return "thumbsup";
        if (ring && pinky) return "peace";
        return "open";
    }

    private bool FillChildren()
    {
        if (handLandmarks.transform.childCount == 0) return false;
        foreach (Transform child in handLandmarks.transform.GetChild(0).GetChild(0).transform)
        {
            points.Add(child.gameObject);
        }
        return true;
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
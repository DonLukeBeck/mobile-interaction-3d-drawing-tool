using UnityEngine;

public class HandSkeleton : MonoBehaviour
{
    public GameObject[] Joints { get; private set; }
    
    private GameObject _hand;
    private GameObject[] _bones;

    public HandSkeleton()
    {
        Joints = new GameObject[DrawingSettings.Instance.HandDescriptor.Count];
        _bones = new GameObject[DrawingSettings.Instance.HandDescriptor.Count];
        _hand = new GameObject("Hand");
        
        for (int i = 0; i < DrawingSettings.Instance.HandDescriptor.Count; i++)
        {
            Joints[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Sphere"), _hand.transform);
            Joints[i].name = i.ToString();
        }

        for (int i = 0; i < DrawingSettings.Instance.HandDescriptor.Count; i++)
        {
            _bones[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Line"), _hand.transform);
            JointConnector jc = _bones[i].AddComponent<JointConnector>();
            int originIdx = DrawingSettings.Instance.HandDescriptor[i].Item1;
            int destIdx = DrawingSettings.Instance.HandDescriptor[i].Item2;
            jc.origin = Joints[originIdx].transform;
            jc.destination = Joints[destIdx].transform;
            _bones[i].name = "Bone " + originIdx.ToString() + "-" + destIdx.ToString();
        }
    }
}

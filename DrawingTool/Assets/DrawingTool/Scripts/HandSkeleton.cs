using UnityEngine;

public class HandSkeleton : MonoBehaviour
{
    public GameObject[] Joints { get; }

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
            _hand.transform.localPosition = value ? new Vector3(0, 0) : new Vector3(99999, 99999);
        }
    }

    private readonly GameObject _hand;
    private GameObject[] _bones;
    private bool _isActive;

    public HandSkeleton(float jointScale = 1f, float boneWidth = 1f)
    {
        Joints = new GameObject[DrawingSettings.Instance.HandDescriptor.Count];
        _bones = new GameObject[DrawingSettings.Instance.HandDescriptor.Count];
        _hand = new GameObject("Hand");

        for (int i = 0; i < DrawingSettings.Instance.HandDescriptor.Count; i++)
        {
            Joints[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Sphere"), _hand.transform);
            Joints[i].name = i.ToString();
            Joints[i].transform.localScale = new Vector3(jointScale, jointScale, jointScale);
        }

        for (int i = 0; i < DrawingSettings.Instance.HandDescriptor.Count; i++)
        {
            _bones[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Line"), _hand.transform);
            JointConnector jc = _bones[i].AddComponent<JointConnector>();
            int originIdx = DrawingSettings.Instance.HandDescriptor[i].Item1;
            int destIdx = DrawingSettings.Instance.HandDescriptor[i].Item2;
            jc.origin = Joints[originIdx].transform;
            jc.destination = Joints[destIdx].transform;
            jc.SetWidth(boneWidth);
            _bones[i].name = "Bone " + originIdx + "-" + destIdx;
        }
    }
}
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
            _hand.transform.localScale = value ? _jointScale : new Vector3(0, 0, 0);
        }
    }

    private readonly GameObject _hand;
    private GameObject[] _bones;
    private bool _isActive;
    private readonly Vector3 _jointScale;

    public HandSkeleton(float jointScale = 1f, float boneWidth = 1f)
    {
        Joints = new GameObject[DrawingSettings.Instance.HandDescriptor.Count];
        _bones = new GameObject[DrawingSettings.Instance.HandDescriptor.Count];
        _hand = new GameObject("Hand");

        for (int i = 0; i < DrawingSettings.Instance.HandDescriptor.Count; i++)
        {
            Joints[i] = Instantiate(Resources.Load<GameObject>("Prefabs/Sphere"), _hand.transform);
            Joints[i].name = i.ToString();
            _jointScale = new Vector3(jointScale, jointScale, jointScale);
            Joints[i].transform.localScale = _jointScale;
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
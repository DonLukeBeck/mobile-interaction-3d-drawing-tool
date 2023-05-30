using System.Collections.Generic;
using Mediapipe;
using UnityEngine;

public abstract class HandLandmarkUser : MonoBehaviour
{
    public abstract void ProcessHandLandmark(IList<NormalizedLandmarkList> handLandmarkLists, IList<ClassificationList> handedness = null);
}

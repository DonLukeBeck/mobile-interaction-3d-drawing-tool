using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintBrush : MonoBehaviour
{
    public GameObject brush, manager;
    public float brushSize;
    // Update is called once per frame

    private HandTracking handTracking;
    void Start()
    {
        handTracking = manager.GetComponent<HandTracking>();
    }
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var paint = Instantiate(Resources.Load<GameObject>("Prefabs/Brush"));
            paint.transform.localScale = Vector3.one * brushSize;
            paint.transform.localPosition = handTracking.averagePos;
        } 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteGridHandler : MonoBehaviour
{
    public Transform target;
    public float snap;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = new Vector3(Mathf.Round(target.position.x / snap) * snap,   //x 
                                 -6,                                              //y
                                  Mathf.Round(target.position.z / snap) * snap);  //z
        transform.position = pos;                                                                                  
    }
}

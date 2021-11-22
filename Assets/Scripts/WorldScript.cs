using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldScript : MonoBehaviour
{
    public Transform worldLight;
    // Start is called before the first frame update
    [Range(0.1f,10)]
    public float timeInterval;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var dt = Time.deltaTime;
        worldLight.Rotate(Vector3.right,timeInterval*dt);
    }
}

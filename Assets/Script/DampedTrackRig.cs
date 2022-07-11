using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DampedTrackRig : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform source;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(transform.position, target.position - transform.position, Color.red);
        Debug.DrawRay(transform.position, transform.forward, Color.red);
        //Debug.DrawLine(source.position, target.position, Color.yellow);
        transform.LookAt(target.position, Vector3.up);// = Quaternion.Euler(target.position - source.position);
        //Debug.Log("tracking");
    }
}

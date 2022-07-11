using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApparatusMap : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed = 4f;

    private Vector3 initialPos;
    private bool isMoving;
    private bool isDrag;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                initialPos = Input.mousePosition;
                isDrag = true;
            }

            if (Input.GetMouseButtonUp(0))
            {
                isDrag = false;
                //targetRB.angularVelocity = Vector3.zero;
            }

            if (isDrag)
            {
                //float apparatusRotation = Mathf.Lerp(0, 180f, rotationScrollbar.value);
                //transform.rotation = Quaternion.Euler((new Vector3(Input.mousePosition.y, -Input.mousePosition.x, Input.mousePosition.z) - new Vector3(initialPos.y, -initialPos.x, initialPos.z)).normalized * Time.deltaTime * rotateSpeed);
                transform.parent.Rotate((new Vector3(Input.mousePosition.y, -Input.mousePosition.x, Input.mousePosition.z) - new Vector3(initialPos.y, -initialPos.x, initialPos.z)).normalized * Time.deltaTime * rotateSpeed, Space.World);
                initialPos = Input.mousePosition;
            }
        }
    }

    public void ReadyMove()
    {
        isMoving = true;
    }

    public void ReleaseMove()
    {
        isMoving = false;
        isDrag = false;
    }

    public void ResetToDefault()
    {
        transform.parent.rotation = Quaternion.identity;
    }
}

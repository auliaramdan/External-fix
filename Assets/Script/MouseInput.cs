using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : IApparatusInput
{
    public Vector2 RayOrigin => Input.mousePosition;
    public bool Click => Input.GetMouseButtonDown(0);
}

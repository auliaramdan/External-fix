using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IApparatusInput
{
    bool Click { get; }
    Vector2 RayOrigin { get; }
}

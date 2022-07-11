using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartSimulation : MonoBehaviour
{
    [SerializeField]
    private Animator fixatorAnimator;
    [SerializeField]
    private TMP_InputField distanceInput, rotXInput, rotYInput;
    [SerializeField]
    private Apparatus apparatus;

    public void StartApp()
    {
        float position = float.Parse(distanceInput.text);
        position = (position > 10 ? 10 : position)/10;
        //fixatorAnimator.SetFloat("Position",  position);
        apparatus.StartSimulation(position);

        float rotationXZ = float.Parse(rotXInput.text);
        if (rotationXZ > 45) rotationXZ = 45;
        else if (rotationXZ < -45) rotationXZ = -45;
        fixatorAnimator.SetFloat("RotationXZ", (rotationXZ > 45 ? 45 : rotationXZ)/45);

        float rotationYZ = float.Parse(rotYInput.text);
        if (rotationYZ > 45) rotationYZ = 45;
        else if (rotationYZ < -45) rotationYZ = -45;
        fixatorAnimator.SetFloat("RotationYZ", rotationYZ/45);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProcessParameter : MonoBehaviour
{
    [SerializeField] private float strutMinLength = 50;
    [SerializeField] private float strutMaxLength = 250;
    
    [Header("Initial")]
    [SerializeField]
    private TMP_InputField translationInitial;
    [SerializeField]
    private TMP_InputField rotationInitial;
    [SerializeField]
    private TMP_InputField apInitial;
    [SerializeField]
    private TMP_InputField lateralInitial;
    [SerializeField]
    private TMP_InputField ringDistanceInitial;

    [Header("Final")]
    [SerializeField]
    private TMP_InputField translationFinal;
    [SerializeField]
    private TMP_InputField rotationFinal;
    [SerializeField]
    private TMP_InputField apFinal;
    [SerializeField]
    private TMP_InputField lateralFinal;
    [SerializeField]
    private TMP_InputField ringDistanceFinal;

    [Header("Initial Field")] 
    [SerializeField]
    private List<TMP_InputField> lengthFields = new List<TMP_InputField>();

    [Header("Point origin shift field")] [SerializeField]
    private TMP_InputField shiftField;

    private ProcessOutput outputProcessor;
    private float minRingDistance = 33.716f;
    private float maxRingDistance = 111.878f;

    private float _xPointLateralView = 0;
    private float _yPointAPView = 0;

    private Vector2[] _lengthConfig =
    {
        
        new Vector2(-95, 101),
        new Vector2(-24, 133),
        new Vector2(124, 44),
        new Vector2(124, -41),
        new Vector2(-31, -133),
        new Vector2(-98, -102),
    };

    private Vector2[] animationParameterConfig =
    {
        new Vector2(-1, 1),
        new Vector2(-1, 0),
        new Vector2(-0.6f, -1),
        new Vector2(1, -1),
        new Vector2(1, 0),
        new Vector2(0.6f,1)
    };

    private Vector3[] animationParameterConfigZ =
    {
        new Vector3(-0.9f, 0.6f, -0.2f),
        new Vector3(-0.9f, -0.6f, 0.2f),
        new Vector3(-0.5f, -1, -0.23f),
        new Vector3(0.5f, -0.2f, 0.23f),
        new Vector3(0.9f, 0.2f, -0.2f),
        new Vector3(0.9f, 1f, 0.2f)
    };

    private float _averageInitialLength;
    private float _averageZRotation;

    private int[] _positiveZStrut = {0, 2, 4};
    private int[] _negativeZStrut = {1, 3, 5};
    
    private float outputShiftUpper = 100;
    private float outputShiftLower = 50;
    
    // Start is called before the first frame update
    void Start()
    {
        outputProcessor = GetComponent<ProcessOutput>();
        
        for (var i = 0; i < lengthFields.Count; i++)
        {
            lengthFields[i].text = "130";
            outputProcessor.InitialLengthField[i] = lengthFields[i];
        }

        Default(true);
    }

    /// <summary>
    /// Submit parameters to output processor
    /// </summary>
    [Obsolete]
    public void Submit()
    {
        float initialTranslationVal = float.Parse(translationInitial.text == "" ? "0" : translationInitial.text);
        initialTranslationVal = initialTranslationVal >= 0 ? (initialTranslationVal > 52 ? 52 : initialTranslationVal) : 0;
        translationInitial.text = initialTranslationVal.ToString();

        float initialRotationVal = float.Parse(rotationInitial.text == "" ? "0" : rotationInitial.text);
        initialRotationVal = initialRotationVal > 0 ? (initialRotationVal > 90 ? 90 : initialRotationVal) : (initialRotationVal < -90 ? -90 : initialRotationVal);
        rotationInitial.text = initialRotationVal.ToString();

        float initialLateralViewVal = float.Parse(lateralInitial.text == "" ? "0" : lateralInitial.text);
        initialLateralViewVal = initialLateralViewVal > 0 ? (initialLateralViewVal > 45 ? 45 : initialLateralViewVal) : (initialLateralViewVal < -45 ? -45 : initialLateralViewVal);
        lateralInitial.text = initialLateralViewVal.ToString();

        float initialAPViewVal = float.Parse(apInitial.text == "" ? "0" : apInitial.text);
        initialAPViewVal = initialAPViewVal > 0 ? (initialAPViewVal > 45 ? 45 : initialAPViewVal) : (initialAPViewVal < -45 ? -45 : initialAPViewVal);
        apInitial.text = initialAPViewVal.ToString();

        float initialRingDistance = float.Parse(ringDistanceInitial.text == "" ? minRingDistance.ToString() : ringDistanceInitial.text);
        initialRingDistance = initialRingDistance >= minRingDistance ? (initialRingDistance > (maxRingDistance - _averageInitialLength) ? (maxRingDistance - _averageInitialLength) : initialRingDistance) : minRingDistance;
        ringDistanceInitial.text = initialRingDistance.ToString();
        //
        float finalTranslationVal = float.Parse(translationFinal.text == "" ? "0" : translationFinal.text);
        finalTranslationVal = finalTranslationVal > 0 ? (finalTranslationVal > 250 ? 250 : finalTranslationVal) : 0;
        translationFinal.text = finalTranslationVal.ToString();

        float finalRotationVal = float.Parse(rotationFinal.text == "" ? "0" : rotationFinal.text);
        finalRotationVal = finalRotationVal > 0 ? (finalRotationVal > 90 ? 90 : finalRotationVal) : (finalRotationVal < -90 ? -90 : finalRotationVal);
        rotationFinal.text = finalRotationVal.ToString();

        float finalLateralViewVal = float.Parse(lateralFinal.text == "" ? "0" : lateralFinal.text);
        finalLateralViewVal = finalLateralViewVal > 0 ? (finalLateralViewVal > 45 ? 45 : finalLateralViewVal) : (finalLateralViewVal < -45 ? -45 : finalLateralViewVal);
        lateralFinal.text = finalLateralViewVal.ToString();

        float finalAPViewVal = float.Parse(apFinal.text == "" ? "0" : apFinal.text);
        finalAPViewVal = finalAPViewVal > 0 ? (finalAPViewVal > 45 ? 45 : finalAPViewVal) : (finalAPViewVal < -45 ? -45 : finalAPViewVal);
        apFinal.text = finalAPViewVal.ToString();

        float finalRingDistance = float.Parse(ringDistanceFinal.text == "" ? minRingDistance.ToString() : ringDistanceFinal.text);
        finalRingDistance = finalRingDistance >= minRingDistance ? (finalRingDistance > (maxRingDistance - _averageInitialLength) ? (maxRingDistance - _averageInitialLength) : finalRingDistance) : minRingDistance;
        ringDistanceFinal.text = finalRingDistance.ToString();
        
        ProcessLengthInput();
        
        initialAPViewVal = Mathf.Lerp(0, 45, Mathf.Abs(_yPointAPView));
        initialLateralViewVal = Mathf.Lerp(0, 45, Mathf.Abs(_xPointLateralView));
        initialTranslationVal = Mathf.Lerp(0, 52, _averageInitialLength);
        initialRotationVal = Mathf.Lerp(0, 90, Mathf.Abs(_averageZRotation));

        if (_yPointAPView < 0)
        {
            initialAPViewVal *= -1;
        }

        if (_xPointLateralView < 0)
        {
            initialLateralViewVal *= -1;
        }

        if (_averageZRotation < 0)
        {
            initialRotationVal *= -1;
        }

        StartCoroutine(outputProcessor.SubmitNewParameter(initialTranslationVal, initialRotationVal, -initialLateralViewVal, -initialAPViewVal, initialRingDistance, finalTranslationVal, finalRotationVal, finalLateralViewVal, finalAPViewVal, finalRingDistance));
    }

    /// <summary>
    /// Submit new set of parameters to old output processor
    /// </summary>
    public void NewSubmit(bool defaulting = false)
    {
        ProcessLengthInput();
        
        float finalTranslationVal = float.Parse(translationFinal.text == "" ? "0" : translationFinal.text);
        
        float finalRotationVal = float.Parse(rotationFinal.text == "" ? "0" : rotationFinal.text);

        float finalLateralViewVal = float.Parse(lateralFinal.text == "" ? "0" : lateralFinal.text);

        float finalAPViewVal = float.Parse(apFinal.text == "" ? "0" : apFinal.text);
        //finalAPViewVal = finalAPViewVal > 0 ? (finalAPViewVal > 45 ? 45 : finalAPViewVal) : (finalAPViewVal < -45 ? -45 : finalAPViewVal);

        ValidateValues(ref finalTranslationVal, ref finalRotationVal, ref finalLateralViewVal, ref finalAPViewVal);
        
        translationFinal.text = finalTranslationVal.ToString();
        rotationFinal.text = finalRotationVal.ToString();
        lateralFinal.text = finalLateralViewVal.ToString();
        apFinal.text = finalAPViewVal.ToString();

        float initialRingDistance = 0;
        float finalRingDistance = 0;

        //TODO Finish the final output for rotation
        
        //finalTranslationVal = Mathf.Lerp(0, 52, Mathf.InverseLerp(strutMinLength, strutMaxLength, finalTranslationVal - _averageInitialLength));

        finalTranslationVal = _averageInitialLength + finalTranslationVal;
        
        _averageInitialLength = Mathf.InverseLerp(strutMinLength, strutMaxLength, _averageInitialLength);
        
        _averageInitialLength = Mathf.Lerp(0, 52, _averageInitialLength);
        
        finalTranslationVal = Mathf.Lerp(0, 52, Mathf.InverseLerp(strutMinLength, strutMaxLength, finalTranslationVal));
        
        print($"final length change : {finalTranslationVal}");
        finalLateralViewVal += _xPointLateralView;
        finalAPViewVal += _yPointAPView;
        finalRotationVal += _averageZRotation;
        
        Debug.Log($"Initial rotations: {_yPointAPView} {_xPointLateralView} {_averageZRotation}");
        Debug.Log($"Final rotations: {finalAPViewVal} {finalLateralViewVal} {finalRotationVal}");

        float originPointShift = string.IsNullOrWhiteSpace(shiftField.text) ? outputShiftUpper : float.Parse(shiftField.text);

        if (originPointShift > outputShiftUpper)
            originPointShift = outputShiftUpper;
        
        else if (originPointShift < outputShiftLower)
            originPointShift = outputShiftLower;
        
        StartCoroutine(outputProcessor.SubmitNewParameter(
            _averageInitialLength,
            _averageZRotation,
            _xPointLateralView,
            _yPointAPView,
            initialRingDistance,
            finalTranslationVal,
            finalRotationVal,
            finalLateralViewVal,
            finalAPViewVal,
            finalRingDistance,
            originPointShift,
            defaulting)
        );
    }

    /// <summary>
    /// Return all parameters back to 0
    /// </summary>
    public void Default(bool isStart)
    {
        translationInitial.text = "0";
        rotationInitial.text = "0";
        lateralInitial.text = "0";
        apInitial.text = "0";
        ringDistanceInitial.text = Mathf.Lerp(minRingDistance, maxRingDistance, 0.5f).ToString("F3");

        translationFinal.text = "0";
        rotationFinal.text = "0";
        lateralFinal.text = "0";
        apFinal.text = "0";
        ringDistanceFinal.text = Mathf.Lerp(minRingDistance, maxRingDistance, 0.5f).ToString("F3");

        for (int i = 0; i < lengthFields.Count; i++)
        {
            lengthFields[i].text = "130";
        }
        
        shiftField.text = "100";

        if(!isStart) {
            NewSubmit(true);
            outputProcessor.SetOutput();
        }
    }

    private void ProcessLengthInput()
    {
        _xPointLateralView = 0;
        _yPointAPView = 0;
        _averageInitialLength = 0;
        _averageZRotation = 0;

        float currentStrutLength = 0;
        
        for (int i = 0; i < lengthFields.Count; i++)
        {
            currentStrutLength = float.Parse(lengthFields[i].text);

            if (currentStrutLength < strutMinLength)
            {
                lengthFields[i].text = strutMinLength.ToString("F1");
                currentStrutLength = strutMinLength;
            }
            
            else if (currentStrutLength > strutMaxLength)
            {
                lengthFields[i].text = strutMaxLength.ToString("F1");
                currentStrutLength = strutMaxLength;
            }
            
            float lerp = Mathf.InverseLerp(50, 250, currentStrutLength);
            
            //_xPointLateralView += lerp * (_lengthConfig[i].x/136);
            //_yPointAPView += lerp * (_lengthConfig[i].y/136);
            
            _xPointLateralView += lerp * animationParameterConfigZ[i].x;
            _yPointAPView += lerp * animationParameterConfigZ[i].y;
            _averageZRotation += lerp * animationParameterConfigZ[i].z;
            _averageInitialLength += currentStrutLength;
        }
        
        Debug.Log($"xpoint : {_xPointLateralView}");

        _xPointLateralView = Mathf.Lerp(0, _xPointLateralView > 0 ? 45 : -45, Mathf.Abs(_xPointLateralView));
        _yPointAPView = Mathf.Lerp(0, _yPointAPView > 0 ? 45 : -45, Mathf.Abs(_yPointAPView));
        _averageZRotation = Mathf.Lerp(0, _averageZRotation > 0 ? 90 : -90, Mathf.Abs(_averageZRotation));
        
        _averageInitialLength /= lengthFields.Count;
        
        /*
        float positiveZRotation = 0;
        float negativeZRotation = 0;
        
        for (int i = 0; i < _positiveZStrut.Length; i++)
        {
            positiveZRotation += Mathf.InverseLerp(strutMinLength, strutMaxLength, float.Parse(lengthFields[_positiveZStrut[i]].text));
        }

        positiveZRotation /= _positiveZStrut.Length;
        
        for (int i = 0; i < _negativeZStrut.Length; i++)
        {
            negativeZRotation -= Mathf.InverseLerp(strutMinLength, strutMaxLength,float.Parse(lengthFields[_negativeZStrut[i]].text));
        }

        negativeZRotation /= _negativeZStrut.Length;

        _averageZRotation = positiveZRotation + negativeZRotation;
        
        _averageZRotation = Mathf.Lerp(0, 90, Mathf.Abs(_averageZRotation));
        
        if (_averageZRotation < 0)
        {
            _averageZRotation *= -1;
        }*/
        
        print($"x: {_xPointLateralView} y:{_yPointAPView} length: {Mathf.InverseLerp(strutMinLength, strutMaxLength, _averageInitialLength)} z rotation: {_averageZRotation}");
    }
    
/// <summary>
/// Validate that all values not overflowed
/// </summary>
/// <param name="finalTranslation"></param>
/// <param name="finalRotation"></param>
/// <param name="finalLateral"></param>
/// <param name="finalAP"></param>
    private void ValidateValues(
        ref float finalTranslation,
        ref float finalRotation,
        ref float finalLateral,
        ref float finalAP)
    {
        // Length
        if (finalTranslation > strutMaxLength - _averageInitialLength)
        {
            finalTranslation = strutMaxLength - _averageInitialLength;
        }
        
        else if (finalTranslation < strutMinLength - _averageInitialLength)
        {
            finalTranslation = strutMinLength - _averageInitialLength;
        }
        
        // Rotation Z
        if (finalRotation > 90 - _averageZRotation)
            finalTranslation = 90 - _averageZRotation;

        else if (finalTranslation < -90 - _averageZRotation)
            finalTranslation = -90 - _averageZRotation;

        // Rotation Lateral
        if (finalLateral > 45 - _xPointLateralView)
            finalLateral = 45 - _xPointLateralView;
        
        else if (finalLateral < -45 - _xPointLateralView)
            finalLateral = -45 - _xPointLateralView;
        
        // Rotation AP
        if (finalAP > 45 - _yPointAPView)
            finalAP = 45 - _yPointAPView;
        
        else if (finalAP < -45 - _yPointAPView)
            finalAP = -45 - _yPointAPView;
    }
}

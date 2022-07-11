using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class StrutDetail
{
    public float distance;
    public Transform lowerTracker;
    public Transform upperTracker;

    public StrutDetail(Transform lowerTracker, Transform upperTracker)
    {
        this.lowerTracker = lowerTracker;
        this.upperTracker = upperTracker;
    }

    public float GetLength()
    {
        distance = Vector3.Distance(lowerTracker.position, upperTracker.position);
        return distance;
    }
}

public class ProcessOutput : MonoBehaviour
{
    [SerializeField]
    private Button checkButton;

    [Header("Animators")]
    [SerializeField]
    private Animator initialAnimator;
    [SerializeField]
    private Animator finalAnimator;
    [SerializeField]
    private Animator outputAnimator;

    [Header("Initial Upper")]
    [SerializeField]
    private Transform[] initialArmsUpper;

    [Header("Initial Lower")]
    [SerializeField]
    private Transform[] initialArmsLower;

    [Header("Final Upper")]
    [SerializeField]
    private Transform[] finalArmsUpper;

    [Header("Final Lower")] [SerializeField]
    private Transform[] finalArmsLower;

    /*
     * 0 -> L0 - U0 
     * 1 -> U0 - L1
     * 2 -> L1 - U1
     * 3 -> U1 - L2
     * 4 -> L2 - U2
     * 5 -> U2 - L0
     */

    private StrutDetail[] finalStrutDetails = new StrutDetail[6];
    private StrutDetail[] initialStrutDetails = new StrutDetail[6];

    [SerializeField]
    private TextMeshProUGUI[] initialLengthsHolder = new TextMeshProUGUI[6];
    //private float[] initialLengths = new float[6];
    private float[] initialLengthsMin = new float[6];
    private float[] initialLengthsMax = new float[6];


    [SerializeField]
    private TextMeshProUGUI[] finalLengthsHolder = new TextMeshProUGUI[6];
    //private float[] finalLengths = new float[6];
    private float[] finalLengthsMin = new float[6];
    private float[] finalLengthsMax = new float[6];

    [SerializeField]
    private Scrollbar outputScrollbar;

    #region Coordinates
    
    [Header("Coordinates")]
    [SerializeField]
    private TextMeshProUGUI[] columnAInitial1 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnAInitial2 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnAInitial3 = new TextMeshProUGUI[3];

    [SerializeField]
    private TextMeshProUGUI[] columnBInitial1 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnBInitial2 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnBInitial3 = new TextMeshProUGUI[3];

    [SerializeField]
    private TextMeshProUGUI[] columnAFinal1 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnAFinal2 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnAFinal3 = new TextMeshProUGUI[3];

    [SerializeField]
    private TextMeshProUGUI[] columnBFinal1 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnBFinal2 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnBFinal3 = new TextMeshProUGUI[3];

    [SerializeField]
    private TextMeshProUGUI[] columnAInitial4 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnAInitial5 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnAInitial6 = new TextMeshProUGUI[3];

    [SerializeField]
    private TextMeshProUGUI[] columnBInitial4 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnBInitial5 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnBInitial6 = new TextMeshProUGUI[3];

    [SerializeField]
    private TextMeshProUGUI[] columnAFinal4 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnAFinal5 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnAFinal6 = new TextMeshProUGUI[3];

    [SerializeField]
    private TextMeshProUGUI[] columnBFinal4 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnBFinal5 = new TextMeshProUGUI[3];
    [SerializeField]
    private TextMeshProUGUI[] columnBFinal6 = new TextMeshProUGUI[3];

    #endregion
    
    [Header("Tibia")]
    [SerializeField]
    private GameObject tibiaTop;
    [SerializeField]
    private GameObject tibiaBottom;

    private float minStrut = 116; //202.187f;
    private float maxStrut = 178; //252.881f;

    private float topTibiaMax = 1.981039f;
    private float topTibiaMin = 2.445129f;

    private float bottomTibiaMax = 0.00163f;
    private float bottomTibiaMin =  0.00204f;

    private float minRingDistance = 33.716f;
    private float maxRingDistance = 111.878f;

    private float outputRingDistanceInitial = 0.5f;
    private float outputRingDistanceFinal = 0.5f;
    private float outputTranslationInitial = 0;
    private float outputTranslationFinal = 0;
    private float outputRotationInitial = 0;
    private float outputRotationFinal = 0;
    private float outputLateralInitial = 0;
    private float outputLateralFinal = 0;
    private float outputAPInitial = 0;
    private float outputAPFinal = 0;

    private float outputShiftUpper = 100;
    private float outputShiftLower = 50;
    private float outputTransformShiftUpper = 0;
    private float outputTransformShiftLower = -1;

    public TMP_InputField[] InitialLengthField { get; set; } = new TMP_InputField[6];

    // Start is called before the first frame update
    void Start()
    {
        checkButton.interactable = false;

        columnAFinal1[0].text = "";
        columnAFinal1[1].text = "";
        columnAFinal1[2].text = "";

        columnBFinal1[0].text = "";
        columnBFinal1[1].text = "";
        columnBFinal1[2].text = "";
        
        /**/
        
        columnAFinal2[0].text = "";
        columnAFinal2[1].text = "";
        columnAFinal2[2].text = "";

        columnBFinal2[0].text = "";
        columnBFinal2[1].text = "";
        columnBFinal2[2].text = "";
        
        /**/
        
        columnAFinal3[0].text = "";
        columnAFinal3[1].text = "";
        columnAFinal3[2].text = "";

        columnBFinal3[0].text = "";
        columnBFinal3[1].text = "";
        columnBFinal3[2].text = "";
        
        /**/
        
        columnAFinal4[0].text = "";
        columnAFinal4[1].text = "";
        columnAFinal4[2].text = "";

        columnBFinal4[0].text = "";
        columnBFinal4[1].text = "";
        columnBFinal4[2].text = "";
        
        /**/
        
        columnAFinal5[0].text = "";
        columnAFinal5[1].text = "";
        columnAFinal5[2].text = "";

        columnBFinal5[0].text = "";
        columnBFinal5[1].text = "";
        columnBFinal5[2].text = "";
        
        /**/
        
        columnAFinal6[0].text = "";
        columnAFinal6[1].text = "";
        columnAFinal6[2].text = "";

        columnBFinal6[0].text = "";
        columnBFinal6[1].text = "";
        columnBFinal6[2].text = "";
        
        for (var i = 0; i < finalLengthsHolder.Length; i++)
        {
            finalLengthsHolder[i].text = "";
        }
        
        StartCoroutine(PrepareStruts());
    }

    /// <summary>
    /// Check length of each strut from initial model and final model
    /// </summary>
    private void CheckLength()
    {
        //GameObject debugCubePrimitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        for (int i = 0; i < InitialLengthField.Length; i++)
        {
            /*GameObject debugCube = GameObject.Instantiate(debugCubePrimitive,
                Vector3.Lerp(finalStrutDetails[i].lowerTracker.position, finalStrutDetails[i].upperTracker.position, .5f),
                new Quaternion());

            debugCube.transform.localScale *= .2f;
            debugCube.transform.name = i.ToString();*/
            
            //float currentDistance = Vector3.Distance(initialArmsLower[(i / 2) % (initialLengthsHolder.Length / 2)].position, initialArmsUpper[((i / 2) + (i % 2)) % (initialLengthsHolder.Length / 2)].position);
            float currentPercentage = Mathf.InverseLerp(initialLengthsMin[i], initialLengthsMax[i], initialStrutDetails[i].GetLength());

            InitialLengthField[i].text = Mathf.Lerp(minStrut, maxStrut, currentPercentage).ToString("F1");
        }
        
        for (int i = 0; i < finalStrutDetails.Length; i++)
        {
            //float currentDistance = Vector3.Distance(finalArmsLower[(i / 2) % (finalLengthsHolder.Length / 2)].position, finalArmsUpper[((i / 2) + (i % 2)) % (finalLengthsHolder.Length / 2)].position);
            float currentPercentage = Mathf.InverseLerp(finalLengthsMin[i], finalLengthsMax[i], finalStrutDetails[i].GetLength());

            finalLengthsHolder[i].text = Mathf.Lerp(minStrut, maxStrut, currentPercentage).ToString("F1") + " mm";
        }
    }
    
    private void CheckInitialLength()
    {
        //GameObject debugCubePrimitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
        
        for (int i = 0; i < InitialLengthField.Length; i++)
        {
            /*GameObject debugCube = GameObject.Instantiate(debugCubePrimitive,
                Vector3.Lerp(finalStrutDetails[i].lowerTracker.position, finalStrutDetails[i].upperTracker.position, .5f),
                new Quaternion());

            debugCube.transform.localScale *= .2f;
            debugCube.transform.name = i.ToString();*/
            
            //float currentDistance = Vector3.Distance(initialArmsLower[(i / 2) % (initialLengthsHolder.Length / 2)].position, initialArmsUpper[((i / 2) + (i % 2)) % (initialLengthsHolder.Length / 2)].position);
            float currentPercentage = Mathf.InverseLerp(initialLengthsMin[i], initialLengthsMax[i], initialStrutDetails[i].GetLength());

            InitialLengthField[i].text = Mathf.Lerp(minStrut, maxStrut, currentPercentage).ToString("F1");
        }
    }

    private void CheckCoordinate(bool defaulting = false)
    {
        columnAInitial1[0].text = (initialArmsUpper[0].position.x*100).ToString("F1");
        columnAInitial1[1].text = (initialArmsUpper[0].position.y*100).ToString("F1");
        columnAInitial1[2].text = (initialArmsUpper[0].position.z*100).ToString("F1");

        columnBInitial1[0].text = (100 * initialArmsLower[0].position.x).ToString("F1");
        columnBInitial1[1].text = (100 * initialArmsLower[0].position.y).ToString("F1");
        columnBInitial1[2].text = (100 * initialArmsLower[0].position.z).ToString("F1");

        /*columnAFinal1[0].text = (100 * finalArmsUpper[0].position.x).ToString("F1");
        columnAFinal1[1].text = (100 * finalArmsUpper[0].position.y).ToString("F1");
        columnAFinal1[2].text = (100 * finalArmsUpper[0].position.z).ToString("F1");*/
        
        columnAFinal1[0].text = (100 * initialArmsUpper[0].position.x).ToString("F1");
        columnAFinal1[1].text = (100 * initialArmsUpper[0].position.y).ToString("F1");
        columnAFinal1[2].text = (100 * initialArmsUpper[0].position.z).ToString("F1");

        columnBFinal1[0].text = (100 * finalArmsLower[0].position.x).ToString("F1");
        columnBFinal1[1].text = (100 * finalArmsLower[0].position.y).ToString("F1");
        columnBFinal1[2].text = (100 * finalArmsLower[0].position.z).ToString("F1");

        /************************************************************************/

        columnAInitial2[0].text = (100 * initialArmsUpper[1].position.x).ToString("F1");
        columnAInitial2[1].text = (100 * initialArmsUpper[1].position.y).ToString("F1");
        columnAInitial2[2].text = (100 * initialArmsUpper[1].position.z).ToString("F1");

        columnBInitial2[0].text = (100 * initialArmsLower[1].position.x).ToString("F1");
        columnBInitial2[1].text = (100 * initialArmsLower[1].position.y).ToString("F1");
        columnBInitial2[2].text = (100 * initialArmsLower[1].position.z).ToString("F1");

        /*columnAFinal2[0].text = (100 * finalArmsUpper[1].position.x).ToString("F1");
        columnAFinal2[1].text = (100 * finalArmsUpper[1].position.y).ToString("F1");
        columnAFinal2[2].text = (100 * finalArmsUpper[1].position.z).ToString("F1");*/
        
        columnAFinal2[0].text = (100 * initialArmsUpper[1].position.x).ToString("F1");
        columnAFinal2[1].text = (100 * initialArmsUpper[1].position.y).ToString("F1");
        columnAFinal2[2].text = (100 * initialArmsUpper[1].position.z).ToString("F1");

        columnBFinal2[0].text = (100 * finalArmsLower[1].position.x).ToString("F1");
        columnBFinal2[1].text = (100 * finalArmsLower[1].position.y).ToString("F1");
        columnBFinal2[2].text = (100 * finalArmsLower[1].position.z).ToString("F1");

        /************************************************************************/

        columnAInitial3[0].text = (100 * initialArmsUpper[2].position.x).ToString("F1");
        columnAInitial3[1].text = (100 * initialArmsUpper[2].position.y).ToString("F1");
        columnAInitial3[2].text = (100 * initialArmsUpper[2].position.z).ToString("F1");

        columnBInitial3[0].text = (100 * initialArmsLower[2].position.x).ToString("F1");
        columnBInitial3[1].text = (100 * initialArmsLower[2].position.y).ToString("F1");
        columnBInitial3[2].text = (100 * initialArmsLower[2].position.z).ToString("F1");

        /*columnAFinal3[0].text = (100 * finalArmsUpper[2].position.x).ToString("F1");
        columnAFinal3[1].text = (100 * finalArmsUpper[2].position.y).ToString("F1");
        columnAFinal3[2].text = (100 * finalArmsUpper[2].position.z).ToString("F1");*/
        
        columnAFinal3[0].text = (100 * initialArmsUpper[2].position.x).ToString("F1");
        columnAFinal3[1].text = (100 * initialArmsUpper[2].position.y).ToString("F1");
        columnAFinal3[2].text = (100 * initialArmsUpper[2].position.z).ToString("F1");

        columnBFinal3[0].text = (100 * finalArmsLower[2].position.x).ToString("F1");
        columnBFinal3[1].text = (100 * finalArmsLower[2].position.y).ToString("F1");
        columnBFinal3[2].text = (100 * finalArmsLower[2].position.z).ToString("F1");

        /************************************************************************/

        columnAInitial4[0].text = (100 * initialArmsUpper[3].position.x).ToString("F1");
        columnAInitial4[1].text = (100 * initialArmsUpper[3].position.y).ToString("F1");
        columnAInitial4[2].text = (100 * initialArmsUpper[3].position.z).ToString("F1");

        columnBInitial4[0].text = (100 * initialArmsLower[3].position.x).ToString("F1");
        columnBInitial4[1].text = (100 * initialArmsLower[3].position.y).ToString("F1");
        columnBInitial4[2].text = (100 * initialArmsLower[3].position.z).ToString("F1");

        /*columnAFinal4[0].text = (100 * finalArmsUpper[3].position.x).ToString("F1");
        columnAFinal4[1].text = (100 * finalArmsUpper[3].position.y).ToString("F1");
        columnAFinal4[2].text = (100 * finalArmsUpper[3].position.z).ToString("F1");*/
        
        columnAFinal4[0].text = (100 * initialArmsUpper[3].position.x).ToString("F1");
        columnAFinal4[1].text = (100 * initialArmsUpper[3].position.y).ToString("F1");
        columnAFinal4[2].text = (100 * initialArmsUpper[3].position.z).ToString("F1");

        columnBFinal4[0].text = (100 * finalArmsLower[3].position.x).ToString("F1");
        columnBFinal4[1].text = (100 * finalArmsLower[3].position.y).ToString("F1");
        columnBFinal4[2].text = (100 * finalArmsLower[3].position.z).ToString("F1");

        /************************************************************************/

        columnAInitial5[0].text = (100 * initialArmsUpper[4].position.x).ToString("F1");
        columnAInitial5[1].text = (100 * initialArmsUpper[4].position.y).ToString("F1");
        columnAInitial5[2].text = (100 * initialArmsUpper[4].position.z).ToString("F1");

        columnBInitial5[0].text = (100 * initialArmsLower[4].position.x).ToString("F1");
        columnBInitial5[1].text = (100 * initialArmsLower[4].position.y).ToString("F1");
        columnBInitial5[2].text = (100 * initialArmsLower[4].position.z).ToString("F1");

        /*columnAFinal5[0].text = (100 * finalArmsUpper[4].position.x).ToString("F1");
        columnAFinal5[1].text = (100 * finalArmsUpper[4].position.y).ToString("F1");
        columnAFinal5[2].text = (100 * finalArmsUpper[4].position.z).ToString("F1");*/
        
        columnAFinal5[0].text = (100 * initialArmsUpper[4].position.x).ToString("F1");
        columnAFinal5[1].text = (100 * initialArmsUpper[4].position.y).ToString("F1");
        columnAFinal5[2].text = (100 * initialArmsUpper[4].position.z).ToString("F1");

        columnBFinal5[0].text = (100 * finalArmsLower[4].position.x).ToString("F1");
        columnBFinal5[1].text = (100 * finalArmsLower[4].position.y).ToString("F1");
        columnBFinal5[2].text = (100 * finalArmsLower[4].position.z).ToString("F1");

        /************************************************************************/

        columnAInitial6[0].text = (100 * initialArmsUpper[5].position.x).ToString("F1");
        columnAInitial6[1].text = (100 * initialArmsUpper[5].position.y).ToString("F1");
        columnAInitial6[2].text = (100 * initialArmsUpper[5].position.z).ToString("F1");

        columnBInitial6[0].text = (100 * initialArmsLower[5].position.x).ToString("F1");
        columnBInitial6[1].text = (100 * initialArmsLower[5].position.y).ToString("F1");
        columnBInitial6[2].text = (100 * initialArmsLower[5].position.z).ToString("F1");

        /*columnAFinal6[0].text = (100 * finalArmsUpper[5].position.x).ToString("F1");
        columnAFinal6[1].text = (100 * finalArmsUpper[5].position.y).ToString("F1");
        columnAFinal6[2].text = (100 * finalArmsUpper[5].position.z).ToString("F1");*/
        
        columnAFinal6[0].text = (100 * initialArmsUpper[5].position.x).ToString("F1");
        columnAFinal6[1].text = (100 * initialArmsUpper[5].position.y).ToString("F1");
        columnAFinal6[2].text = (100 * initialArmsUpper[5].position.z).ToString("F1");

        columnBFinal6[0].text = (100 * finalArmsLower[5].position.x).ToString("F1");
        columnBFinal6[1].text = (100 * finalArmsLower[5].position.y).ToString("F1");
        columnBFinal6[2].text = (100 * finalArmsLower[5].position.z).ToString("F1");
        
        /************************************************************************/

        #region Defaulting

        if (defaulting)
        {
            columnAFinal1[0].text = "";
            columnAFinal1[1].text = "";
            columnAFinal1[2].text = "";

            columnBFinal1[0].text = "";
            columnBFinal1[1].text = "";
            columnBFinal1[2].text = "";
        
            /**/
        
            columnAFinal2[0].text = "";
            columnAFinal2[1].text = "";
            columnAFinal2[2].text = "";

            columnBFinal2[0].text = "";
            columnBFinal2[1].text = "";
            columnBFinal2[2].text = "";
        
            /**/
        
            columnAFinal3[0].text = "";
            columnAFinal3[1].text = "";
            columnAFinal3[2].text = "";

            columnBFinal3[0].text = "";
            columnBFinal3[1].text = "";
            columnBFinal3[2].text = "";
        
            /**/
        
            columnAFinal4[0].text = "";
            columnAFinal4[1].text = "";
            columnAFinal4[2].text = "";

            columnBFinal4[0].text = "";
            columnBFinal4[1].text = "";
            columnBFinal4[2].text = "";
        
            /**/
        
            columnAFinal5[0].text = "";
            columnAFinal5[1].text = "";
            columnAFinal5[2].text = "";

            columnBFinal5[0].text = "";
            columnBFinal5[1].text = "";
            columnBFinal5[2].text = "";
        
            /**/
        
            columnAFinal6[0].text = "";
            columnAFinal6[1].text = "";
            columnAFinal6[2].text = "";

            columnBFinal6[0].text = "";
            columnBFinal6[1].text = "";
            columnBFinal6[2].text = "";
            
            for (var i = 0; i < finalLengthsHolder.Length; i++)
            {
                finalLengthsHolder[i].text = "";
            }
        }

        #endregion
    }    
    
    private void CheckInitialCoordinate()
    {
        columnAInitial1[0].text = (initialArmsUpper[0].position.x*100).ToString("F1");
        columnAInitial1[1].text = (initialArmsUpper[0].position.y*100).ToString("F1");
        columnAInitial1[2].text = (initialArmsUpper[0].position.z*100).ToString("F1");

        columnBInitial1[0].text = (100 * initialArmsLower[0].position.x).ToString("F1");
        columnBInitial1[1].text = (100 * initialArmsLower[0].position.y).ToString("F1");
        columnBInitial1[2].text = (100 * initialArmsLower[0].position.z).ToString("F1");

        /************************************************************************/

        columnAInitial2[0].text = (100 * initialArmsUpper[1].position.x).ToString("F1");
        columnAInitial2[1].text = (100 * initialArmsUpper[1].position.y).ToString("F1");
        columnAInitial2[2].text = (100 * initialArmsUpper[1].position.z).ToString("F1");

        columnBInitial2[0].text = (100 * initialArmsLower[1].position.x).ToString("F1");
        columnBInitial2[1].text = (100 * initialArmsLower[1].position.y).ToString("F1");
        columnBInitial2[2].text = (100 * initialArmsLower[1].position.z).ToString("F1");
        
        /************************************************************************/

        columnAInitial3[0].text = (100 * initialArmsUpper[2].position.x).ToString("F1");
        columnAInitial3[1].text = (100 * initialArmsUpper[2].position.y).ToString("F1");
        columnAInitial3[2].text = (100 * initialArmsUpper[2].position.z).ToString("F1");

        columnBInitial3[0].text = (100 * initialArmsLower[2].position.x).ToString("F1");
        columnBInitial3[1].text = (100 * initialArmsLower[2].position.y).ToString("F1");
        columnBInitial3[2].text = (100 * initialArmsLower[2].position.z).ToString("F1");

        /************************************************************************/

        columnAInitial4[0].text = (100 * initialArmsUpper[3].position.x).ToString("F1");
        columnAInitial4[1].text = (100 * initialArmsUpper[3].position.y).ToString("F1");
        columnAInitial4[2].text = (100 * initialArmsUpper[3].position.z).ToString("F1");

        columnBInitial4[0].text = (100 * initialArmsLower[3].position.x).ToString("F1");
        columnBInitial4[1].text = (100 * initialArmsLower[3].position.y).ToString("F1");
        columnBInitial4[2].text = (100 * initialArmsLower[3].position.z).ToString("F1");

        /************************************************************************/

        columnAInitial5[0].text = (100 * initialArmsUpper[4].position.x).ToString("F1");
        columnAInitial5[1].text = (100 * initialArmsUpper[4].position.y).ToString("F1");
        columnAInitial5[2].text = (100 * initialArmsUpper[4].position.z).ToString("F1");

        columnBInitial5[0].text = (100 * initialArmsLower[4].position.x).ToString("F1");
        columnBInitial5[1].text = (100 * initialArmsLower[4].position.y).ToString("F1");
        columnBInitial5[2].text = (100 * initialArmsLower[4].position.z).ToString("F1");

        /************************************************************************/

        columnAInitial6[0].text = (100 * initialArmsUpper[5].position.x).ToString("F1");
        columnAInitial6[1].text = (100 * initialArmsUpper[5].position.y).ToString("F1");
        columnAInitial6[2].text = (100 * initialArmsUpper[5].position.z).ToString("F1");

        columnBInitial6[0].text = (100 * initialArmsLower[5].position.x).ToString("F1");
        columnBInitial6[1].text = (100 * initialArmsLower[5].position.y).ToString("F1");
        columnBInitial6[2].text = (100 * initialArmsLower[5].position.z).ToString("F1");
        

    }

    /// <summary>
    /// Receive parameters from parameter processor
    /// </summary>
    /// <param name="initialTranslation"></param>
    /// <param name="initialRotation"></param>
    /// <param name="initialLateralView"></param>
    /// <param name="initialAPView"></param>
    /// <param name="finalTranslation"></param>
    /// <param name="finalRotation"></param>
    /// <param name="finalLateralView"></param>
    /// <param name="finalAPView"></param>
    [Obsolete]
    public IEnumerator SubmitParameter(float initialTranslation, float initialRotation, float initialLateralView, float initialAPView, float finalTranslation, float finalRotation, float finalLateralView, float finalAPView)
    {
        checkButton.interactable = false;

        outputTranslationInitial = Mathf.InverseLerp(52, 0, initialTranslation);
        initialAnimator.SetFloat("Position", outputTranslationInitial);

        outputRotationInitial = Mathf.InverseLerp(0, 90, initialRotation);
        initialAnimator.SetFloat("RotationZ", outputRotationInitial);

        outputLateralInitial = Mathf.InverseLerp(0, initialLateralView > 0 ? 45 : -45, initialLateralView);
        initialAnimator.SetFloat(initialLateralView > 0 ? "RotationXZPositive" : "RotationXZNegative", outputLateralInitial);
        initialAnimator.SetFloat(initialLateralView > 0 ? "RotationXZNegative" : "RotationXZPositive", 0);
        outputLateralInitial *= initialLateralView > 0 ? 1 : -1;

        outputAPInitial = Mathf.InverseLerp(0, initialAPView > 0 ? 45 : -45, initialAPView);
        initialAnimator.SetFloat(initialAPView > 0 ? "RotationYZPositive" : "RotationYZNegative", outputAPInitial);
        initialAnimator.SetFloat(initialAPView > 0 ? "RotationYZNegative" : "RotationYZPositive", 0);
        outputAPInitial *= initialAPView > 0 ? 1 : -1;

        outputTranslationFinal = Mathf.InverseLerp(52, 0, finalTranslation);
        finalAnimator.SetFloat("Position", outputTranslationFinal);

        outputRotationFinal = Mathf.InverseLerp(0, 90, finalRotation);
        finalAnimator.SetFloat("RotationZ", outputRotationFinal);

        outputLateralFinal = Mathf.InverseLerp(0, finalLateralView > 0 ? 45 : -45, finalLateralView);
        finalAnimator.SetFloat(finalLateralView > 0 ? "RotationXZPositive" : "RotationXZNegative", outputLateralFinal);
        finalAnimator.SetFloat(finalLateralView > 0 ? "RotationXZNegative" : "RotationXZPositive", 0);
        outputLateralFinal *= finalLateralView > 0 ? 1 : -1;

        outputAPFinal = Mathf.InverseLerp(0, finalAPView > 0 ? 45 : -45, finalAPView);
        finalAnimator.SetFloat(finalAPView > 0 ? "RotationYZPositive" : "RotationYZNegative", outputAPFinal);
        finalAnimator.SetFloat(finalAPView > 0 ? "RotationYZNegative" : "RotationYZPositive", 0);
        outputAPFinal *= finalAPView > 0 ? 1 : -1;

        yield return new WaitForSecondsRealtime(2f);

        CheckLength();
        CheckCoordinate();

        checkButton.interactable = true;
    }

    /// <summary>
    /// Receive parameters from parameter processors using new calculations
    /// </summary>
    /// <param name="initialTranslation"></param>
    /// <param name="initialRotation"></param>
    /// <param name="initialLateralView"></param>
    /// <param name="initialAPView"></param>
    /// <param name="initialRingDistance"></param>
    /// <param name="finalTranslation"></param>
    /// <param name="finalRotation"></param>
    /// <param name="finalLateralView"></param>
    /// <param name="finalAPView"></param>
    /// <param name="finalRingDistance"></param>
    /// <returns></returns>
    public IEnumerator SubmitNewParameter(
        float initialTranslation, 
        float initialRotation, 
        float initialLateralView, 
        float initialAPView, 
        float initialRingDistance,
        float finalTranslation, 
        float finalRotation, 
        float finalLateralView, 
        float finalAPView,
        float finalRingDistance,
        float originPointShift = 0,
        bool defaulting = false)
    {

        //minStrut = Mathf.Lerp(/*Absolute min length*/ 50, /*Absolute max length*/ 250, Mathf.InverseLerp(0, 52, initialTranslation));
        //print($"min strut: {minStrut}");
            
        checkButton.interactable = false;

        outputLateralInitial = Mathf.InverseLerp(0, 45, Mathf.Abs(initialLateralView));
        outputLateralInitial *= initialLateralView >= 0 ? 1 : -1;
        initialAnimator.SetFloat("RotationXZPositive", outputLateralInitial);

        outputAPInitial = Mathf.InverseLerp(0, 45, Mathf.Abs(initialAPView));
        outputAPInitial *= initialAPView >= 0 ? 1 : -1;
        initialAnimator.SetFloat("RotationYZPositive", outputAPInitial);

        outputTranslationInitial = Mathf.InverseLerp(0, 52, initialTranslation);
        initialAnimator.SetFloat("Position", outputTranslationInitial);

        outputRotationInitial = Mathf.InverseLerp(0, 90, Mathf.Abs(initialRotation));
        outputRotationInitial *= initialRotation >= 0 ? 1 : -1;
        initialAnimator.SetFloat("RotationZ", outputRotationInitial);

        outputRingDistanceInitial = Mathf.InverseLerp(minRingDistance, maxRingDistance, initialRingDistance);
        initialAnimator.SetFloat("DistanceFromRing", outputRingDistanceInitial);

        outputLateralFinal = Mathf.InverseLerp(0, 45/*finalLateralView > 0 ? 45 : -45*/, Mathf.Abs(finalLateralView));
        outputLateralFinal *= finalLateralView >= 0 ? 1 : -1;
        finalAnimator.SetFloat("RotationXZPositive", outputLateralFinal);

        outputAPFinal = Mathf.InverseLerp(0, 45/*finalAPView > 0 ? 45 : -45*/, Mathf.Abs(finalAPView));
        outputAPFinal *= finalAPView >= 0 ? 1 : -1;
        finalAnimator.SetFloat("RotationYZPositive", outputAPFinal);

        outputTranslationFinal = Mathf.InverseLerp(0, 52, finalTranslation);
        finalAnimator.SetFloat("Position", outputTranslationFinal);

        outputRotationFinal = Mathf.InverseLerp(0, 90, Math.Abs(finalRotation));
        outputRotationFinal *= finalRotation >= 0 ? 1 : -1;
        finalAnimator.SetFloat("RotationZ", outputRotationFinal);

        outputRingDistanceFinal = Mathf.InverseLerp(minRingDistance, maxRingDistance, finalRingDistance);
        finalAnimator.SetFloat("DistanceFromRing", outputRingDistanceFinal);

        finalAnimator.gameObject.transform.localPosition = Vector3.up * Mathf.Lerp(outputTransformShiftUpper, outputTransformShiftLower, Mathf.InverseLerp(outputShiftUpper, outputShiftLower, originPointShift));

        yield return new WaitForSecondsRealtime(2f);

        CheckLength();
        CheckCoordinate(defaulting);
        SetOutput();

        checkButton.interactable = true;
    }

    public void SetOutput()
    {
        outputAnimator.SetFloat("Position", Mathf.Lerp(outputTranslationInitial, outputTranslationFinal, outputScrollbar.value));
        outputAnimator.SetFloat("RotationZ", Mathf.Lerp(outputRotationInitial, outputRotationFinal, outputScrollbar.value));
        outputAnimator.SetFloat("DistanceFromRing", Mathf.Lerp(outputRingDistanceInitial, outputRingDistanceFinal, outputScrollbar.value));
/*
        if(outputLateralInitial > 0)
        {
            outputAnimator.SetFloat("RotationXZNegative", 0);
            outputAnimator.SetFloat("RotationXZPositive", Mathf.Lerp(outputLateralInitial, outputLateralFinal, outputScrollbar.value));
        }
*/

        float outputLateralVal = Mathf.Lerp(outputLateralInitial, outputLateralFinal, outputScrollbar.value);
        //outputAnimator.SetFloat(outputLateralVal > 0 ? "RotationXZPositive" : "RotationXZNegative", Mathf.Abs(outputLateralVal));
        //outputAnimator.SetFloat(outputLateralVal > 0 ? "RotationXZNegative" : "RotationXZPositive", 0);
        outputAnimator.SetFloat("RotationXZPositive", outputLateralVal);

        float outputAPVal = Mathf.Lerp(outputAPInitial, outputAPFinal, outputScrollbar.value);
        //outputAnimator.SetFloat(outputAPVal > 0 ? "RotationYZPositive" : "RotationYZNegative", Mathf.Abs(outputAPVal));
        //outputAnimator.SetFloat(outputAPVal > 0 ? "RotationYZNegative" : "RotationYZPositive", 0);
        outputAnimator.SetFloat("RotationYZPositive", outputAPVal);

        //outputLateralVal > 0 ? (outputAnimator.SetFloat("", outputLateralVal) : (outputAnimator.SetFloat);

        float outputShift = Mathf.Lerp(initialAnimator.gameObject.transform.position.y,
            finalAnimator.gameObject.transform.position.y, outputScrollbar.value);

        outputAnimator.gameObject.transform.position = new Vector3(outputAnimator.gameObject.transform.position.x,
            outputShift, outputAnimator.gameObject.transform.position.z);
        
        //outputAnimator.SetFloat(output)
        tibiaTop.transform.localPosition = new Vector3(tibiaTop.transform.localPosition.x, Mathf.Lerp(topTibiaMin, topTibiaMax, Mathf.Lerp(outputRingDistanceInitial, outputRingDistanceFinal, outputScrollbar.value)),tibiaTop.transform.localPosition.z);
        tibiaBottom.transform.localPosition = new Vector3(tibiaBottom.transform.localPosition.x, Mathf.Lerp(bottomTibiaMin, bottomTibiaMax, Mathf.Lerp(outputRingDistanceInitial, outputRingDistanceFinal, outputScrollbar.value)),tibiaBottom.transform.localPosition.z);
    }

    /// <summary>
    /// Prepare struts before inputting parameters
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrepareStruts()
    {
        finalStrutDetails[0] = new StrutDetail(finalArmsLower[0], finalArmsUpper[0]);
        finalStrutDetails[1] = new StrutDetail(finalArmsUpper[0], finalArmsLower[1]);
        finalStrutDetails[2] = new StrutDetail(finalArmsLower[1], finalArmsUpper[1]);
        finalStrutDetails[3] = new StrutDetail(finalArmsUpper[1], finalArmsLower[2]);
        finalStrutDetails[4] = new StrutDetail(finalArmsLower[2], finalArmsUpper[2]);
        finalStrutDetails[5] = new StrutDetail(finalArmsUpper[2], finalArmsLower[0]);
        
        initialStrutDetails[0] = new StrutDetail(initialArmsLower[0], initialArmsUpper[0]);
        initialStrutDetails[1] = new StrutDetail(initialArmsUpper[0], initialArmsLower[1]);
        initialStrutDetails[2] = new StrutDetail(initialArmsLower[1], initialArmsUpper[1]);
        initialStrutDetails[3] = new StrutDetail(initialArmsUpper[1], initialArmsLower[2]);
        initialStrutDetails[4] = new StrutDetail(initialArmsLower[2], initialArmsUpper[2]);
        initialStrutDetails[5] = new StrutDetail(initialArmsUpper[2], initialArmsLower[0]);
        
        initialAnimator.SetFloat("Position", 1);
        finalAnimator.SetFloat("Position", 1);

        yield return new WaitForSecondsRealtime(2f);

        for (int i = 0; i < initialStrutDetails.Length; i++)
        {
            initialLengthsMax[i] = initialStrutDetails[i].GetLength();
        }

        for (int i = 0; i < finalStrutDetails.Length; i++)
        {
            finalLengthsMax[i] = finalStrutDetails[i].GetLength();
        }

        initialAnimator.SetFloat("Position", 0);
        finalAnimator.SetFloat("Position", 0);
        //outputAnimator.SetFloat("Position", 0);
        yield return new WaitForSecondsRealtime(2f);
        
        /*for (int i = 0; i < initialLengthsHolder.Length; i++)
        {
            initialLengthsMin[i] = Vector3.Distance(initialArmsLower[(i / 2) % (initialLengthsHolder.Length / 2)].position, initialArmsUpper[((i / 2) + (i % 2)) % (initialLengthsHolder.Length / 2)].position);
        }*/
        
        for (var i = 0; i < initialStrutDetails.Length; i++)
        {
            initialLengthsMin[i] = initialStrutDetails[i].GetLength();
        }

        /*for (int i = 0; i < finalLengthsHolder.Length; i++)
        {
            finalLengthsMin[i] = Vector3.Distance(finalArmsLower[(i / 2) % (finalLengthsHolder.Length / 2)].position, finalArmsUpper[((i / 2) + (i % 2)) % (finalLengthsHolder.Length / 2)].position);
        }*/

        for (var i = 0; i < finalStrutDetails.Length; i++)
        {
            finalLengthsMin[i] = finalStrutDetails[i].GetLength();
        }
        
        initialAnimator.SetFloat("Position", 0.2258f);
        yield return new WaitForSecondsRealtime(2f);
        
        checkButton.interactable = true;
        
        CheckInitialLength();
        CheckInitialCoordinate();
        SetOutput();
    }
}

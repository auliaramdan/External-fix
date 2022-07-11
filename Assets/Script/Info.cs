using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Info : MonoBehaviour
{
    [SerializeField]
    private Transform middlePoint, bottomPoint, topPoint;
    [SerializeField]
    private TextMeshProUGUI angleHolder;
    
    [Header("Initial Upper")]
    [SerializeField]
    private Transform[] initialArmsUpper;

    [Header("Initial Lower")]
    [SerializeField]
    private Transform[] initialArmsLower;
    
    [SerializeField]
    private TextMeshProUGUI[] initialLengthsHolder = new TextMeshProUGUI[6];
    
    private StrutDetail[] initialStrutDetails = new StrutDetail[6];

    private Animator initialAnimator;
    private float[] initialLengthsMax = new float[6];
    private float[] initialLengthsMin = new float[6];

    private bool ready = false;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        initialAnimator = GetComponent<Animator>();
        
        initialStrutDetails[0] = new StrutDetail(initialArmsLower[0], initialArmsUpper[0]);
        initialStrutDetails[1] = new StrutDetail(initialArmsUpper[0], initialArmsLower[1]);
        initialStrutDetails[2] = new StrutDetail(initialArmsLower[1], initialArmsUpper[1]);
        initialStrutDetails[3] = new StrutDetail(initialArmsUpper[1], initialArmsLower[2]);
        initialStrutDetails[4] = new StrutDetail(initialArmsLower[2], initialArmsUpper[2]);
        initialStrutDetails[5] = new StrutDetail(initialArmsUpper[2], initialArmsLower[0]);
        
        initialAnimator.SetFloat("Position", 1);
        
        yield return new WaitForSecondsRealtime(2f);
        
        for (int i = 0; i < initialStrutDetails.Length; i++)
        {
            initialLengthsMax[i] = initialStrutDetails[i].GetLength();
        }
        
        initialAnimator.SetFloat("Position", 0);
        
        yield return new WaitForSecondsRealtime(2f);
        
        for (var i = 0; i < initialStrutDetails.Length; i++)
        {
            initialLengthsMin[i] = initialStrutDetails[i].GetLength();
        }

        ready = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready) return;
        
        /*Vector3 topLine =  middlePoint.position - topPoint.position;
        Vector3 middleLine = bottomPoint.position - middlePoint.position;
        float boneAngle = Vector3.Angle(middleLine, topLine);
        Ray ray1 = new Ray(middlePoint.position, bottomPoint.position);
        Ray ray2 = new Ray(middlePoint.position, topPoint.position);
        Debug.DrawRay(ray1.origin, ray1.direction * 10, Color.red);
        Debug.DrawRay(ray2.origin, ray2.direction * 10, Color.yellow);
        angleHolder.text = "Sudut tulang : " + boneAngle.ToString("F2");*/
        
        for (int i = 0; i < initialLengthsHolder.Length; i++)
        {
            float currentPercentage = Mathf.InverseLerp(initialLengthsMin[i], initialLengthsMax[i], initialStrutDetails[i].GetLength());

            initialLengthsHolder[i].text = Mathf.Lerp(0, 100, currentPercentage).ToString("F3");
        }
    }
}

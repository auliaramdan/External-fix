using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Apparatus : MonoBehaviour
{
    [SerializeField]
    private Animation anim;
    [SerializeField]
    private Scrollbar scrollbar;
    [SerializeField]
    private Scrollbar strutScrollbar;
    [SerializeField]
    private Scrollbar rotationScrollbar;
    [SerializeField]
    private TextMeshProUGUI lengthHolder;
    [SerializeField]
    private float rotateSpeed = 4f;

    [Header("Initial")]
    [SerializeField]
    private TMP_InputField translationInitial;
    [SerializeField]
    private TMP_InputField rotationInitial;
    [SerializeField]
    private TMP_InputField apInitial;
    [SerializeField]
    private TMP_InputField lateralInitial;

    [Header("Final")]
    [SerializeField]
    private TMP_InputField translationFinal;
    [SerializeField]
    private TMP_InputField rotationFinal;
    [SerializeField]
    private TMP_InputField apFinal;
    [SerializeField]
    private TMP_InputField lateralFinal;

    private Animator animator;
    private IApparatusInput apparatusInput;
    private Camera mainCam;
    private Strut arm1L, arm1R, arm2L, arm2R, arm3L, arm3R, armNone, chosenStrut;
    private Vector2 blendPos = Vector2.zero;
    private Transform capTransform;
    private bool isMoving;
    private Vector3 initialPos;
    private bool isDrag;

    //private float initialLength = 202.187f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        mainCam = Camera.main;
        apparatusInput = new MouseInput();
        armNone = new Strut("None", Vector2.zero);
        chosenStrut = armNone;
        capTransform = null;

        arm1L = new Strut("Arm.1.L", new Vector2(0.5f, 1));
        arm1R = new Strut("Arm.1.R", new Vector2(-0.5f, 1));
        arm2L = new Strut("Arm.2.L", new Vector2(1, 0));
        arm2R = new Strut("Arm.2.R", new Vector2(0.5f, -1));
        arm3L = new Strut("Arm.3.L", new Vector2(-1, 0));
        arm3R = new Strut("Arm.3.R", new Vector2(-0.5f, -1));
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Position", scrollbar.value);
        //lengthHolder.text = "Panjang strut: " + Mathf.Abs(1 - scrollbar.value);

        //float apparatusRotation = Mathf.Lerp(0, 180f, rotationScrollbar.value);
        //transform.rotation = Quaternion.Euler(0f, apparatusRotation, 0f);

        if (chosenStrut == armNone)
            strutScrollbar.gameObject.SetActive(false);
        else
        {
            strutScrollbar.gameObject.SetActive(true);
        }

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

    private void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(apparatusInput.RayOrigin);

        if(apparatusInput.Click)
        {
            if (Physics.Raycast(ray, out hit, 10f))
            {
                if (hit.collider.name != chosenStrut.StrutName)
                    chosenStrut.Progress = strutScrollbar.value;

                capTransform = hit.collider.GetComponentInChildren<Cap>().CapTransform;

                //if (hit.collider.tag == "Arm") Debug.Log(hit.collider.name);
                switch (hit.collider.name)
                {
                    case "Arm.1.L":
                        chosenStrut = arm1L;
                        strutScrollbar.value = chosenStrut.Progress;
                        break;
                    case "Arm.1.R":
                        chosenStrut = arm1R;
                        strutScrollbar.value = chosenStrut.Progress;
                        break;
                    case "Arm.2.L":
                        chosenStrut = arm2L;
                        strutScrollbar.value = chosenStrut.Progress;
                        break;
                    case "Arm.2.R":
                        chosenStrut = arm2R;
                        strutScrollbar.value = chosenStrut.Progress;
                        break;
                    case "Arm.3.L":
                        chosenStrut = arm3L;
                        strutScrollbar.value = chosenStrut.Progress;
                        break;
                    case "Arm.3.R":
                        chosenStrut = arm3R;
                        strutScrollbar.value = chosenStrut.Progress;
                        break;
                    default:
                        chosenStrut = armNone;
                        capTransform = null;
                        break;
                }
            }
            //else chosenStrut = armNone;
        }
    }

    public void CheckProgress()
    {
        chosenStrut.Progress = strutScrollbar.value;

        Vector2 averageProgress = Vector2.zero;

        capTransform.localRotation = Quaternion.Euler(0f, Mathf.Lerp(0f, 720f, strutScrollbar.value), 0f);

        averageProgress += Vector2.Lerp(Vector2.zero, arm1L.BlendPos, arm1L.Progress);
        averageProgress += Vector2.Lerp(Vector2.zero, arm1R.BlendPos, arm1R.Progress);
        averageProgress += Vector2.Lerp(Vector2.zero, arm2L.BlendPos, arm2L.Progress);
        averageProgress += Vector2.Lerp(Vector2.zero, arm2R.BlendPos, arm2R.Progress);
        averageProgress += Vector2.Lerp(Vector2.zero, arm3L.BlendPos, arm3L.Progress);
        averageProgress += Vector2.Lerp(Vector2.zero, arm3R.BlendPos, arm3R.Progress);

        averageProgress /= 6;
        averageProgress *= 5;

        animator.SetFloat("RotationXZ", averageProgress.x);
        animator.SetFloat("RotationYZ", averageProgress.y);
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

    public void StartSimulation(float position)
    {
        scrollbar.value = position;
    }
}

public class Strut
{
    public string StrutName { get; private set; }
    public Vector2 BlendPos { get; private set; }
    public float Progress { get; set; }

    public Strut(string name, Vector2 pos)
    {
        StrutName = name;
        BlendPos = pos;
        Progress = 0;
    }
}


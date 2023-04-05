using UnityEngine;

public class BodyController : MonoBehaviour
{
    [Header("Head and body")]
    [SerializeField] public GameObject HeadAndBody;
    [SerializeField] public GameObject Head;
    [SerializeField] public GameObject Body;

    [Header("Looking right")]
    [SerializeField] public GameObject LookingRight;
    [SerializeField] public GameObject LeftHandActiveBackR;
    [SerializeField] public GameObject LeftHandInactiveBackR;
    [SerializeField] public GameObject RightHandActiveFrontR;
    [SerializeField] public GameObject RightHandInactiveFrontR;

    [Header("Renderers")]
    [SerializeField] public Renderer LeftHandActiveBackRRenderer;
    [SerializeField] public Renderer LeftHandInactiveBackRRenderer;
    [SerializeField] public Renderer RightHandActiveFrontRRenderer;
    [SerializeField] public Renderer RightHandInactiveFrontRRenderer;

    [Header("Looking left")]
    [SerializeField] public GameObject LookingLeft;
    [SerializeField] public GameObject LeftHandActiveFrontL;
    [SerializeField] public GameObject LeftHandInactiveFrontL;
    [SerializeField] public GameObject RightHandActiveBackL;
    [SerializeField] public GameObject RightHandInactiveBackL;

    [Header("Renderers")]
    [SerializeField] public Renderer LeftHandActiveFrontLRenderer;
    [SerializeField] public Renderer LeftHandInactiveFrontLRenderer;
    [SerializeField] public Renderer RightHandActiveBackLRenderer;
    [SerializeField] public Renderer RightHandInactiveBackLRenderer;

    [Header("Other")]
    [SerializeField] private AimGun _aimGun;


    private void Start()
    {
        SetStartBodyPartPosition();
    }

    private void SetStartBodyPartPosition()
    {
        HeadAndBody.SetActive(true);
        Head.SetActive(true);
        Body.SetActive(true);

        HeadAndBody.transform.rotation = Quaternion.identity;
        Head.transform.rotation = Quaternion.identity;
        Body.transform.rotation = Quaternion.identity;


        LeftHandActiveBackRRenderer.enabled = true;
        LeftHandInactiveBackRRenderer.enabled = false;
        RightHandActiveFrontRRenderer.enabled = false;
        RightHandInactiveFrontRRenderer.enabled = true;


        LeftHandActiveFrontLRenderer.enabled = false;
        LeftHandInactiveFrontLRenderer.enabled = false;
        RightHandActiveBackLRenderer.enabled = false;
        RightHandInactiveBackLRenderer.enabled = false;

    }
}

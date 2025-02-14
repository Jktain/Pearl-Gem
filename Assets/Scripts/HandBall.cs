using UnityEngine;
using System.Collections;
using TMPro;

public class HandBall : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private AimLine aimLine;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TMP_Text ballsCountTMP;

    [SerializeField] private float launchForce = 10f;
    [SerializeField] private float delayBeforeNewBall = 2f;
    [SerializeField] private int ballsMaxCount = 5;

    private GameObject currentBall;
    private Camera cam;
    private Vector3 launchDirection;
    private int ballsCurrentCount;
    private bool isAiming;
    private bool canThrow = true;

    public Material[] materials;
    public void StartThrowing()
    {
        cam = Camera.main;
        ballsCurrentCount = ballsMaxCount;
        CreateBall();
    }

    private void Update()
    {
        if (!canThrow || currentBall == null) return;

        if (Input.GetMouseButton(0))
        {
            isAiming = true;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
            launchDirection = (worldPos - transform.position).normalized;
            aimLine.ShowTrajectory(transform.position, launchDirection * launchForce);
        }
        else if (Input.GetMouseButtonUp(0) && isAiming) 
        {
            isAiming = false;
            aimLine.HideTrajectory();
            LaunchBall();
            ballsCountTMP.gameObject.SetActive(false);
            ballsCurrentCount--;
        }
    }

    public void CreateBall()
    {
        if(ballsCurrentCount <= 0)
        {
            gameManager.GameOver(true);
            return;
        }
        else
        {
            if (currentBall != null) Destroy(currentBall);

            currentBall = Instantiate(ballPrefab, transform.position, Quaternion.identity, transform.parent);

            MeshRenderer meshRenderer = currentBall.GetComponent<MeshRenderer>();
            meshRenderer.material = materials[Random.Range(0, materials.Length)];

            Rigidbody rb = currentBall.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            ballsCountTMP.text = ballsCurrentCount.ToString();
            ballsCountTMP.gameObject.SetActive(true);

            canThrow = true;

            BallCollision ballScript = currentBall.GetComponent<BallCollision>();
            if (ballScript != null)
            {
                ballScript.SetHandBall(this);
            }
        }
    }

    private void LaunchBall()
    {
        if (currentBall == null) return;

        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.linearVelocity = launchDirection * launchForce;

        canThrow = false;
        StartCoroutine(DeleteBallCoroutine(currentBall));
    }

    private IEnumerator DeleteBallCoroutine(GameObject ball)
    {
        yield return new WaitForSeconds(delayBeforeNewBall);

        if (ball != null)
        {
            Destroy(ball);
            CreateBall();
        }
    }
}

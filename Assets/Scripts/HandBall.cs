using UnityEngine;
using System.Collections;
using TMPro;

public class HandBall : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private TMP_Text ballsCountTMP;
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private AimLine aimLine;
    [SerializeField] private float delayBeforeNewBall = 2f; // Час очікування перед новою кулькою
    [SerializeField] private int ballsMaxCount = 5; // Час очікування перед новою кулькою
    [SerializeField] private GameManager gameManager; // Час очікування перед новою кулькою

    private int ballsCurrentCount;
    private GameObject currentBall;
    private Camera cam;
    private Vector3 launchDirection;
    private bool isAiming;
    private bool canThrow = true; // Чи можна кидати кульку зараз

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

        if (Input.GetMouseButton(0)) // Натискання миші або тачскрін
        {
            isAiming = true;
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;
            Vector3 worldPos = cam.ScreenToWorldPoint(mousePos);
            launchDirection = (worldPos - transform.position).normalized;
            aimLine.ShowTrajectory(transform.position, launchDirection * launchForce);
        }
        else if (Input.GetMouseButtonUp(0) && isAiming) // Відпускання миші
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
            Debug.Log(ballsCurrentCount);

            if (currentBall != null) Destroy(currentBall); // Видаляємо попередню кульку

            currentBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);

            MeshRenderer meshRenderer = currentBall.GetComponent<MeshRenderer>();
            meshRenderer.material = materials[Random.Range(0, materials.Length)];

            Rigidbody rb = currentBall.GetComponent<Rigidbody>();
            rb.isKinematic = true; // Робимо кульку статичною, поки її не запустять

            ballsCountTMP.text = ballsCurrentCount.ToString();
            ballsCountTMP.gameObject.SetActive(true);

            canThrow = true;

            BallCollision ballScript = currentBall.GetComponent<BallCollision>();
            if (ballScript != null)
            {
                ballScript.SetHandBall(this); // Передаємо посилання на HandBall у Ball
            }
        }
    }

    private void LaunchBall()
    {
        if (currentBall == null) return;

        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.isKinematic = false; // Тепер кулька може рухатися
        rb.linearVelocity = launchDirection * launchForce;

        canThrow = false; // Забороняємо новий кидок до завершення
        StartCoroutine(DeleteBallCoroutine(currentBall));
    }

    private IEnumerator DeleteBallCoroutine(GameObject ball)
    {
        yield return new WaitForSeconds(delayBeforeNewBall); // Чекаємо `n` секунд

        if (ball != null)
        {
            Destroy(ball);
            CreateBall();
        }
    }
}

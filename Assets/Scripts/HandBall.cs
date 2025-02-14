using UnityEngine;
using System.Collections;

public class HandBall : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private float launchForce = 10f;
    [SerializeField] private AimLine aimLine;
    [SerializeField] private float delayBeforeNewBall = 2f; // Час очікування перед новою кулькою
    [SerializeField] private int throwCount = 5; // Час очікування перед новою кулькою

    private GameObject currentBall;
    private Camera cam;
    private Vector3 launchDirection;
    private bool isAiming;
    private bool canThrow = true; // Чи можна кидати кульку зараз

    public Material[] materials;
    private void Start()
    {
        cam = Camera.main;
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
        }
    }

    public void CreateBall()
    {
        if (currentBall != null) Destroy(currentBall); // Видаляємо попередню кульку

        currentBall = Instantiate(ballPrefab, transform.position, Quaternion.identity);

        MeshRenderer meshRenderer = currentBall.GetComponent<MeshRenderer>();
        meshRenderer.material = materials[Random.Range(0, materials.Length)];

        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.isKinematic = true; // Робимо кульку статичною, поки її не запустять

        canThrow = true;

        BallCollision ballScript = currentBall.GetComponent<BallCollision>();
        if (ballScript != null)
        {
            ballScript.SetHandBall(this); // Передаємо посилання на HandBall у Ball
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

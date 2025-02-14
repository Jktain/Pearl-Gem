using System.Collections;
using UnityEngine;
using UnityEngine.TextCore;

public class BallCollision : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private Material handBallMaterial;
    private HandBall handBall;

    private void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        handBallMaterial = GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void SetHandBall(HandBall hb)
    {
        handBall = hb;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform layer = other.transform.parent.transform.parent;
        Transform planet = layer.transform.parent;

        Material planetBallMaterial = other.GetComponent<MeshRenderer>().sharedMaterial;

        if (planetBallMaterial == handBallMaterial)
        {
            Destroy(other.transform.parent.gameObject);

            gameManager.StartCheckLayerCoroutine(layer, planet);
        }

        Destroy(gameObject);

        handBall.CreateBall();
    }
}

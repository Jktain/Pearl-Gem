using UnityEngine;
using UnityEngine.TextCore;

public class BallCollision : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private Material handBallMaterial;
    private HandBall handBall;

    private void Start()
    {
        handBallMaterial = GetComponent<MeshRenderer>().sharedMaterial;
    }

    public void SetHandBall(HandBall hb)
    {
        handBall = hb;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform rootObject = other.transform.root;
        Material planetBallMaterial = other.GetComponent<MeshRenderer>().sharedMaterial;

        if (planetBallMaterial == handBallMaterial)
        {
            Destroy(other.transform.parent.gameObject);

            if(rootObject.childCount == 0)
            {
                gameManager.GameOver(false);
            }
        }

        handBall.CreateBall();

        Destroy(gameObject); // Видаляємо кульку
    }
}

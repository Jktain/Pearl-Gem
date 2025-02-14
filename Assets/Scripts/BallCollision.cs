using UnityEngine;

public class BallCollision : MonoBehaviour
{
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
        Material planetBallMaterial = other.gameObject.GetComponent<MeshRenderer>().sharedMaterial;

        if (planetBallMaterial == handBallMaterial)
        {
            Destroy(other.transform.parent.gameObject);
        }

        if (handBall != null)
        {
            handBall.CreateBall(); // Викликаємо створення нової кульки
        }

        Destroy(gameObject); // Видаляємо кульку
    }
}

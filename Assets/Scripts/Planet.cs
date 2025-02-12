using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab; // Префаб кульки
    [SerializeField] private int ballsInLayer = 500; // Кількість кульок на кожному шарі
    [SerializeField] private float planetRadius = 6.2f; // Статичний радіус планети
    [SerializeField] private int layers = 5; // Кількість шарів планети
    [SerializeField] private float[] layerBallSizes;

    private void Start()
    {
        GeneratePlanet();
    }

    private void GeneratePlanet()
    {
        for (int layer = 0; layer < layers; layer++)
        {
            // Створюємо окремий GameObject для кожного шару
            GameObject layerObject = new GameObject($"Layer_{layer + 1}");
            layerObject.transform.SetParent(transform); // Встановлюємо планету як батьківський об'єкт
            layerObject.transform.localPosition = Vector3.zero; // Встановлюємо позицію шару всередині планети


            float ballSize = layerBallSizes[layer];
            float layerRadius = planetRadius - (layer * (ballSize + 0.1f));

            GenerateLayer(layerObject.transform, layerRadius, ballsInLayer, ballSize); // Генеруємо кульки для шару
        }
    }

    private void GenerateLayer(Transform layerParent, float layerRadius, int ballsCount, float ballSize)
    {
        // Використовуємо Фібоначчі спіраль для рівномірного розподілу кульок
        for (int i = 0; i < ballsCount; i++)
        {
            // Формула для вертикального кута
            float phi = Mathf.Acos(1 - 2 * (i + 0.5f) / ballsCount);

            // Формула для горизонтального кута
            float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * i;

            // Перераховуємо сферичні координати в декартову систему координат
            float x = layerRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = layerRadius * Mathf.Cos(phi);
            float z = layerRadius * Mathf.Sin(phi) * Mathf.Sin(theta);

            // Створюємо кульку на відповідній позиції в межах шару
            Vector3 position = new Vector3(x, y, z);

            // Створюємо кульку як дитину шару
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity, layerParent);
            // Задаємо розмір кульки
            ball.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
        }
    }
}

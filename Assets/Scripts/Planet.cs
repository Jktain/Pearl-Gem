using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Planet : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab; // Префаб кульки
    [SerializeField] private int ballsInLayer = 500; // Кількість кульок на кожному шарі
    [SerializeField] private float planetRadius = 6.2f; // Статичний радіус планети
    [SerializeField] private int layers = 5; // Кількість шарів планети
    [SerializeField] private float[] layerBallSizes;
    [SerializeField] private int segmentsCount = 1;
    [SerializeField] private Material[] materials;

    private void Start()
    {
        GeneratePlanet();
    }

    private void GeneratePlanet()
    {
        for (int layer = 0; layer < layers; layer++)
        {
            GameObject layerObject = new GameObject($"Layer_{layer + 1}");

            layerObject.transform.SetParent(transform); // Встановлюємо планету як батьківський об'єкт
            layerObject.transform.localPosition = Vector3.zero; // Встановлюємо позицію шару всередині планети

            GenerateLayer(layerObject.transform, layer); // Генеруємо кульки для шару
        }
    }

    private void GenerateLayer(Transform layerParent, int layer)
    {
        GameObject[] segments = CreateSegments(layerParent);
        int[] segmentLimits = CalculateSegmentLimits();


        float ballSize = layerBallSizes[layer];
        float layerRadius = planetRadius - (layer * (ballSize + 0.1f));// Використовуємо Фібоначчі спіраль для рівномірного розподілу кульок

        for(int i = 0; i < segments.Length; i++)
        {
            for (int j = segmentLimits[i]; j < segmentLimits[i + 1]; j++)
            {
                // Формула для вертикального кута
                float phi = Mathf.Acos(1 - 2 * (j + 0.5f) / ballsInLayer);

                // Формула для горизонтального кута
                float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * j;

                // Перераховуємо сферичні координати в декартову систему координат
                float x = layerRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = layerRadius * Mathf.Cos(phi);
                float z = layerRadius * Mathf.Sin(phi) * Mathf.Sin(theta);

                // Створюємо кульку на відповідній позиції в межах шару
                Vector3 position = new Vector3(x, y, z);

                // Створюємо кульку як дитину шару
                GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity, segments[i].transform);
                // Задаємо розмір кульки
                ball.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
                
                MeshRenderer meshRenderer = ball.GetComponent<MeshRenderer>();
                meshRenderer.material = materials[i % materials.Length];
            }
        }

        
    }

    private GameObject[] CreateSegments(Transform layerParent)
    {
        GameObject[] segments = new GameObject[segmentsCount];

        for (int i = 0; i < segmentsCount; i++)
        {
            segments[i] = new GameObject($"Segment_{i + 1}");

            segments[i].transform.SetParent(layerParent); // Встановлюємо планету як батьківський об'єкт
            segments[i].transform.localPosition = Vector3.zero; // Встановлюємо позицію шару всередині планет
        }

        return segments;
    }

    private int[] CalculateSegmentLimits()
    {
        int[] limits = new int[segmentsCount + 1];
        
        int basePart = ballsInLayer / segmentsCount;
        int remainder = ballsInLayer % segmentsCount;

        for (int i = 0; i <= segmentsCount - remainder; i++)
        {
            limits[i] = basePart * i;// Частини без надбавки
            Debug.Log(limits[i]);
        }

        for (int i = segmentsCount - remainder + 1; i <= segmentsCount; i++)
        {
            limits[i] = limits[i - 1] + basePart + 1; // Частини з надбавкою
            Debug.Log(limits[i]);
        }

        return limits;
    }
}

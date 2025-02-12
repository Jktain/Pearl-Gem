using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab; // ������ ������
    [SerializeField] private int ballsInLayer = 500; // ʳ������ ������ �� ������� ���
    [SerializeField] private float planetRadius = 6.2f; // ��������� ����� �������
    [SerializeField] private int layers = 5; // ʳ������ ���� �������
    [SerializeField] private float[] layerBallSizes;

    private void Start()
    {
        GeneratePlanet();
    }

    private void GeneratePlanet()
    {
        for (int layer = 0; layer < layers; layer++)
        {
            // ��������� ������� GameObject ��� ������� ����
            GameObject layerObject = new GameObject($"Layer_{layer + 1}");
            layerObject.transform.SetParent(transform); // ������������ ������� �� ����������� ��'���
            layerObject.transform.localPosition = Vector3.zero; // ������������ ������� ���� �������� �������


            float ballSize = layerBallSizes[layer];
            float layerRadius = planetRadius - (layer * (ballSize + 0.1f));

            GenerateLayer(layerObject.transform, layerRadius, ballsInLayer, ballSize); // �������� ������ ��� ����
        }
    }

    private void GenerateLayer(Transform layerParent, float layerRadius, int ballsCount, float ballSize)
    {
        // ������������� Գ������� ������ ��� ���������� �������� ������
        for (int i = 0; i < ballsCount; i++)
        {
            // ������� ��� ������������� ����
            float phi = Mathf.Acos(1 - 2 * (i + 0.5f) / ballsCount);

            // ������� ��� ��������������� ����
            float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * i;

            // ������������ ������� ���������� � ��������� ������� ���������
            float x = layerRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
            float y = layerRadius * Mathf.Cos(phi);
            float z = layerRadius * Mathf.Sin(phi) * Mathf.Sin(theta);

            // ��������� ������ �� �������� ������� � ����� ����
            Vector3 position = new Vector3(x, y, z);

            // ��������� ������ �� ������ ����
            GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity, layerParent);
            // ������ ����� ������
            ball.transform.localScale = new Vector3(ballSize, ballSize, ballSize);
        }
    }
}

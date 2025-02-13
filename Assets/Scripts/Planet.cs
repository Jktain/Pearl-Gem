using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Planet : MonoBehaviour
{
    [SerializeField] private int layers = 5;
    [SerializeField] private int ballsInLayer = 500;
    [SerializeField] private int segmentsCount = 1;

    [SerializeField] private float planetRadius = 6.2f;
    [SerializeField] private float[] layerBallSizes;

    [SerializeField] private GameObject ballPrefab;
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

            layerObject.transform.SetParent(transform);
            layerObject.transform.localPosition = Vector3.zero;

            GenerateLayer(layerObject.transform, layer);
        }
    }

    private void GenerateLayer(Transform layerParent, int layer)
    {
        GameObject[] segments = CreateSegments(layerParent);
        int[] segmentLimits = CalculateSegmentLimits();


        float ballSize = layerBallSizes[layer];
        float layerRadius = planetRadius - (layer * (ballSize + 0.1f));

        for(int i = 0; i < segments.Length; i++)
        {
            for (int j = segmentLimits[i]; j < segmentLimits[i + 1]; j++)
            {
                float phi = Mathf.Acos(1 - 2 * (j + 0.5f) / ballsInLayer);
                float theta = Mathf.PI * (1 + Mathf.Sqrt(5)) * j;

                float x = layerRadius * Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = layerRadius * Mathf.Cos(phi);
                float z = layerRadius * Mathf.Sin(phi) * Mathf.Sin(theta);

                Vector3 position = new Vector3(x, y, z);

                GameObject ball = Instantiate(ballPrefab, position, Quaternion.identity, segments[i].transform);

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
            segments[i].transform.SetParent(layerParent);
            segments[i].transform.localPosition = Vector3.zero;
        }

        return segments;
    }

    private int[] CalculateSegmentLimits()
    {
        int[] limits = new int[segmentsCount + 1];
        
        int basePart = ballsInLayer / segmentsCount;
        int remainder = ballsInLayer % segmentsCount;

        for (int i = 0; i < segmentsCount; i++)
        {
            limits[i] = basePart * i;
            if (i < remainder)
            {
                limits[i] += i;
            }
        }

        limits[segmentsCount] = ballsInLayer;

        return limits;
    }
}

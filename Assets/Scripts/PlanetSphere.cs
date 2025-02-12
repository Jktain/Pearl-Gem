using UnityEngine;

public class PlanetSphere : MonoBehaviour
{
    [SerializeField] private int layerNumber;
    [SerializeField] private Color color;

    private void UpdateSizeBasedOnLayer(int layerNumber)
    {
        float newSize = GetSizeForLayer(layerNumber);
        transform.localScale = new Vector3(newSize, newSize, newSize);
    }

    private void UpdateColor(Color newColor)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.sharedMaterial.color = newColor;
        }
    }

    private float GetSizeForLayer(int layerNumber)
    {
        switch (layerNumber)
        {
            case 1: return 0.5f;
            case 2: return 0.6f;
            case 3: return 0.7f;
            default: return 1f;
        }
    }
}

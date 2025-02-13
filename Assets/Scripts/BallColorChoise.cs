using UnityEngine;

public class BallColorChoise : MonoBehaviour
{
    [SerializeField] private Material[] materials;

    void Start()
    {
        ChooseMaterial();
    }

    private void ChooseMaterial()
    {
        MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
        meshRenderer.material = materials[0];
    }
}

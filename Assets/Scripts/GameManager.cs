using UnityEngine;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Planet planet;
    [SerializeField] private HandBall hand;
    [SerializeField] private TMP_Text gameOverText;
    [SerializeField] private TMP_Text ballsCount;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject gameEnv;

    public void StartGame(GameObject panel)
    {
        gameEnv.SetActive(true);
        hand.StartThrowing();
        planet.GeneratePlanet();
        panel.SetActive(false);
    }

    public void GameOver(bool hasPlanetChildren)
    {
        switch(hasPlanetChildren)
        {
            case false:
                gameOverText.text = "You won";
                break;
            case true:
                gameOverText.text = "You lost";
                DestroyAllChildren(planet.transform);
                break;
        }

        ballsCount.gameObject.SetActive(false);
        gameEnv.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    private void DestroyAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void StartCheckLayerCoroutine(Transform layer, Transform planet)
    {
        StartCoroutine(CheckAndDestroyLayer(layer, planet));
    }

    private IEnumerator CheckAndDestroyLayer(Transform layer, Transform planet)
    {
        yield return new WaitForEndOfFrame();

        if (layer != null && layer.childCount == 0)
        {
            Destroy(layer.gameObject);
            StartCoroutine(CheckAndDestroyPlanet(planet));
        }
    }

    private IEnumerator CheckAndDestroyPlanet(Transform planet)
    {
        yield return new WaitForEndOfFrame();

        if (planet != null && planet.childCount == 0)
        {
            GameOver(false);
        }
    }

}

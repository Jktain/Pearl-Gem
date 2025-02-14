using UnityEngine;
using TMPro;

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
}

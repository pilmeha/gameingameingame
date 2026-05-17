using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniZeldaManager : MonoBehaviour
{
    public static MiniZeldaManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void WinGame()
    {
        MusicManager.Instance.PlayHubMusic();

        SceneManager.LoadScene(GameManager.Instance.hubSceneName);
    }

    public void LoseGame()
    {
        MusicManager.Instance.PlayHubMusic();

        SceneManager.LoadScene(GameManager.Instance.hubSceneName);
    }

}
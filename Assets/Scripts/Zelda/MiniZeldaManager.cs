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
        Debug.Log("Boss defeated");

        SceneManager.LoadScene(GameManager.Instance.hubSceneName);
    }

    public void LoseGame()
    {
        SceneManager.LoadScene(GameManager.Instance.hubSceneName);
    }

}
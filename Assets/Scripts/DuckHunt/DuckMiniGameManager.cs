using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DuckMiniGameManager : MonoBehaviour
{
    public static DuckMiniGameManager Instance;

    [SerializeField] private float returnDelay = 3f;

    private void Awake()
    {
        Instance = this;
    }

    public void WinGame()
    {
        StartCoroutine(ReturnToHub());
    }

    public void LoseGame()
    {
        StartCoroutine(ReturnToHub());
    }

    private IEnumerator ReturnToHub()
    {
        yield return new WaitForSeconds(returnDelay);

        MusicManager.Instance.PlayHubMusic();

        SceneManager.LoadScene(GameManager.Instance.hubSceneName);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class StationInteract : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    public void Interact()
    {

        MusicManager.Instance.PauseHubMusic();

        MusicManager.Instance.PlayZeldaMusic();

        SceneManager.LoadScene(sceneToLoad);
    }
}
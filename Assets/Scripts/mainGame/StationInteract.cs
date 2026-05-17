using UnityEngine;
using UnityEngine.SceneManagement;

public class StationInteract : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    public void Interact()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] private string endingSceneName;

    public void Interact()
    {
        if (GameManager.Instance.AllKeysCollected())
        {
            SceneManager.LoadScene(endingSceneName);
        }
        else
        {
            Debug.Log("Need all key parts");
        }
    }
}
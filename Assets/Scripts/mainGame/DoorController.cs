using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    [SerializeField] private string endingSceneName;

    public void TryOpenDoor()
    {
        if (GameManager.Instance.AllKeysCollected())
        {
            SceneManager.LoadScene(endingSceneName);
        }
        else
        {
            Debug.Log("Not enough keys");
        }
    }
}
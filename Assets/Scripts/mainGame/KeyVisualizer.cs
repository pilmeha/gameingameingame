using UnityEngine;

public class KeyVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject duckKeyPiece;
    [SerializeField] private GameObject zeldaKeyPiece;
    [SerializeField] private GameObject pacmanKeyPiece;

    void Update()
    {
        duckKeyPiece.SetActive(GameManager.Instance.duckKey);
        zeldaKeyPiece.SetActive(GameManager.Instance.zeldaKey);
        pacmanKeyPiece.SetActive(GameManager.Instance.pacmanKey);
    }
}
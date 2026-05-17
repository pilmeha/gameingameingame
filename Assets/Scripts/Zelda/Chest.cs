using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private PlayerAttack playerAttack;

    [SerializeField] private GameObject swordObject;

    private bool opened;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (opened) return;

        if (collision.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                opened = true;

                playerAttack.enabled = true;

                swordObject.SetActive(true);

                Debug.Log("Sword obtained");
            }
        }
    }
}
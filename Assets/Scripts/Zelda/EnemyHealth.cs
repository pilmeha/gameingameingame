using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 3;

    [SerializeField] private bool isNPC;
    [SerializeField] private bool isBoss;

    public void TakeDamage(int damage)
    {
        health -= damage;

        Debug.Log(gameObject.name + " took damage");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isNPC)
        {
            GameManager.Instance.GiveZeldaKey();
        }

        if (isBoss)
        {
            MiniZeldaManager.Instance.WinGame();
        }

        Destroy(gameObject);
    }
}
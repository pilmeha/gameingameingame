using UnityEngine;

public class SwordDamage : MonoBehaviour
{
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hit something");

        EnemyHealth enemy = collision.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            Debug.Log("Hit enemy");

            enemy.TakeDamage(damage);
        }
    }
}
using UnityEngine;

public class BossDamage : MonoBehaviour
{
    public int damage = 1;
    public float damageCooldown = 1f;

    private bool canDamage = true;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!canDamage) return;

        PlayerHealth player = collision.collider.GetComponent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);

            StartCoroutine(DamageCooldown());
        }
    }

    System.Collections.IEnumerator DamageCooldown()
    {
        canDamage = false;

        yield return new WaitForSeconds(damageCooldown);

        canDamage = true;
    }
}
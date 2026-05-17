using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject swordHitbox;
    [SerializeField] private float attackDuration = 0.2f;

    private bool isAttacking;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;

        swordHitbox.SetActive(true);

        yield return new WaitForSeconds(attackDuration);

        swordHitbox.SetActive(false);

        isAttacking = false;
    }
}
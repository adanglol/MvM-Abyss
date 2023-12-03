using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    public float attackSpeed = 0.5f;
    public float attackCooldown = 1f;
    public float damage = 10f;

    private bool isAttacking;
    private bool canAttack = true;

    [SerializeField] private GameObject meleeAttack;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1") && canAttack) {
            StartCoroutine(MeleeAttack());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.transform.tag == "Enemy") {
            collision.gameObject.GetComponent<EnemyController>().health -= damage;
        }
    }

    IEnumerator MeleeAttack() {

        isAttacking = true;
        canAttack = false;
        
        meleeAttack.SetActive(true);

        yield return new WaitForSeconds(attackSpeed);

        meleeAttack.SetActive(false);
        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);

        canAttack = true;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    Animator animator;
    NavMeshAgent navMeshAgent;

    public int healthPoints = 1;

    void Start()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();


    }

    // Update is called once per frame
    void Update()
    {
        if(healthPoints <= 0)
        {
            DeathRoutine();
        }
    }

    public void TakeDamange(int damage)
    {
        healthPoints -= damage;
        animator.SetTrigger("isDamaged");
    }

    private void DeathRoutine()
    {
        navMeshAgent.enabled = false;

        animator.SetBool("isDead", true);
        animator.SetBool("isWalking", false);
        animator.SetBool("isAttacking", false);

        Destroy(gameObject, 5f);
    }
}

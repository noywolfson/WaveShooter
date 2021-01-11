using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    [SerializeField] Transform theTarget;
    [SerializeField] float forwardSpeed = 5f;
    [SerializeField] float distanceFromTarget = 1f;

    Animator animator;
    EnemyHealth enemyHealth;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();

        //Need this' otherwise prefabs will not have a target
        if(theTarget == null)
        {
            theTarget = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

        FaceTarget();

        if (enemyHealth.healthPoints > 0)
        {
            // if further than distance, chase, once close attack
            if (Vector3.Distance(theTarget.position, this.transform.position) < distanceFromTarget)
            {
                AttackTarget();
            }
            else
            {
                ChaseTarget();
            }
        }
    }

    void ChaseTarget()
    {
        // Move towards the target
        this.transform.Translate(0, 0, 0.01f * forwardSpeed);

        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);

    }

    void AttackTarget()
    {
        animator.SetBool("isAttacking", true);
        animator.SetBool("isWalking", false);

    }

    void FaceTarget()
    {
        Vector3 targetDirection = theTarget.position - this.transform.position;

        targetDirection.y = 0;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(targetDirection), 0.1f);

    }
}

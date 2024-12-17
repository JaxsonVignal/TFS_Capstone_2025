using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform player;
    public GameObject projectilePrefab;
    public Transform firePoint;

    public float detectionRange = 20f;
    public float rangedAttackRange = 15f;
    public float meleeAttackRange = 5f;
    public float projectileSpeed = 10f;
    public float projectileLifeTime = 5.0f;
    public float meleeDamage = 20f;
    public float attackCooldown = 1f;

    private NavMeshAgent agent;
    private int currentPatrolIndex = 0;
    private float lastAttackTime = 0;

    private enum EnemyState { Patrol, RangedAttack, MeleeAttack }
    private EnemyState currentState;




    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentState = EnemyState.Patrol;
        GoToNextPatrolPoint();

    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            if (distanceToPlayer <= meleeAttackRange)
            {
                SwitchState(EnemyState.MeleeAttack);
            }
            else if (distanceToPlayer <= rangedAttackRange)
            {
                SwitchState(EnemyState.RangedAttack);
            }
            else
            {
                SwitchState(EnemyState.Patrol);
            }
        }
        else
        {
            SwitchState(EnemyState.Patrol);
        }

        HandleState(distanceToPlayer);
    }

    private void SwitchState(EnemyState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            if (currentState == EnemyState.Patrol)
            {
                agent.isStopped = false;
                GoToNextPatrolPoint();
            }
        }
    }

    private void HandleState(float distanceToPlayer)
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.RangedAttack:
                RangedAttack();
                break;
            case EnemyState.MeleeAttack:
                MeleeAttack(distanceToPlayer);
                break;
        }
    }

    private void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    private void RangedAttack()
    {
        agent.isStopped = true;
        FacePlayer();

        if (Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            ShootProjectile();
        }
    }

    private void MeleeAttack(float distanceToPlayer)
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);

        if (distanceToPlayer <= meleeAttackRange && Time.time - lastAttackTime >= attackCooldown)
        {
            lastAttackTime = Time.time;
            PerformMeleeAttack();
        }
    }

    private void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }




    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileManager = projectile.GetComponent<Projectile>();
        if (projectileManager != null)
        {
            projectileManager.speed = projectileSpeed;
            projectileManager.lifeTime = projectileLifeTime;
        }
    }

    private void PerformMeleeAttack()
    {
        Debug.Log("Melee attack performed, dealing " + meleeDamage + " damage.");
    }
}

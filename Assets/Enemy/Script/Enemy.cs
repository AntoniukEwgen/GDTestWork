using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 30f;
    [SerializeField] private float attackCD = 3f;
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float aggroRange = 4f;
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private Image healthBar;

    private GameObject player;
    private NavMeshAgent agent;
    private Animator animator;
    private float timePassed;
    private float newDestinationCD = 0.5f;
    private float maxHealth;
    public bool isDying = false;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        maxHealth = health;
    }

    private void Update()
    {
        if (isDying || player.GetComponent<PlayerHearth>().health <= 0)
        {
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
            return;
        }

        if (timePassed >= attackCD && Vector3.Distance(player.transform.position, transform.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
            agent.isStopped = true;
            timePassed = 0;
        }
        else if (!agent.isStopped)
        {
            agent.isStopped = false;
        }
        timePassed += Time.deltaTime;

        if (newDestinationCD <= 0 && Vector3.Distance(player.transform.position, transform.position) <= aggroRange)
        {
            newDestinationCD = 0.5f;
            agent.SetDestination(player.transform.position);
        }
        newDestinationCD -= Time.deltaTime;
        transform.LookAt(player.transform);
        animator.SetFloat("Speed", agent.velocity.magnitude / agent.speed);
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDying)
        {
            return;
        }

        health -= damageAmount;
        if (health <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Damage");
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    private void Die()
    {
        isDying = true;
        animator.SetTrigger("Death");
        if (player.GetComponent<PlayerHearth>().health < player.GetComponent<PlayerHearth>().maxHealth)
        {
            player.GetComponent<PlayerHearth>().Heal();
        }
        Invoke("DestroyObject", 3f);
        FindObjectOfType<EnemySpawner>().EnemyKilled();
    }

    public void StopMovement() 
    {
        agent.isStopped = true;
    }

    public void ResumeMovement()
    {
        if (!isDying)
        {
            agent.isStopped = false;
        }
    }

    private void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, aggroRange);
    }

    public void AttackPlayer()
    {
        player.GetComponent<PlayerHearth>().TakeDamage(damageAmount);
    }
}

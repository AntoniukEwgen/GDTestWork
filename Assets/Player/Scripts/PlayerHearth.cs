using UnityEngine;
using UnityEngine.UI;

public class PlayerHearth : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private float healAmount = 5f; 

    public float health = 100f;
    public float maxHealth;

    private Animator animator;
    private bool isDying = false;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        maxHealth = health;
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
            animator.SetTrigger("DamagePlayer");
        }
        UpdateHealthBar();
    }

    public void Heal()
    {
        health += healAmount;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = health / maxHealth;
    }

    private void Die()
    {
        animator.SetTrigger("DiePlayer");
    }
}

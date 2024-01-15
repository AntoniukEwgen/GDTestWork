using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    public void AttackPlayer()
    {
        enemy.AttackPlayer();
    }
    public void StopMovement()
    {
        enemy.StopMovement();
    }

    public void ResumeMovement()
    {
        enemy.ResumeMovement();
    }
}

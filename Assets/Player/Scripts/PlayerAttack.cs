using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    bool canDealDamage;
    List<GameObject> hasDealtDamage = new List<GameObject>();

    [SerializeField] float lightDamage;
    [SerializeField] float heavyDamage;
    [SerializeField] float attackDistance;
    [SerializeField] float attackAngle;

    public void DealDamage(float damage)
    {
        canDealDamage = true;
        hasDealtDamage.Clear();

        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackDistance);

        foreach (Collider enemy in hitEnemies)
        {
            Vector3 toEnemy = (enemy.transform.position - transform.position).normalized;
            if (Vector3.Dot(toEnemy, transform.forward) > Mathf.Cos(attackAngle * Mathf.Deg2Rad / 2) && enemy.TryGetComponent(out Enemy enemyComponent) && !hasDealtDamage.Contains(enemy.gameObject))
            {
                enemyComponent.TakeDamage(damage);
                hasDealtDamage.Add(enemy.gameObject);
            }
        }
    }

    public void LightDamage()
    {
        DealDamage(lightDamage);
    }

    public void HeavyDamage()
    {
        DealDamage(heavyDamage);
    }

    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    public bool AreEnemiesInRadius()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, attackDistance);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.TryGetComponent(out Enemy enemyComponent))
            {
                return true;
            }
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -attackAngle / 2, 0) * transform.forward * attackDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, attackAngle / 2, 0) * transform.forward * attackDistance);
    }
}

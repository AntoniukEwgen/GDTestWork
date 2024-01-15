using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject enemyPrefab; 
    public int numberOfEnemies = 4; 

    private Enemy enemyScript;

    private void Start()
    {
        enemyScript = GetComponent<Enemy>();
    }

    private void Update()
    {
        if (enemyScript != null && enemyScript.isDying)
        {
            SpawnEnemies();
            Destroy(this);
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
    }
}

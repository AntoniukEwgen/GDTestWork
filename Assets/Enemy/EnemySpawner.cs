using System.Collections;
using UnityEngine;
using TMPro;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    public int[] waves;
    public float spawnRadius;
    private int currentWave = 0;
    private bool bossSpawned = false;
    private int totalEnemies = 0;
    private int killedEnemies = 0;

    public TextMeshProUGUI waveText;
    public TextMeshProUGUI enemyText; 

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !bossSpawned)
        {
            SpawnBoss();
            bossSpawned = true;
        }
        else if (GameObject.FindGameObjectsWithTag("Boss").Length == 0 && bossSpawned)
        {
            StartCoroutine(SpawnWave());
            bossSpawned = false;
        }

        waveText.text = "Number of waves: " + currentWave + "/" + waves.Length;
        enemyText.text = "Number of enemies: " + killedEnemies + "/" + totalEnemies;
    }

    IEnumerator SpawnWave()
    {
        if (currentWave < waves.Length)
        {
            for (int i = 0; i < waves[currentWave]; i++)
            {
                Vector3 spawnPosition = transform.position + Random.onUnitSphere * spawnRadius;
                spawnPosition.y = transform.position.y;

                Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                totalEnemies++; 
                yield return new WaitForSeconds(0.1f);
            }
            currentWave++;
            killedEnemies = 0;
        }
        else
        {
            Debug.Log("Всі хвилі завершено!");
        }
    }

    void SpawnBoss()
    {
        Instantiate(bossPrefab, transform.position, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }

    public void EnemyKilled() 
    {
        killedEnemies++;
    }
}

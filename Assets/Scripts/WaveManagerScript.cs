using UnityEngine;

public class WaveManagerScript : MonoBehaviour
{
    public bool waveIsActive = false;
    public float spawnInterval = 0.5f;
    public float ratSpawnInterval = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnInterval -= Time.deltaTime;
        ratSpawnInterval -= Time.deltaTime;
        if (spawnInterval <= 0 && waveIsActive) 
        {
            Debug.Log("Spawn Enemy at " + Time.frameCount);
        }
        if (ratSpawnInterval <= 0 && waveIsActive) 
        {
            // Spawn an enemy that goes for one of the machines.
        }
    }

    public void StartWave()
    {
        waveIsActive = true;
    }
    public void EndWave()
    {
        waveIsActive = false;
    }

    public void SpawnEnemy()
    {
        // spawnRandomEnemy
    }
}

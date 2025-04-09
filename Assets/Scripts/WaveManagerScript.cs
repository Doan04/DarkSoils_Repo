using UnityEngine;

public class WaveManagerScript : MonoBehaviour
{
    public GameObject grunt;
    public GameObject shanker;
    public GameObject mouth;
    public bool waveIsActive = false;
    public float spawnInterval = 1f;
    public float ratSpawnInterval = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveIsActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        spawnInterval -= Time.deltaTime;
        ratSpawnInterval -= Time.deltaTime;
        if (spawnInterval <= 0 && waveIsActive) 
        {
            //Debug.Log("Spawn Enemy at " + Time.frameCount);
            SpawnEnemy(grunt);
            SpawnEnemy(shanker);
            SpawnEnemy(mouth);
            spawnInterval = 2f;
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

    public void SpawnEnemy(GameObject enemyType)
    {
        Vector3 spawnCoordinate = ChooseBorder();
        Instantiate(enemyType, spawnCoordinate, Quaternion.Euler(0, 0, 0));

        // spawnRandomEnemy
    }

    public Vector3 ChooseBorder()
    {
        //Wanting the enemy to spawn on the border
        Vector3 leftBorder = new Vector3(Random.Range(-8, -7), Random.Range(-5, 5), 0);
        Vector3 rightBorder = new Vector3(Random.Range(7, 8), Random.Range(-5, 5), 0);
        Vector3 upBorder = new Vector3(Random.Range(-8, 8), Random.Range(4, 5), 0);
        Vector3 downBorder = new Vector3(Random.Range(-8, 8), Random.Range(-5, -4), 0);
        //Choosing one of four borders
        int value = (int)Random.Range(0, 4);
        switch(value)
        {
            case 0:
                return leftBorder;
            case 1:
                return rightBorder;
            case 2:
                return upBorder;
            case 3:
                return downBorder;
            //This should never activate
            default:
                return new Vector3(0, 0, 0);
        }
    }
}

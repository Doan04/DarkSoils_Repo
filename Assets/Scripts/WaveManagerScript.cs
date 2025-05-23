using UnityEngine;

public class WaveManagerScript : MonoBehaviour
{
    public GameObject grunt;
    public GameObject shanker;
    public GameObject mouth;
    public GameObject rat;
    public GameObject coin;
    public GameObject fertilizer;
    public GameObject audioManager;
    public bool waveIsActive = false;
    public float spawnInterval = 2f;
    public float shankerSpawnInterval = 10f;
    public float fertSpawnInterval = 7.5f;
    public float RatSpawnInterval = 20f;
    public int wave;
    float shankSpawnTime = 10f;
    float fertSpawnTime = 7.5f;
    float RatSpawnTime = 20f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveIsActive = false;
        wave = 0;
    }

    // Update is called once per frame
    void Update()
    {
        spawnInterval -= Time.deltaTime;
        shankerSpawnInterval -= Time.deltaTime;
        fertSpawnInterval -= Time.deltaTime;
        RatSpawnInterval -= Time.deltaTime;
        if(fertSpawnInterval < 0  && waveIsActive)
        {
            Instantiate(fertilizer, new Vector3(Random.Range(-8, 8), Random.Range(-5, 5), 0), Quaternion.Euler(0, 0, 0));
            fertSpawnInterval = fertSpawnTime;
        }
        if (spawnInterval <= 0 && waveIsActive)
        {
            SpawnEnemy(grunt);
            spawnInterval = 3f - (0.1f * (wave - 1));
        }
        if (shankerSpawnInterval <= 0 && waveIsActive) 
        {
            // Spawn an enemy that goes for the players.
            SpawnEnemy(shanker);
            SpawnEnemy(mouth);
            shankerSpawnInterval = shankSpawnTime;
        }
        if (RatSpawnInterval <= 0 && waveIsActive) 
        {
            SpawnEnemy(rat);
            Debug.Log("Spawn Rat");
            RatSpawnInterval = RatSpawnTime;
        }
    }

    public void StartWave()
    {
        wave++;
        waveIsActive = true;
        audioManager.GetComponent<MainAudio>().PlayWaveMusic();
    }
    public void EndWave()
    {
        waveIsActive = false;
    }

    public void SpawnEnemy(GameObject enemyType)
    {
        Vector3 spawnCoordinate = ChooseBorder();
        Instantiate(enemyType, spawnCoordinate, Quaternion.Euler(0, 0, 0));
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

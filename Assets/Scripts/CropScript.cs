using UnityEngine;
public class CropScript : MonoBehaviour
{
    public int stage;
    public bool canGrow;
    public bool canHeal;
    public bool waveActive;
    public float maxHealth = 500f;
    public float currentHealth = 500f;
    public float currentGrowth = 0f;
    public float maxGrowth = 100f;
    public float growthInterval = 1f;
    public float HealInterval = 5f;
    public float secondsToNextWave = 120f;
    public float currentSecondsTilWave;
    public ParticleSystem healingVFX;
    public Sprite[] cropStageSprite;
    public SpriteRenderer cropSprite;
    public WaveManagerScript waveManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cropSprite = GetComponent<SpriteRenderer>();
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        growthInterval -= Time.deltaTime;
        HealInterval -= Time.deltaTime;
        if(canGrow && growthInterval <= 0 && waveActive)
        {
            currentGrowth += 1;
            if (currentGrowth >= maxGrowth)
            {
                waveManager.EndWave();
                Debug.Log("Growth reached 100%. End any wave");
            }
        }
        if(canHeal && HealInterval <= 0)
        {
            currentHealth += 5f;

        }
    }

    public void Heal()
    {
        if (canHeal)
        {
            currentHealth += 50f;
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0) 
        {
            Debug.Log("Crop has Died.");
        }
    }

    public void SetGrowth(bool val) { canGrow = val; }
    public void SetHealing(bool val) { canHeal = val; }
}

using TMPro;
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
    public float maxGrowth = 5f;
    public float growthInterval = 1f;
    public float HealInterval = 5f;
    public float secondsToNextWave = 5f;
    public float currentSecondsTilWave;
    public ParticleSystem healingVFX;
    public Sprite[] cropStageSprite;
    public SpriteRenderer cropSprite;
    public WaveManagerScript waveManager;
    public CropBarScript cropBarScript;
    public TMP_Text timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cropSprite = GetComponent<SpriteRenderer>();
        cropSprite.sprite = cropStageSprite[stage];
        maxGrowth = 30f;
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManagerScript>();
        canHeal = true;
        canGrow = true;
        waveActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // increment the growth and heal timers
        growthInterval -= Time.deltaTime;
        HealInterval -= Time.deltaTime;
        
        // grows the crops during an enemy wave and ends the wave once they reach their next stage
        if(canGrow && growthInterval <= 0 && waveActive)
        {
            currentGrowth += 1;
            growthInterval = 1;
            cropBarScript.updateCropValue(currentGrowth / maxGrowth);
            if (currentGrowth >= maxGrowth)
            {
                waveManager.EndWave();
                waveActive = false;
                currentSecondsTilWave = secondsToNextWave;
                currentGrowth = 0;
                Debug.Log("Growth reached 100%. End any wave");
                timerText.enabled = true;
                stage += 1;
                // sets the crop sprites to the next stage in the array
                // if there is not another, then the crops are fully grown
                Debug.Log(cropStageSprite.Length);
                if(stage < cropStageSprite.Length)
                {
                    cropSprite.sprite = cropStageSprite[stage];
                }
                else
                {
                    Debug.Log("Crop fully grown, you win!");
                }
            }
        }
        if(canHeal && HealInterval <= 0)
        {
            currentHealth += 5f;
            HealInterval = 5;
            cropBarScript.updateCropHealthValue(currentHealth / maxHealth);
        }
        // decrements the wave timer if there is no wave active
        if (!waveActive)
        {
            currentSecondsTilWave -= Time.deltaTime;
            timerText.text = "Wave in: " + Mathf.Round(currentSecondsTilWave).ToString();
        }
        
        // starts a new wave when the wave timer runs out
        if(currentSecondsTilWave <= 0 && !waveActive)
        {
            Debug.Log("STARTING NEW WAVE");
            waveActive = true;
            waveManager.StartWave();
            timerText.enabled = false;
            currentSecondsTilWave = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D collider) 
    {
        if (collider.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerScript>().playerInCrops = true;
        }
    }

    void OnTriggerEnterExit(Collider2D collider)
    {
        if (collider.CompareTag("Player")){
            collider.gameObject.GetComponent<PlayerScript>().playerInCrops = false;
        }
    }

    public void Heal()
    {
        if (canHeal)
        {
            currentHealth += 100f;
            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            cropBarScript.updateCropHealthValue(currentHealth / maxHealth);
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        cropBarScript.updateCropHealthValue(currentHealth / maxHealth);
        if (currentHealth <= 0) 
        {
            Debug.Log("Crop has Died.");
        }
    }

    public void SetGrowth(bool val) { canGrow = val; }
    public void SetHealing(bool val) { canHeal = val; }
}

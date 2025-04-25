using UnityEngine;
public class WaterPumpScript : MonoBehaviour
{
    public float maxHealth = 300f;
    public float currentHealth = 300f;
    public AudioSource pumpAudio;
    public AudioClip powerDownNoise;
    public AudioClip powerUpNoise;
    public bool isBroken;
    public ParticleSystem smokeVFX;
    public float damageInterval = 1f;
    public CropScript cropfield;
    public ObjectiveManager questManager;
    void Start()
    {
        cropfield = GameObject.Find("CropField").GetComponent<CropScript>();
    }

    // Update is called once per frame
    void Update()
    {
        damageInterval -= Time.deltaTime;
        if(damageInterval < 0)
        {
            TakeDamage(5);
            damageInterval = 1f;
        }
    }
    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            cropfield.SetHealing(false);
            questManager.EnableWaterQuest();
            //pumpAudio.PlayOneShot(powerDownNoise);
        }
    }
    public void Fix()
    {
        currentHealth = maxHealth;
        cropfield.SetHealing(true);
        questManager.DisableWaterQuest();
        //pumpAudio.PlayOneShot(powerUpNoise);
    }

}

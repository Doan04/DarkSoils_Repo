using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GeneratorScript : MonoBehaviour
{
    public float maxHealth = 300f;
    public float currentHealth = 300f;
    public List<Light2D> lights = new List<Light2D>();
    public AudioSource generatorAudio;
    public AudioClip powerDownNoise;
    public AudioClip powerUpNoise;
    public bool isBroken;
    public ParticleSystem smokeVFX;
    public float damageInterval = 1f;
    public CropScript cropfield;
    void Start()
    {
        cropfield = GameObject.Find("CropField").GetComponent<CropScript>();
        foreach (Transform child in gameObject.transform)
        {
            lights.Add(child.gameObject.GetComponent<Light2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        damageInterval -= Time.deltaTime;
        if (damageInterval < 0)
        {
            // TODO: Randomize the damage taken per second for both machines
            TakeDamage(5);
            damageInterval = 1f;
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            cropfield.SetGrowth(false);
            SetLights(false);
            //generatorAudio.PlayOneShot(powerDownNoise);
        }
    }
    public void Fix()
    {
        currentHealth = maxHealth;
        cropfield.SetGrowth(true);
        SetLights(true);
        //generatorAudio.PlayOneShot(powerUpNoise);
    }

    public void SetLights(bool val)
    {
        for(int i = 0; i < lights.Count; i++)
        {
            lights[i].enabled = val;
        }
    }
}

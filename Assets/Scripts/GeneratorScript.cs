using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GeneratorScript : MonoBehaviour
{
    public float maxHealth = 300f;
    public float currentHealth = 300f;
    public List<Light2D> lights = new List<Light2D>();
    public AudioSource generatorAudio;
    public AudioSource glassAudio;
    public AudioClip powerDownNoise;
    public AudioClip generatorNoise;
    public bool isBroken;
    public ParticleSystem smokeVFX;
    public float damageInterval = 1f;
    public CropScript cropfield;
    public ObjectiveManager questManager;
    private bool audioPlaying = false;
    private GameObject player;
    // references to UI objects and managers that we'll need
    void Start()
    {
        cropfield = GameObject.Find("CropField").GetComponent<CropScript>();
        foreach (Transform child in gameObject.transform)
        {
            if (child.gameObject.GetComponent<Light2D>()) { 
                lights.Add(child.gameObject.GetComponent<Light2D>());
            }
        }
        generatorAudio = gameObject.GetComponent<AudioSource>();
        generatorAudio.loop = true;
        generatorAudio.clip = generatorNoise;
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        damageInterval -= Time.deltaTime;
        if (damageInterval < 0 && currentHealth > 0)
        {
            // TODO: Randomize the damage taken per second for both machines
            TakeDamage(5);
            damageInterval = 1f;
        }

        if((player.transform.position - gameObject.transform.position).magnitude <= 10 && !audioPlaying && !isBroken)
        {
            audioPlaying = true;
            generatorAudio.Play();
        }
        else if((player.transform.position - gameObject.transform.position).magnitude > 10 && audioPlaying)
        {
            audioPlaying = false;
            generatorAudio.Stop();
        }
    }

    public void TakeDamage(float dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            if (isBroken == false)
            {
                glassAudio.Play();
            }
            cropfield.SetGrowth(false);
            SetLights(false);
            generatorAudio.Pause();
            questManager.EnableGenQuest();
            isBroken = true;
        }
    }
    public void Fix()
    {
        currentHealth = maxHealth;
        cropfield.SetGrowth(true);
        SetLights(true);
        questManager.DisableGenQuest();
        generatorAudio.Play();
    }

    public void SetLights(bool val)
    {
        for(int i = 0; i < lights.Count; i++)
        {
            lights[i].enabled = val;
        }
    }
}

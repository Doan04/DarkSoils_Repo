using UnityEngine;
using System.Collections;
public class MainAudio : MonoBehaviour
{
    private AudioSource[] audioSources;
    public AudioClip WaveMusic;
    public AudioClip EnemyHit;

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }

    void Update()
    {
        
    }

    public IEnumerator Shop(bool enter)
    {
        float timer = 0.005f;
        if(enter)
        {
            for(float i = audioSources[1].volume; i < 0.25f; i += 0.01f)
            {
                audioSources[0].volume = 0.25f - i;
                audioSources[1].volume = i;
                yield return new WaitForSeconds(timer);
            }
        }
        else
        {
            for(float i = 0; i < 0.25f; i += 0.01f)
            {
                audioSources[1].volume = 0.25f - i;
                audioSources[0].volume = i;
                yield return new WaitForSeconds(timer);
            }
        }
    }

    public IEnumerator Boss(bool enter)
    {
        float timer = 0.005f;
        if(enter)
        {
            for(float i = audioSources[1].volume; i < 0.25f; i += 0.01f)
            {
                audioSources[0].volume = 0.25f - i;
                audioSources[2].volume = i;
                yield return new WaitForSeconds(timer);
            }
        }
        else
        {
            for(float i = 0; i < 0.25f; i += 0.01f)
            {
                audioSources[2].volume = 0.25f - i;
                audioSources[0].volume = i;
                yield return new WaitForSeconds(timer);
            }
        }
    }
    public void PlayWaveMusic() // call this when the wave starts
    {
        audioSources[0].PlayOneShot(WaveMusic);
    }

    public void PlayEnemyHit() // call this when an enemy takes damage
    {
        audioSources[0].PlayOneShot(EnemyHit);
    }
}

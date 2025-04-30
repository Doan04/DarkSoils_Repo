using UnityEngine;

public class MainAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip WaveMusic;
    public AudioClip EnemyHit;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void PlayWaveMusic() // call this when the wave starts
    {
        audioSource.PlayOneShot(WaveMusic);
    }

    public void PlayEnemyHit() // call this when an enemy takes damage
    {
        audioSource.PlayOneShot(EnemyHit);
    }
}

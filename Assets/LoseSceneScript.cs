using UnityEngine;
using UnityEngine.SceneManagement;
public class LoseSceneScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AudioSource audioSource;
    public AudioClip loseSound;
    void Start()
    {
        audioSource.PlayOneShot(loseSound);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

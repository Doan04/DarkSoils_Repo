using UnityEngine;
using TMPro;
public class StatsScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public PlayerScript playerScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = gameObject.GetComponent<TextMeshProUGUI>();
        GameObject player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        string stats = "Player Stats: \n";
        stats += "Health: " + playerScript.currentHealth + "/" + playerScript.maxHealth + "\n";
        stats += "Attack: " + playerScript.attack + "\n";
        stats += "Fertilizer: " + playerScript.currentFert + "/" + playerScript.maxFert + "\n";
        stats += "Speed: " + playerScript.playerSpeed + "\n";
        stats += "Money: " + playerScript.money + "\n";
        text.SetText(stats);
    }
}

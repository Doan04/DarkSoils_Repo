using TMPro;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text npcText;
    public TMP_Text farmText;
    public TMP_Text waterText;
    public TMP_Text generatorText;
    public void EnableNPCQuest(string text)
    {
        npcText.enabled = true;
        npcText.SetText(text);
    }
    public void DisableNPCQuest()
    {
        npcText.enabled = false;
    }
    public void EnableWaterQuest()
    {
        waterText.enabled = true;
    }
    public void DisableWaterQuest()
    {
        waterText.enabled = false;
    }
    public void EnableGenQuest()
    {
        generatorText.enabled = true;
    }
    public void DisableGenQuest()
    {
        generatorText.enabled = false;
    }
}

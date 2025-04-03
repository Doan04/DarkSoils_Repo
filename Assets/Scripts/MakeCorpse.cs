using UnityEngine;

public class MakeCorpse : MonoBehaviour
{
    public GameObject corpse;
    public void SpawnCorpse(Quaternion orientation)
    {
        Instantiate(corpse, transform.position, orientation);
    }
}

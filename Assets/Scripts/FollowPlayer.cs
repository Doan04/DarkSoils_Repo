using UnityEngine;
public class FollowPlayer : MonoBehaviour
{
    Transform playerTransform;
    void Start()
    {
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }
    void Update()
    {
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10);
    }
}

using UnityEngine;

public class MinimapFollow : MonoBehaviour
{
    public Transform player; // Drag your player object (blue dot) here
    public Vector3 offset = new Vector3(0, 10, 0); // Adjust height if needed

    void LateUpdate()
    {
        if (player != null)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
        }
    }
}

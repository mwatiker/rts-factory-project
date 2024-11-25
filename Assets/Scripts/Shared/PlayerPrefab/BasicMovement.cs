using Unity.Netcode;
using UnityEngine;

public class BasicMovement : NetworkBehaviour
{
    public float moveSpeed = 5f;

    private void Update()
    {
        if (!IsOwner) return; // Only the owner can move this object

        // Handle input and move the circle
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(new Vector3(moveX, moveY, 0));
    }
}

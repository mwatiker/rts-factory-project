using Unity.Netcode;
using UnityEngine;

public class OnlineShooting : NetworkBehaviour
{
    public GameObject projectilePrefab; // Prefab for the projectile
    public float projectileForce = 10f; // Force applied to the projectile

    private void Update()
    {
        if (!IsOwner) return; // Only the owner can shoot

        // Fire a projectile when the left mouse button is clicked
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0; // Ensure the Z position matches the player's plane
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;

            FireProjectileServerRpc(direction);
        }
    }

    [ServerRpc]
    private void FireProjectileServerRpc(Vector3 direction)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab is not assigned.");
            return;
        }

        // Instantiate the projectile on the server
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Pass initial velocity to the projectile
        BulletController bulletController = projectile.GetComponent<BulletController>();
        if (bulletController != null)
        {
            bulletController.SetInitialVelocity(direction * projectileForce);
        }

        // Spawn the projectile on all clients
        NetworkObject networkObject = projectile.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.Spawn();
        }
        else
        {
            Debug.LogError("Projectile prefab is missing NetworkObject component.");
        }
    }
}

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
            FireProjectileServerRpc(mouseWorldPosition);
        }
    }

    [ServerRpc]
    private void FireProjectileServerRpc(Vector3 mouseWorldPosition)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab is not assigned.");
            return;
        }

        // Calculate the direction to fire the projectile
        Vector3 direction = (mouseWorldPosition - transform.position).normalized;

        // Instantiate the projectile on the server
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Spawn the projectile on all clients
        NetworkObject networkObject = projectile.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.Spawn();
        }
        else
        {
            Debug.LogError("Projectile prefab is missing NetworkObject component.");
            return;
        }

        // Apply force after spawning
        ApplyForceToProjectile(projectile, direction);
    }

    private void ApplyForceToProjectile(GameObject projectile, Vector3 direction)
    {
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Debug.Log("Rigidbody2D found. Applying force...");
            rb.AddForce(direction * projectileForce, ForceMode2D.Impulse);
            Debug.Log($"Force applied: {direction * projectileForce}");
        }
        else
        {
            Debug.LogError("Projectile prefab is missing Rigidbody2D component.");
        }
    }
}

using Unity.Netcode;
using UnityEngine;

public class OnlineShooting : NetworkBehaviour
{
    public GameObject projectilePrefab; // Prefab for the projectile
    public float projectileForce = 10f; // Force applied to the projectile

    private void Update()
    {
        if (!IsOwner) return; // Only the owner can shoot

        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;

            // Immediate local feedback
            FireProjectileClient(mouseWorldPosition);

            // Synchronize with server
            FireProjectileServerRpc(mouseWorldPosition);
        }
    }

    private void FireProjectileClient(Vector3 mouseWorldPosition)
    {
        Vector3 direction = (mouseWorldPosition - transform.position).normalized;

        // Local projectile creation and force application
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileForce;
        }
    }

    [ServerRpc]
    private void FireProjectileServerRpc(Vector3 mouseWorldPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector3 direction = (mouseWorldPosition - transform.position).normalized;
            rb.linearVelocity = direction * projectileForce;
        }
        projectile.GetComponent<NetworkObject>().Spawn();
    }
}

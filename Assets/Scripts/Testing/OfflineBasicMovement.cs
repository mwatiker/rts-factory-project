using UnityEngine;

public class OfflineBasicMovement : MonoBehaviour
{
    public GameObject projectilePrefab; // Prefab for the projectile
    public float moveSpeed = 5f; // Movement speed
    public float projectileForce = 10f; // Force applied to the projectile

    private void Update()
    {
        // Handle input and move the circle
        float moveX = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float moveY = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.Translate(new Vector3(moveX, moveY, 0));

        // Fire a projectile when the left mouse button is clicked
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab is not assigned.");
            return;
        }

        // Convert mouse position to world position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the Z position matches the player

        // Calculate the direction to fire the projectile
        Vector3 direction = (mousePosition - transform.position).normalized;

        // Instantiate the projectile
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        // Add force to the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * projectileForce, ForceMode2D.Impulse);
        }
        else
        {
            Debug.LogError("Projectile prefab is missing Rigidbody2D component.");
        }
    }
}

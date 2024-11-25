using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Bullet is missing a Rigidbody2D component.");
        }
    }

    public void SetInitialVelocity(Vector2 velocity)
    {
        if (rb != null)
        {
            rb.linearVelocity = velocity; // Set the Rigidbody2D's velocity directly
        }
    }
}

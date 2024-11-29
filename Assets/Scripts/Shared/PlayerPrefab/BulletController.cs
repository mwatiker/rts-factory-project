using Unity.Netcode;
using UnityEngine;


public class BulletController : MonoBehaviour
{
    private Rigidbody2D rb;

    private ulong shooterID;

    private NetworkObject thisNetworkObject;

    private void Awake()
    {
        thisNetworkObject = GetComponent<NetworkObject>();
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Bullet is missing a Rigidbody2D component.");
        }
    }

    public void SetShooterID(ulong id)
    {
        shooterID = id;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // get the network object of the player
            NetworkObject networkObject = collision.gameObject.GetComponent<NetworkObject>();
            // if the network object id is not equal to the owner id of the bullet
            // then destroy the bullet
            if (networkObject.NetworkObjectId != shooterID)
            {
                thisNetworkObject.Despawn();
                Destroy(gameObject);

            }
        }
    }
}

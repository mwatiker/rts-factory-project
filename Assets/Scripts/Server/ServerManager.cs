using Unity.Netcode;
using UnityEngine;

public class ServerManager : MonoBehaviour
{
    private void Start()
    {
        // Start the server when the scene loads
        NetworkManager.Singleton.StartServer();
        Debug.Log("Server started. Waiting for clients...");
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client connected: {clientId}");
    }

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }
}

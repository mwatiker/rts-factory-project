using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

public class ServerPlayerSpawner : MonoBehaviour
{
    private HashSet<ulong> spawnedPlayers = new HashSet<ulong>();

    private void OnEnable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += SpawnPlayer;
    }

    private void OnDisable()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= SpawnPlayer;
    }

    private void SpawnPlayer(ulong clientId)
    {
        // Only run this on the server
        if (!NetworkManager.Singleton.IsServer) 
        {
            Debug.LogWarning("Tried to spawn a player on the server, but the current machine is not a server.");
            return;
        }

        // Ensure we don't spawn multiple players for the same client
        if (spawnedPlayers.Contains(clientId))
        {
            Debug.LogWarning($"Player for client {clientId} is already spawned.");
            return;
        }

        // Instantiate and spawn the player
        GameObject playerObject = Instantiate(
            NetworkManager.Singleton.NetworkConfig.PlayerPrefab,
            Vector3.zero, // Spawn at (0,0) for now
            Quaternion.identity
        );

        playerObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);

        // Track the spawned player
        spawnedPlayers.Add(clientId);

        Debug.Log($"Spawned player for client {clientId}");
    }
}

using Unity.Netcode;
using UnityEngine;

public class ClientConnect : MonoBehaviour
{
    public void ConnectToServer()
    {
        NetworkManager.Singleton.StartClient();
        Debug.Log("Attempting to connect to server...");
    }
}

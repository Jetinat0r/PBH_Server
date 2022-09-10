using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

/// <summary>
/// Network Manager for the SERVER
/// </summary>
public class NetworkManager : MonoBehaviour
{
    //Singleton
    public static NetworkManager instance;

    public Server Server { get; private set; }

    [SerializeField]
    private ushort port = 7777;
    [SerializeField]
    private ushort maxClientCount = 4;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Server = new Server();
        Server.Start(port, maxClientCount);
    }

    private void FixedUpdate()
    {
        Server.Tick();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }
}

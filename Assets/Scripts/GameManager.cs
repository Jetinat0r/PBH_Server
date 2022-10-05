using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player playerPrefab;

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
        NetworkManager.instance.Server.ClientDisconnected += RemovePlayer;
    }

    // Update is called once per frame
    void Update()
    {

    }

    [MessageHandler((ushort)ClientToServerId.sendJoinInfo)]
    private static void SpawnPlayer(ushort fromClientId, Message message)
    {
        //Tell the player who just joined who is on the server
        //This is sent before the new player is added to the dictionary
        //  because the players own avatars are handled differently
        //  on their respective clients
        SendCurrentPlayersInfo(fromClientId);


        //Handle message
        string playerName = message.GetString();

        Player player = Instantiate(GameManager.instance.playerPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        Player.PlayerList.Add(fromClientId, player);
        player.GetComponent<SpriteRenderer>().color = Player.PlayerColorMap[fromClientId]; //TODO: Change to a setup function
        player.SetSpawnInfo(fromClientId, playerName);


        //Tell everyone else about it
        SendNewPlayerJoined(fromClientId, playerName);
    }

    //TODO: Refactor using the player dictionary probably
    //Sends all currently connected clients info about the new client that just joined
    private static void SendNewPlayerJoined(ushort fromClientId, string playerName)
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ServerToClientId.playerSpawnInfo);
        message.AddUShort(fromClientId);     //Add client ID
        message.AddString(playerName);       //Add player name

        NetworkManager.instance.Server.SendToAll(message, fromClientId);
    }

    //TODO: Refactor for Player dictionary
    //Sends the client that just joined info about all other currently connected clients
    private static void SendCurrentPlayersInfo(ushort toClientId)
    {
        foreach(KeyValuePair<ushort, Player> player in Player.PlayerList)
        {
            Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.playerSpawnInfo);

            message.Add(player.Value.PlayerId);
            message.AddString(player.Value.PlayerUserName);

            NetworkManager.instance.Server.Send(message, toClientId);
        }
    }

    //Uses the "playerDisconnected" server event
    private void RemovePlayer(object sender, ClientDisconnectedEventArgs e)
    {
        //TODO: Check if it "breaks" a round

        Destroy(Player.PlayerList[e.Id].gameObject);
        Player.PlayerList.Remove(e.Id);
    }
}

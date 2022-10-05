using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> PlayerList = new Dictionary<ushort, Player>();
    public static Dictionary<ushort, Color> PlayerColorMap = new Dictionary<ushort, Color>();

    public ushort PlayerId { get; private set; }
    public string PlayerUserName { get; private set; }

    private void Awake()
    {
        if(PlayerColorMap.Count == 0)
        {
            PlayerColorMap.Add(1, Color.red);
            PlayerColorMap.Add(2, Color.green);
            PlayerColorMap.Add(3, Color.magenta);
            PlayerColorMap.Add(4, Color.blue);
        }
    }

    public void SetSpawnInfo(ushort id, string username)
    {
        PlayerId = id;
        PlayerUserName = username;
    }
}

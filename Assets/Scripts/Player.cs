using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;
using TMPro;

public class Player : MonoBehaviour
{
    public static Dictionary<ushort, Player> PlayerList = new Dictionary<ushort, Player>();
    public static Dictionary<ushort, Color> PlayerColorMap = new Dictionary<ushort, Color>();

    [SerializeField]
    private GameObject playerPivot;

    [SerializeField]
    private TMP_Text nameplate;
    [SerializeField]
    private SpriteRenderer baseGraphics;

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
        PlayerUserName = username != "" ? username : $"Guest ({PlayerId})"; //TODO: Move to when the name is received
        baseGraphics.color = PlayerColorMap[id];

        nameplate.text = PlayerUserName;
    }

    public void SetPosRot(Vector3 pos, Quaternion rot)
    {
        transform.position = pos;
        playerPivot.transform.rotation = rot;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetRotation()
    {
        return playerPivot.transform.rotation;
    }
}

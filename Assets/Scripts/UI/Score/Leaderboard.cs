using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Leaderboard : NetworkBehaviour
{
    [SerializeField] private Transform leaderboardHolder;
    [SerializeField] private DisplayScore leaderboardPrefab;
    
    private NetworkList<LeaderboardState> stateList;

    private void Awake()
    {
        stateList = new NetworkList<LeaderboardState>();
    }
}

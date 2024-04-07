using System;
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

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            stateList.OnListChanged += HandleLeaderboardStateChanged;

            foreach (LeaderboardState state in stateList)
            {
                HandleLeaderboardStateChanged(new NetworkListEvent<LeaderboardState>
                {
                    Type = NetworkListEvent<LeaderboardState>.EventType.Add,
                    Value = state
                });
            }
        }

        if (IsServer)
        {
            TankPlayer[] players = FindObjectsByType<TankPlayer>(FindObjectsSortMode.None);

            foreach (TankPlayer player in players)
            {
                HandlePlayerSpawned(player);
            }

            TankPlayer.OnPlayerSpawned += HandlePlayerSpawned;
            TankPlayer.OnPlayerDespawned += HandlePlayerDespawned;
        }
    }

    public override void OnNetworkDespawn()
    {
        if(IsClient)
        {
            stateList.OnListChanged -= HandleLeaderboardStateChanged;
        }

        if (IsServer)
        {
            TankPlayer.OnPlayerSpawned -= HandlePlayerSpawned;
            TankPlayer.OnPlayerDespawned -= HandlePlayerDespawned;
        }
    }

    private void HandleLeaderboardStateChanged(NetworkListEvent<LeaderboardState> changeEvent)
    {
        switch (changeEvent.Type)
        {
            case NetworkListEvent<LeaderboardState>.EventType.Add:
                Instantiate(leaderboardPrefab, leaderboardHolder);
                break;
        }
    }

    private void HandlePlayerSpawned(TankPlayer player)
    {
        stateList.Add(new LeaderboardState
        {
            ClientID = player.OwnerClientId,
            PlayerName = player.PlayerName.Value,
            Coins = 0
        }); 
    }

    private void HandlePlayerDespawned(TankPlayer player)
    {
        if (stateList == null) { return; }

        foreach (LeaderboardState state in stateList)
        {
            if(state.ClientID != player.OwnerClientId)
            {
                continue;
            }
            stateList.Remove(state);
            break;
        }
    }
}

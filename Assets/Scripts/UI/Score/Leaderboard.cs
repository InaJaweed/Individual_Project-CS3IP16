using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Leaderboard : NetworkBehaviour
{
    [SerializeField] private Transform leaderboardHolder;
    [SerializeField] private DisplayScore leaderboardPrefab;
    [SerializeField] private int scoreToDisplay = 5;
    
    private NetworkList<LeaderboardState> stateList;
    private List<DisplayScore> scoreDisplay = new List<DisplayScore>();

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
                if(!scoreDisplay.Any(x => x.ClientId == changeEvent.Value.ClientID))
                {
                   DisplayScore leaderboardScore = Instantiate(leaderboardPrefab, leaderboardHolder);
                    
                    leaderboardScore.Initialise(changeEvent.Value.ClientID, changeEvent.Value.PlayerName, changeEvent.Value.Coins);

                    scoreDisplay.Add(leaderboardScore);
                }
                break;
            case NetworkListEvent<LeaderboardState>.EventType.Remove:
               DisplayScore displayToRemove = scoreDisplay.FirstOrDefault(x => x.ClientId == changeEvent.Value.ClientID);

                if(displayToRemove != null)
                {
                    displayToRemove.transform.SetParent(null);
                    Destroy(displayToRemove.gameObject);
                    scoreDisplay.Remove(displayToRemove);
                }
                break;
            case NetworkListEvent<LeaderboardState>.EventType.Value:
                DisplayScore displayToValueUpdate = scoreDisplay.FirstOrDefault(x => x.ClientId == changeEvent.Value.ClientID);

                if(displayToValueUpdate != null)
                {
                    displayToValueUpdate.UpdateCoins(changeEvent.Value.Coins);
                }
                break;
        }

        scoreDisplay.Sort((first, last) => last.Coins.CompareTo(first.Coins));

        for (int i = 0; i < scoreDisplay.Count; i++)
        {
            scoreDisplay[i].transform.SetSiblingIndex(i);
            scoreDisplay[i].UpdateText();

            bool shouldShow = i <= scoreToDisplay - 1;
            scoreDisplay[i].gameObject.SetActive(shouldShow);
        }

        DisplayScore myScore = scoreDisplay.FirstOrDefault(x => x.ClientId == NetworkManager.Singleton.LocalClientId);

        if (myScore != null)
        {
            if(myScore.transform.GetSiblingIndex() >= scoreToDisplay)
            {
                leaderboardHolder.GetChild(scoreToDisplay -1).gameObject.SetActive(false);
                myScore.gameObject.SetActive(true);
            }
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

        player.Wallet.TotalCoins.OnValueChanged += (oldCoins, newCoins) => HandleCoinsUpdate(player.OwnerClientId, newCoins);
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

        player.Wallet.TotalCoins.OnValueChanged -= (oldCoins, newCoins) => HandleCoinsUpdate(player.OwnerClientId, newCoins);
    }

    private void HandleCoinsUpdate(ulong clientId, int newCoins)
    {
        for (int i = 0; i < stateList.Count; i++)
        {
            if(stateList[i].ClientID != clientId)
            {
                continue;
            }

            stateList[i] = new LeaderboardState
            {
                ClientID = stateList[i].ClientID,
                PlayerName = stateList[i].PlayerName,
                Coins = newCoins
            };

            return;
        }
    }
}

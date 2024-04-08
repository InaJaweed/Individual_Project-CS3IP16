using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandle : NetworkBehaviour
{
    [SerializeField] private TankPlayer playerPrefab;
    [SerializeField] private float KeptcoinsPercentage;

    public override void OnNetworkSpawn()
    {
        if(!IsServer)
        {
            return;
        }

        TankPlayer[] players = FindObjectsByType<TankPlayer>(FindObjectsSortMode.None);
        foreach(TankPlayer player in players)
        {
            HandlePlayerSpawned(player);
        }

        TankPlayer.OnPlayerSpawned += HandlePlayerSpawned;
        TankPlayer.OnPlayerDespawned += HandlePlayerDespawned;
    }
    
    public override void OnNetworkDespawn()
    {
        if(!IsServer)
        {
            return;
        }

        TankPlayer.OnPlayerSpawned -= HandlePlayerSpawned;
        TankPlayer.OnPlayerDespawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(TankPlayer player)
    {
        player.Health.onDie += (health) => HandlePlayerDeath(player);
    }

    private void HandlePlayerDespawned(TankPlayer player)
    {
        player.Health.onDie -= (health) => HandlePlayerDeath(player);
    }

    private void HandlePlayerDeath(TankPlayer player)
    {
        int keptCoins = (int) (player.Wallet.TotalCoins.Value * (KeptcoinsPercentage / 100));
        Destroy(player.gameObject);

        StartCoroutine(RespawnPlayer(player.OwnerClientId, keptCoins));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId, int keptCoins)
    {
        yield return null;

       TankPlayer playerInstance =  Instantiate(playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);

        playerInstance.NetworkObject.SpawnAsPlayerObject(ownerClientId);
        playerInstance.Wallet.TotalCoins.Value += keptCoins;
    }
}

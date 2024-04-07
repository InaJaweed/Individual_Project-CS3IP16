using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RespawnHandle : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;

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
        Destroy(player.gameObject);

        StartCoroutine(RespawnPlayer(player.OwnerClientId));
    }

    private IEnumerator RespawnPlayer(ulong ownerClientId)
    {
        yield return null;

       NetworkObject playerInstance =  Instantiate(playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);

        playerInstance.SpawnAsPlayerObject(ownerClientId);
    }
}

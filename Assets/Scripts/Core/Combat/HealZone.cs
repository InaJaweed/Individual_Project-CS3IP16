using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HealZone : NetworkBehaviour
{
    [Header("References")]
    [SerializeField] private Image healBar;

    [Header("Settings")]
    [SerializeField] private int maxHealPower = 30;
    [SerializeField] private float zoneCooldown = 60f;
    [SerializeField] private float healTickRate = 1f;
    [SerializeField] private int coinsPerTick = 2;
    [SerializeField] private int healthPerTick = 10;

    private List<TankPlayer> playersInZone = new List<TankPlayer>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer)
        {
            return;
        }

        if(!collision.attachedRigidbody.TryGetComponent<TankPlayer>(out TankPlayer player))
        {
            return;
        }

        playersInZone.Add(player);
        Debug.Log($"Player Entered Zone : {player.PlayerName.Value}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(!IsServer) 
        { 
            return; 
        }

        if (!collision.attachedRigidbody.TryGetComponent<TankPlayer>(out TankPlayer player))
        {
            return;
        }

        playersInZone.Remove(player);
        Debug.Log($"Player Exit Zone : {player.PlayerName.Value}");
    }

}

using System;
using Unity.Collections;
using Unity.Netcode;

public struct LeaderboardState : INetworkSerializable, IEquatable<LeaderboardState>
{
    public ulong ClientID;
    public FixedString32Bytes PlayerName;
    public int Coins;

    public bool Equals(LeaderboardState other)
    {
        return ClientID == other.ClientID && PlayerName.Equals(other.PlayerName) && Coins == other.Coins;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientID);
        serializer.SerializeValue(ref PlayerName);
        serializer.SerializeValue(ref Coins);
    }
}

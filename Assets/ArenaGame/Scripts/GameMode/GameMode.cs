using System.Collections.Generic;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

/// <summary>
/// A gamemode class network object, owned and instantiated by the server
/// </summary>
public class GameMode : GameModeBehavior
{
    // Use this for initialization
    void Start()
    {
        //safety check lol
        if (!networkObject.IsServer)
        {
            return;
        }

        ArenaGenerator.Instance.InitArena();

        NetworkManager.Instance.Networker.playerAccepted += (player, sender) =>
        {
            MainThreadManager.Run(() =>
            {
                networkObject.SendRpc(player, RPC_GENERATE_MAP,
                    ArenaGenerator.Instance.mapGenerator.seed,
                    ArenaGenerator.Instance.mapGenerator.mapWidth,
                    ArenaGenerator.Instance.mapGenerator.mapHeight,
                    ArenaGenerator.Instance.mapGenerator.noiseScale,
                    ArenaGenerator.Instance.mapGenerator.octaves,
                    ArenaGenerator.Instance.mapGenerator.persistance,
                    ArenaGenerator.Instance.mapGenerator.lacunarity,
                    ArenaGenerator.Instance.mapGenerator.meshHeightMultiplier,
                    ArenaGenerator.Instance.mapGenerator.offset.x,
                    ArenaGenerator.Instance.mapGenerator.offset.y,
                    ArenaGenerator.Instance.mapGenerator.useFalloff
                );
            });
        };

        //Handle disconnection
        NetworkManager.Instance.Networker.playerDisconnected += (player, sender) =>
        {
            MainThreadManager.Run(() =>
            {
                //Loop through all players and find the player who disconnected, store all it's networkobjects to a list
                List<NetworkObject> toDelete = new List<NetworkObject>();
                foreach (var no in sender.NetworkObjectList)
                {
                    if (no.Owner == player)
                    {
                        //Found him
                        toDelete.Add(no);
                    }
                }

                //Remove the actual network object outside of the foreach loop, as we would modify the collection at runtime elsewise. (could also use a return, too late)
                if (toDelete.Count > 0)
                {
                    for (int i = toDelete.Count - 1; i >= 0; i--)
                    {
                        sender.NetworkObjectList.Remove(toDelete[i]);
                        toDelete[i].Destroy();
                    }
                }
            });
        };
    }

    public override void GenerateMap(RpcArgs args)
    {
        ArenaGenerator.Instance.mapGenerator.seed = args.GetNext<int>();
        ArenaGenerator.Instance.mapGenerator.mapWidth = args.GetNext<int>();
        ArenaGenerator.Instance.mapGenerator.mapHeight = args.GetNext<int>();
        ArenaGenerator.Instance.mapGenerator.noiseScale = args.GetNext<float>();
        ArenaGenerator.Instance.mapGenerator.octaves = args.GetNext<int>();
        ArenaGenerator.Instance.mapGenerator.persistance = args.GetNext<float>();
        ArenaGenerator.Instance.mapGenerator.lacunarity = args.GetNext<float>();
        ArenaGenerator.Instance.mapGenerator.meshHeightMultiplier = args.GetNext<float>();
        ArenaGenerator.Instance.mapGenerator.offset.x = args.GetNext<float>();
        ArenaGenerator.Instance.mapGenerator.offset.y = args.GetNext<float>();
        ArenaGenerator.Instance.mapGenerator.useFalloff = args.GetNext<bool>();
        ArenaGenerator.Instance.GenerateArena();
    }
}
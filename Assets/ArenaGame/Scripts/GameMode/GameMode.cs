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

        ArenaGenerator.Instance.GenerateRandomArena();

        NetworkManager.Instance.Networker.playerAccepted += (player, sender) =>
        {
            MainThreadManager.Run(() =>
            {
                //Do some counting logic here for a gamemode, eg, assign team to newly joined player, or restart round if enough people joined
                //Remember to remove players from counter in playerDisconnected event as well

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
                    ArenaGenerator.Instance.mapGenerator.offset.y
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
        MapGenerator.Instance.seed = args.GetNext<int>();
        MapGenerator.Instance.mapWidth = args.GetNext<int>();
        MapGenerator.Instance.mapHeight = args.GetNext<int>();
        MapGenerator.Instance.noiseScale = args.GetNext<float>();
        MapGenerator.Instance.octaves = args.GetNext<int>();
        MapGenerator.Instance.persistance = args.GetNext<float>();
        MapGenerator.Instance.lacunarity = args.GetNext<float>();
        MapGenerator.Instance.meshHeightMultiplier = args.GetNext<float>();
        MapGenerator.Instance.offset.x = args.GetNext<int>();
        MapGenerator.Instance.offset.y = args.GetNext<int>();
        MapGenerator.Instance.GenerateMap();
    }
}
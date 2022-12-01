using DefaultNamespace;
using UnityEngine;

public class ArenaGenerator : MonoBehaviour
{
    public static ArenaGenerator Instance { get; private set; }
    
    public MapGenerator mapGenerator;

    public SpawnPlayer spawnPlayer;
    
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateRandomArena()
    {
        mapGenerator.GenerateRandomMap();
        ReplaceSpawns();
    }
    
    public void ReplaceSpawns()
    {
        for (int i = 0; i < spawnPlayer.transform.childCount; i++)
        {
            Transform spawnPoint = spawnPlayer.transform.GetChild(i).transform; //?
            
            //Put spawn higher to detect floor
            spawnPoint.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y + 200f, spawnPoint.position.z);
            Transform newTransform = SurfaceAligner.CalculatePositionAndAddHeight(spawnPoint, 1.5f);
            spawnPoint.position = newTransform.position;
            spawnPoint.rotation = newTransform.rotation;
        }
    }
    
    
}

using UnityEngine;

public class GenerationParameters : MonoBehaviour
{
    public bool useParams = false;

    public int seed;
    
    public int octaves;

    public float lacunarity;
    
    public float scale;
    
    public float persistance;
    
    public bool falloff;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
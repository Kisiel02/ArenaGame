using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MapCraftingManager : MonoBehaviour
{
    [SerializeField] private ArenaGenerator arenaGenerator;

    [SerializeField] private MapGenerator mapGenerator;

    [SerializeField] private Slider octavesSlider;

    [SerializeField] private Slider lacunaritySlider;

    [SerializeField] private Slider scaleSlider;

    [SerializeField] private Slider persistanceSlider;

    [SerializeField] private Toggle falloffToggle;
    
    // Start is called before the first frame update
    void Awake()
    {
        arenaGenerator.GenerateRandomArena();
    }
    
    public void UseThisMap()
    {
        PrepareGenerationParams();
        SceneManager.LoadScene("MainMenu");
    }

    public void DiscardMap()
    {
        GameObject.FindWithTag("GenerationParameters").GetComponent<GenerationParameters>().useParams = false;
        SceneManager.LoadScene("MainMenu");
    }

    private void PrepareGenerationParams()
    {
        GenerationParameters generationParameters = GameObject.FindWithTag("GenerationParameters")
            .GetComponent<GenerationParameters>();

        generationParameters.seed = mapGenerator.seed;
        generationParameters.useParams = true;
        generationParameters.octaves = (int)octavesSlider.value;
        generationParameters.lacunarity = lacunaritySlider.value;
        generationParameters.scale = scaleSlider.value;
        generationParameters.persistance = persistanceSlider.value;
        generationParameters.falloff = falloffToggle.isOn;
    }

    public void RestoreDefaults()
    {
        octavesSlider.value = 5.0f;
        lacunaritySlider.value = 5.0f;
        scaleSlider.value = 30.0f;
        persistanceSlider.value = 0.2f;
        falloffToggle.isOn = true;
    }

    public void OnSeedInputEnd(TMP_InputField seedInputField)
    {
        int seed = int.Parse(seedInputField.text);
        mapGenerator.seed = seed;
        arenaGenerator.GenerateArena();
    }

    public void OnRandomSeedButtonClicked(TMP_InputField seedInputField)
    {
        int seed = Random.Range(100000, 999999);
        seedInputField.text = seed.ToString();
        mapGenerator.seed = seed;
        arenaGenerator.GenerateArena();
    }

    public void OnFalloffChange()
    {
        bool useFalloff = falloffToggle.isOn;
        mapGenerator.useFalloff = useFalloff;
        arenaGenerator.GenerateArena();
    }

    public void OnOctavesChange(Slider slider)
    {
        int octaves = (int)slider.value;
        mapGenerator.octaves = octaves;
        arenaGenerator.GenerateArena();
    }

    public void OnScaleChange(Slider slider)
    {
        float scale = slider.value;
        mapGenerator.noiseScale = scale;
        arenaGenerator.GenerateArena();
    }

    public void OnPersistanceChange(Slider slider)
    {
        float persistance = slider.value;
        mapGenerator.persistance = persistance;
        arenaGenerator.GenerateArena();
    }

    public void OnLacunarityChange(Slider slider)
    {
        float lacunarity = slider.value;
        mapGenerator.lacunarity = lacunarity;
        arenaGenerator.GenerateArena();
    }
}
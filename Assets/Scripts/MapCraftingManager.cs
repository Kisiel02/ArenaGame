using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapCraftingManager : MonoBehaviour
{
    [SerializeField] private ArenaGenerator arenaGenerator;
    
    [SerializeField] private Slider octavesSlider;

    [SerializeField] private Slider lacunaritySlider;

    [SerializeField] private Slider scaleSlider;

    [SerializeField] private Slider persistanceSlider;

    [SerializeField] private Toggle falloffToggle;
    
    [SerializeField] private GenerationParameters generationParameters;
    
    [SerializeField] private TMP_InputField seedInputField;
    
    void Awake()
    {
        OnRandomSeedButtonClicked();
        PrepareGenerationParams();
        arenaGenerator.GenerateArenaFromParams();
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

        generationParameters.useParams = true;
        generationParameters.seed = int.Parse(seedInputField.text);
        generationParameters.octaves = (int)octavesSlider.value;
        generationParameters.lacunarity = lacunaritySlider.value;
        generationParameters.scale = scaleSlider.value;
        generationParameters.persistance = persistanceSlider.value;
        generationParameters.falloff = falloffToggle.isOn;
    }

    public void RestoreDefaults()
    {
        octavesSlider.value = 5.0f;
        lacunaritySlider.value = 2.0f;
        scaleSlider.value = 30.0f;
        persistanceSlider.value = 0.3f;
        falloffToggle.isOn = true;
    }

    public void OnSeedInputEnd()
    {
        int seed = int.Parse(seedInputField.text);
        generationParameters.seed = seed;
        arenaGenerator.GenerateArenaFromParams();
    }

    public void OnRandomSeedButtonClicked()
    {
        int seed = Random.Range(100000, 999999);
        seedInputField.text = seed.ToString();
        generationParameters.seed = int.Parse(seedInputField.text);
        arenaGenerator.GenerateArenaFromParams();
    }

    public void OnFalloffChange()
    {
        bool useFalloff = falloffToggle.isOn;
        generationParameters.falloff = useFalloff;
        arenaGenerator.GenerateArenaFromParams();
    }

    public void OnOctavesChange()
    {
        int octaves = (int)octavesSlider.value;
        generationParameters.octaves = octaves;
        arenaGenerator.GenerateArenaFromParams();
    }

    public void OnScaleChange()
    {
        float scale = scaleSlider.value;
        generationParameters.scale = scale;
        arenaGenerator.GenerateArenaFromParams();
    }

    public void OnPersistanceChange()
    {
        float persistance = persistanceSlider.value;
        generationParameters.persistance = persistance;
        arenaGenerator.GenerateArenaFromParams();
    }

    public void OnLacunarityChange()
    {
        float lacunarity = lacunaritySlider.value;
        generationParameters.lacunarity = lacunarity;
        arenaGenerator.GenerateArenaFromParams();
    }
}
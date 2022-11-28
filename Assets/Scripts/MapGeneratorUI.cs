using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGeneratorUI : MonoBehaviour
{
    public Toggle tog;
    public MapGenerator mapGen;
    public Slider slider;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        slider.onValueChanged.AddListener((v) => {
            text.text = v.ToString("0.00");
            mapGen.persistance = v;
            mapGen.GenerateMap();
        
        });

        tog.onValueChanged.AddListener((v) => {
            mapGen.useFalloff = v;
            mapGen.GenerateMap();
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            mapGen.GenerateMap();
        }
    }
}

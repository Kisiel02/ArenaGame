Shader "Custom/Terrain"
{
    Properties
    {
       
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        const static int maxColourCount = 6;
        const static float epsilon = 1E-4;
        int baseColourCount;
        float3 baseColours[maxColourCount];
        float baseStartHeights[maxColourCount];
        float baseBlends[maxColourCount];

        float minHeight;
        float maxHeight;

        struct Input
        {
            float3 worldPos;
        };

        float inverseLerp(float a, float b, float value){
            return saturate((value-a)/(b-a));
        }


        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float heightPercent = inverseLerp(minHeight, maxHeight, IN.worldPos.y);
            for (int i=0; i<baseColourCount; i++){
                float drawStrength = inverseLerp(-baseBlends[i]/2 - epsilon,baseBlends[i]/2, heightPercent - baseStartHeights[i]);
                o.Albedo = o.Albedo * (1-drawStrength) + baseColours[i] * drawStrength;
            }
        }
        ENDCG
    }
    FallBack "Diffuse"
}

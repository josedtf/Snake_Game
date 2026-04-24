Shader "Custom/GridShader"
{
    Properties
    {
        _Color1 ("Color 1", Color) = (0,0.5,1,1) // Azul
        _Color2 ("Color 2", Color) = (0,1,1,1)   // Cyan
        _GridSize ("Grid Size", Float) = 1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _GridSize;
            float4 _Color1;
            float4 _Color2;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 gridUV = i.uv * (_GridSize * 30) + 0.5;

                float x = floor(gridUV.x);
                float z = floor(gridUV.y);

                float checker = fmod(x + z, 2);

                return checker < 1 ? _Color1 : _Color2;
            }
            ENDCG
        }
    }
}
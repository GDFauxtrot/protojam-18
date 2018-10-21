Shader "Unlit/GradientReveal"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _Gradient ("Gradient Texture", 2D) = "white" {}
        _Step ("Gradient Step", Float) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Blend One OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            
            v2f vert (appdata v)
            {
                v2f o;
                // o.vertex = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                // o.uv = v.uv.xy;
                return o;
            }

            sampler2D _Gradient;
            float _Step;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 gradient = tex2D(_Gradient, i.uv);

                // Greyscale (relevant value) is average of RGB
                float pixelValue = (gradient.r + gradient.b + gradient.b) / 3;

                if (pixelValue <= _Step)
                    discard;

                col.rgb *= col.a;
                return col;
            }
            ENDCG
        }
    }
}

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/PortalScreenShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                //float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                //float2 uv : TEXCOORD0;
                float4 screenPos : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.vertex);

                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                float2 screenSpaceUV = i.screenPos.xy / i.screenPos.w;
                //i.screenPos /= i.screenPos.w;
                //fixed4 col = tex2D(_MainTex, float2(i.screenPos.x, i.screenPos.y));
                return tex2D(_MainTex, screenSpaceUV);

                //return col;
            }
            ENDCG
        }
    }
}

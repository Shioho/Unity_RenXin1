Shader "MyShader/uvShader"
{
    Properties
    {
        _SelfTex ("SelfTex", 2D) = "white" {}
        _WaterFlashTex ("WaterFlashTex", 2D) = "white" {}
        _FlashSpeed("FlashSpeed",Float) = 1
        _FlashColor("FlashColor",Color) = (1,1,1,1)

    }
    SubShader
    {

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
                float4 vertex : POSITION;
            };

            sampler2D _WaterFlashTex;
            sampler2D _SelfTex;
            float4 _SelfTex_ST;
            float _FlashSpeed;
            float4 _FlashColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _SelfTex);
                return o;
            }

            fixed4 frag (v2f i) : COLOR
            {

                float2 uv_offset = float2(_Time.y*_FlashSpeed,_Time.y*_FlashSpeed);
                fixed4 colFlash = tex2D(_WaterFlashTex, i.uv+uv_offset);

                fixed4 col = tex2D(_SelfTex, i.uv);
                return col+colFlash*_FlashColor;
            }
            ENDCG
        }
    }
}

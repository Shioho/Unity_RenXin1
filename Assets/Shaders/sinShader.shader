

Shader "MyShader/sinShader"
{
    Properties
    {
        _WaterRate("WaterRate",Float) = 0.5
        _WaterCycle("WaterCycle",Float) = 0.5
        _Color("Color",Color) = (1,1,1,1)
        _TexTure("Tex",2D) = "white"{}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex my_vertex  //把my_vertex作为顶点shader的入口
            #pragma fragment my_fragment

            fixed4 _Color;
            sampler2D _TexTure;
            float4 _TexTure_ST;
            float _WaterRate;
            float _WaterCycle;

            struct appdata{
                float4 pos:POSITION;
                float2 uv:TEXCOORD0;
            };

            struct v2f{
                float4 pos:POSITION;
                float2 uv:TEXCOORD0;
            };

            
            // 怎么获得输入的顶点数据（通过语义绑定）,输出也绑定语义
            // float4 my_vertex(float4 pos:POSITION):POSITION{

            //     return UnityObjectToClipPos(pos);
            // }
            v2f my_vertex(appdata v){
                v2f f;
                float dis = distance(v.pos.xyz,float3(0,0,0));
                float h = sin(dis*_WaterCycle+_Time.z)/_WaterRate;

                f.pos = mul(unity_ObjectToWorld,v.pos); 
                f.pos.y+=h;
                f.pos = mul(unity_WorldToObject,f.pos);


                f.pos = UnityObjectToClipPos(f.pos);
                f.uv = TRANSFORM_TEX(v.uv,_TexTure);
                return f;
            }
            fixed4 my_fragment(v2f f):COLOR{

                // return fixed4(1,1,0,1);
                // return _Color;
                return tex2D(_TexTure,f.uv);

            }
        

            ENDCG
        }

    }
    FallBack "Diffuse"
    
    
}

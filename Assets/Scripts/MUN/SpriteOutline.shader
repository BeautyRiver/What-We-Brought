Shader "Custom/SpriteOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (1,1,0,1) // 노란색 기본
        _OutlineWidth ("Outline Width", Range(0, 10)) = 1
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True" 
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            fixed4 _Color;
            fixed4 _OutlineColor;
            float _OutlineWidth;
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, IN.texcoord);
                
                // 만약 현재 픽셀이 투명하지 않다면 그냥 원래 색 출력
                if (c.a > 0.1) return c * IN.color;

                // 주변(상하좌우) 픽셀 검사
                float2 pixelUp = float2(0, _MainTex_TexelSize.y) * _OutlineWidth;
                float2 pixelDown = float2(0, -_MainTex_TexelSize.y) * _OutlineWidth;
                float2 pixelRight = float2(_MainTex_TexelSize.x, 0) * _OutlineWidth;
                float2 pixelLeft = float2(-_MainTex_TexelSize.x, 0) * _OutlineWidth;

                fixed4 pixelUpColor = tex2D(_MainTex, IN.texcoord + pixelUp);
                fixed4 pixelDownColor = tex2D(_MainTex, IN.texcoord - pixelUp); // Down은 그냥 빼기
                fixed4 pixelRightColor = tex2D(_MainTex, IN.texcoord + pixelRight);
                fixed4 pixelLeftColor = tex2D(_MainTex, IN.texcoord - pixelRight); // Left는 빼기

                // 주변에 색칠된 픽셀이 하나라도 있다면? -> 나는 아웃라인이다!
                if (pixelUpColor.a > 0.1 || pixelDownColor.a > 0.1 || pixelRightColor.a > 0.1 || pixelLeftColor.a > 0.1)
                {
                    return _OutlineColor;
                }

                return c * IN.color;
            }
        ENDCG
        }
    }
}
Shader "Custom/SpriteOutline_Improved"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [HDR] _OutlineColor ("Outline Color", Color) = (1,1,0,1) // HDR 추가로 발광 효과 대비
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
            float4 _MainTex_TexelSize; // 텍스처의 픽셀 사이즈 (1/width, 1/height)

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
                
                // 이미 색이 있다면(캐릭터 내부) 그냥 출력
                if (c.a > 0.1) return c * IN.color;

                // --- 외곽선 검사 로직 시작 ---
                
                // 픽셀 단위로 오프셋 계산 (X, Y축)
                float2 offset = _MainTex_TexelSize.xy * _OutlineWidth;

                // 8방향 검사 (상하좌우 + 대각선)
                // 하나라도 알파값이 있으면 외곽선으로 판정
                float alphaSum = 0;

                // 상하좌우
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(0, offset.y)).a;  // Up
                alphaSum += tex2D(_MainTex, IN.texcoord - float2(0, offset.y)).a;  // Down
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(offset.x, 0)).a;  // Right
                alphaSum += tex2D(_MainTex, IN.texcoord - float2(offset.x, 0)).a;  // Left

                // 대각선 (품질을 위해 추가, 성능이 중요하다면 제거 가능)
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(offset.x, offset.y)).a;   // Top-Right
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(-offset.x, offset.y)).a;  // Top-Left
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(offset.x, -offset.y)).a;  // Bottom-Right
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(-offset.x, -offset.y)).a; // Bottom-Left

                // 주변에 불투명한 픽셀이 하나라도 발견되었다면
                if (alphaSum > 0.1)
                {
                    return _OutlineColor;
                }
                // -------------------------

                return c * IN.color;
            }
        ENDCG
        }
    }
}
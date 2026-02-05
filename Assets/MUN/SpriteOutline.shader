Shader "Custom/SpriteOutline_Ghost"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        [HDR] _OutlineColor ("Outline Color", Color) = (1,1,0,1)
        _OutlineWidth ("Outline Width", Range(0, 30)) = 1
        
        // ⭐ 새로 추가된 속성: 몸체의 투명도만 따로 조절 (0 = 투명, 1 = 불투명)
        _BodyAlpha ("Body Alpha", Range(0, 1)) = 1 
        
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
        Blend One OneMinusSrcAlpha // 프리멀티플라이드 알파 블렌딩 (투명도 처리 개선)

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
            float _BodyAlpha; // ⭐ 변수 선언
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
                
                // 외곽선 계산을 위한 오프셋
                float2 offset = _MainTex_TexelSize.xy * _OutlineWidth;

                // 8방향 주변 알파값 검사
                float alphaSum = 0;
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(0, offset.y)).a;        // Up
                alphaSum += tex2D(_MainTex, IN.texcoord - float2(0, offset.y)).a;        // Down
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(offset.x, 0)).a;        // Right
                alphaSum += tex2D(_MainTex, IN.texcoord - float2(offset.x, 0)).a;        // Left
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(offset.x, offset.y)).a; // TR
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(-offset.x, offset.y)).a;// TL
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(offset.x, -offset.y)).a;// BR
                alphaSum += tex2D(_MainTex, IN.texcoord + float2(-offset.x, -offset.y)).a;// BL

                // 로직 변경: 
                // 1. 내 픽셀이 비어있는데(c.a <= 0.1) 주변에 픽셀이 있다(alphaSum > 0.1) -> 외곽선
                if (c.a <= 0.1 && alphaSum > 0.1)
                {
                    return _OutlineColor;
                }

                // 2. 그 외(캐릭터 내부) -> 몸체 색상 출력하되 투명도 조절 적용
                c.rgb *= c.a; // Pre-multiplied alpha 적용
                return c * IN.color * _BodyAlpha; 
            }
        ENDCG
        }
    }
}
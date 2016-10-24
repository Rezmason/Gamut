Shader "Gamut/Tinted_Texture_Shader" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_BrightnessTexture ("Brightness Texture", 2D) = "white" {}
		_SrcBlend ("Source Blending", Int) = 1
		_DstBlend ("Destination Blending", Int) = 1
		_ZWrite ("Draw to depth buffer", Int ) = 1
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull off
		Blend [_SrcBlend] [_DstBlend]
		ZWrite [_ZWrite]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct VertexInput
			{
				float4 vertex : POSITION;
				float4 brightnessUV : TEXCOORD0;
			};

			struct VertexOutput
			{
				float4 vertex : SV_POSITION;
				float4 brightnessUV : TEXCOORD1;
			};

			uniform float4x4 _InvertedColorSpaceTransform;
			uniform sampler2D _BrightnessTexture;
			uniform float4 _Color;

			VertexOutput vert (VertexInput input)
			{
				VertexOutput output;
				output.brightnessUV = input.brightnessUV;
				output.vertex = UnityObjectToClipPos(input.vertex);
				return output;
			}
			
			fixed4 frag (VertexOutput output) : SV_Target
			{
				return _Color * tex2D(_BrightnessTexture, output.brightnessUV).r;
			}
			ENDCG
		}
	}
}


Shader "Gamut/HSV_Cylinder_Shader"
{
	Properties
	{
		_BrightnessTexture ("Brightness Texture", 2D) = "white" {}
		_SrcBlend ("Source Blending", Int) = 1
		_DstBlend ("Destination Blending", Int) = 1
		_ZWrite ("Draw to depth buffer", Int ) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull off
		Blend [_SrcBlend] [_DstBlend]
		ZWrite Off

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
				float2 edge : TEXCOORD1;
			};

			struct VertexOutput
			{
				float4 vertex : SV_POSITION;
				float3 color : TEXCOORD0;
				float4 brightnessUV : TEXCOORD1;
				float2 edge : TEXCOORD2;
			};

			uniform float4x4 _InvertedColorSpaceTransform;
			uniform sampler2D _BrightnessTexture;

			// taken directly from http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
			fixed3 hsv2rgb(fixed3 c)
			{
			    fixed4 K = fixed4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
			    fixed3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
			    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
			}

			VertexOutput vert (VertexInput input)
			{
				VertexOutput output;
				float3 pos = mul(_InvertedColorSpaceTransform, mul(unity_ObjectToWorld, input.vertex)).xyz;

				float hue = degrees(atan2(pos.x, pos.z)) / 360; // good
				float sat = length(pos.xz) * 2.0; // good
				float val = pos.y + 0.5; // good

				output.color = hsv2rgb(fixed3(hue, sat, val));

				output.brightnessUV = input.brightnessUV;
				output.edge = input.edge;
				output.vertex = UnityObjectToClipPos(input.vertex);
				return output;
			}

			fixed4 frag (VertexOutput output) : SV_Target
			{
				float3 color = output.color;

				// Edge data falls in range 1.0 to 2.0, so that meshes with no uv are rendered without edges.
				if (output.edge.y >= 1.0) {
					float edge = output.edge.y - 1.0;

					color *= clamp(edge * 2 - 0.9, 0, 1);

					//color = lerp(color, fixed3(0.0, 0.0, 0.0), clamp((edge - 0.995) * 700.0, 0.0, 1.0));
					//color = lerp(color, fixed3(1.0, 1.0, 1.0), clamp((edge - 0.9) * 70.0, 0.0, 1.0));
				}

				// Edge data falls in range 1.0 to 2.0, so that meshes with no uv are rendered without edges.
				if (output.edge.x >= 1.0) {
					float edge = output.edge.x - 1.0;
					color = lerp(color, fixed3(0.0, 0.0, 0.0), clamp((edge - 0.8) * 70.0, 0.0, 1.0));
					//color = lerp(color, fixed3(1.0, 1.0, 1.0), clamp((edge - 0.9) * 70.0, 0.0, 1.0));
				}

				return fixed4(color * tex2D(_BrightnessTexture, output.brightnessUV).r, 1.0);
			}

			ENDCG
		}
	}
}

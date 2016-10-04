Shader "Unlit/HSV_Cylinder_Shader"
{
	Properties
	{
		
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct VertexInput
			{
				float4 vertex : POSITION;
				float2 edge : TEXCOORD0;
			};

			struct VertexOutput
			{
				float4 vertex : SV_POSITION;
				float3 worldPosition : TEXCOORD0;
				float2 edge : TEXCOORD1;
			};

			uniform float4x4 _InvertedColorSpaceTransform;

			VertexOutput vert (VertexInput input)
			{
				VertexOutput output;
				output.worldPosition = mul(_InvertedColorSpaceTransform, mul(unity_ObjectToWorld, input.vertex)).xyz;
				output.edge = input.edge;
				output.vertex = UnityObjectToClipPos(input.vertex);
				return output;
			}

			// taken directly from http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
			fixed3 hsv2rgb(fixed3 c)
			{
			    fixed4 K = fixed4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
			    fixed3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
			    return c.z * lerp(K.xxx, clamp(p - K.xxx, 0.0, 1.0), c.y);
			}
			
			fixed4 frag (VertexOutput output) : SV_Target
			{
				float3 pos = output.worldPosition; // This varies from space to space, naturally

				float hue = degrees(atan2(pos.x, pos.z)) / 360; // good
				float sat = length(pos.xz) * 2.0; // good
				float val = pos.y + 0.5; // good

				fixed3 color = hsv2rgb(fixed3(hue, sat, val));

				// Edge data falls in range 1.0 to 2.0, so that meshes with no uv are rendered without edges.
				if (output.edge.x >= 1.0) {
					float edge = output.edge.x - 1.0;
					color = lerp(color, fixed3(0.0, 0.0, 0.0), clamp((edge - 0.8) * 70.0, 0.0, 1.0));
					color = lerp(color, fixed3(1.0, 1.0, 1.0), clamp((edge - 0.9) * 70.0, 0.0, 1.0));
				}

				return fixed4(color, 1.0);
			}

			ENDCG
		}
	}
}

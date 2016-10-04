Shader "Unlit/RGB_Cube_Shader"
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
			
			fixed4 frag (VertexOutput output) : SV_Target
			{
				fixed3 color = output.worldPosition + 0.5; // This varies from space to space, naturally

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

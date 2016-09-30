Shader "Unlit/ColorCubeShader"
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
			};

			struct VertexOutput
			{
				float4 vertex : SV_POSITION;
				float3 color : TEXCOORD0;
			};

			uniform float4x4 _InvertedColorSpaceTransform;
			// uniform float4x4 _Object2World; // built in

			VertexOutput vert (VertexInput input)
			{
				VertexOutput output;
				output.color = mul(unity_ObjectToWorld, mul(_InvertedColorSpaceTransform, input.vertex)).xyz;

				output.vertex = UnityObjectToClipPos(input.vertex);
				return output;
			}
			
			fixed4 frag (VertexOutput output) : SV_Target
			{
				return fixed4(output.color + 0.5, 1.0);
			}
			ENDCG
		}
	}
}

Shader "Custom/Splat" {
	Properties {
		_Color ("Tint", Color) = (1, 1, 1, 1)
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" }
		LOD 200 
		Cull Off ZTest Always ZWrite Off Fog { Mode Off }
		Blend SrcAlpha OneMinusSrcAlpha

		CGINCLUDE
		#include "UnityCG.cginc"
		
		float4 _Color;

		struct appdata {
			float4 vertex : POSITION;
		};
		
		struct vs2ps {
			float4 vertex : POSITION;
		};

		vs2ps vert(appdata i) { 
			vs2ps o;
			o.vertex = mul(UNITY_MATRIX_MVP, i.vertex);
			return o;
		}
		
		float4 frag(vs2ps i) : COLOR {
			return _Color;
		}
		ENDCG
			
		Pass {
			ColorMask 0
			Stencil {
				ReadMask 1
				WriteMask 1
				PassFront Invert
				FailFront Invert
				ZFailFront Invert
				PassBack Invert
				FailBack Invert
				ZFailBack Invert
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
		
		Pass {
			ColorMask RGBA
			Stencil {
				Ref 1
				ReadMask 1
				WriteMask 1
				Comp Equal
			}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			ENDCG
		}
	} 
	FallBack "Diffuse"
}

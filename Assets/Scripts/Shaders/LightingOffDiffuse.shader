Shader "Custom/LightingOffDiffuse" {
	Properties{
		_MainColor("MainColor (RGB)", Color) = (0.3, 0.3, 0.3, 1)
		_MainTex("Base (RGB)", 2D) = "white" {}
	}
		SubShader{
		Tags{ "Queue" = "Geometry+500" "RenderType" = "Opaque" }
		LOD 200

		Pass{
				
				Material{
					Diffuse(1, 1, 1, 1)
					Ambient(1, 1, 1, 1)
				}
				Lighting Off
				SetTexture[_MainTex]{ ConstantColor[_MainColor] combine constant * texture }
			}

	}
	FallBack "Diffuse"
}

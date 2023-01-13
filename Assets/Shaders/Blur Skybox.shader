Shader "Custom/Skybox" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Tint("Tint Color", Color) = (.5, .5, .5, .5)
		[Gamma] _Exposure("Exposure", Range(0, 8)) = 1.0
		_Rotation("Rotation", Range(0, 360)) = 0
		[NoScaleOffset] _Cubemap("Cubemap   (HDR)", Cube) = "grey" {}
	}
		SubShader{
		Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
		Cull Off ZWrite Off

		Pass {

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0

			#include "UnityCG.cginc"

			samplerCUBE _Cubemap;
			half4 _Cubemap_HDR;
			half4 _Tint;
			half _Exposure;
			float _Rotation;

			sampler2D _MainTex;

			float3 RotateAroundYInDegrees(float3 vertex, float degrees)
			{
				float alpha = degrees * UNITY_PI / 180.0;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);
				return float3(mul(m, vertex.xz), vertex.y).xzy;
			}


			static const int BlurKernelSamples = 54;
			static const float3 BlurKernel[BlurKernelSamples] =
			{
				float3(1,1,1),
				float3(0,1,1),
				float3(-1,1,1),

				float3(1,0,1),
				float3(0,0,1),
				float3(-1,0,1),

				float3(1,-1,1),
				float3(0,-1,1),
				float3(-1,-1,1),

				//====================//

				float3(1,1,0),
				float3(0,1,0),
				float3(-1,1,0),

				float3(1,0,0),
				float3(0,0,0),
				float3(-1,0,0),

				float3(1,-1,0),
				float3(0,-1,0),
				float3(-1,-1,0),

				//====================//

				float3(1,1,-1),
				float3(0,1,-1),
				float3(-1,1,-1),

				float3(1,0,-1),
				float3(0,0,-1),
				float3(-1,0,-1),

				float3(1,-1,-1),
				float3(0,-1,-1),
				float3(-1,-1,-1),
				//====================//
				float3(2,2,2),
				float3(0,2,2),
				float3(-2,2,2),

				float3(2,0,2),
				float3(0,0,2),
				float3(-2,0,2),

				float3(2,-2,2),
				float3(0,-2,2),
				float3(-2,-2,2),

				//====================//

				float3(2,2,0),
				float3(0,2,0),
				float3(-2,2,0),

				float3(2,0,0),
				float3(0,0,0),
				float3(-2,0,0),

				float3(2,-2,0),
				float3(0,-2,0),
				float3(-2,-2,0),

				//====================//

				float3(2,2,-2),
				float3(0,2,-2),
				float3(-2,2,-2),

				float3(2,0,-2),
				float3(0,0,-2),
				float3(-2,0,-2),

				float3(2,-2,-2),
				float3(0,-2,-2),
				float3(-2,-2,-2)
			};

			struct appdata_t {
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				float3 texcoord : TEXCOORD8;
				float3 worldNormal : TEXCOORD7;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(appdata_t v)
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				float3 rotated = RotateAroundYInDegrees(v.vertex, _Rotation);
				o.vertex = UnityObjectToClipPos(rotated);
				o.texcoord = v.vertex.xyz;
				o.worldNormal = UnityObjectToWorldDir(v.normal.xyz);
				return o;
			}

			
			// fragment shader
			fixed4 frag(v2f IN) : SV_Target
			{

				fixed3 worldNormal = normalize(IN.worldNormal.xyz);

				float3 ambIBL = 0.0;
				for (int i = 0; i < BlurKernelSamples; i++)
				{
					ambIBL += texCUBE(_Cubemap, half4(IN.texcoord + BlurKernel[i] * 0.0075 , 1.0)).xyz;
				}
				ambIBL *= 1.0 / BlurKernelSamples;
				half4 tex = texCUBE(_Cubemap, IN.texcoord);
				half3 c = DecodeHDR(tex, _Cubemap_HDR);
				c = c * _Tint.rgb * unity_ColorSpaceDouble.rgb;
				c *= _Exposure;


				

				return float4( ambIBL,1.0 );
				//return float4( ambIBL,1.0 );
				
			}

			ENDCG
		}
		
		/*
		Pass
		{
			Name "DEFERRED"
			Tags { "LightMode" = "Deferred" }
			Fog {Mode Off}
			
			CGPROGRAM
			#pragma vertex vert_surf
			#pragma fragment frag_surf
			#pragma target 3.0
			
			#pragma exclude_renderers nomrt
			#pragma multi_compile_prepassfinal
			#pragma multi_compile _USE_BAKED_Cubemap_ON _USE_BAKED_Cubemap_OFF
			#define UNITY_PASS_DEFERRED
			
			#include "HLSLSupport.cginc"
			#include "UnityShaderVariables.cginc"
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
		
			sampler2D _MainTex;
			samplerCUBE _Cubemap;
			float _Blur;
			float _Factor;

			float _UseProbeTexture;
			
			uniform samplerCUBE _GlobalCubemap;
			uniform samplerCUBE _ProbeCubemap;
			
			#include "DNMST.cginc"

			static const int BlurKernelSamples = 27;		
			static const float3 BlurKernel[BlurKernelSamples] =
			{
				float3(1,1,1),
				float3(0,1,1),
				float3(-1,1,1),

				float3(1,0,1),
				float3(0,0,1),
				float3(-1,0,1),

				float3(1,-1,1),
				float3(0,-1,1),
				float3(-1,-1,1),

				//====================//

				float3(1,1,0),
				float3(0,1,0),
				float3(-1,1,0),

				float3(1,0,0),
				float3(0,0,0),
				float3(-1,0,0),

				float3(1,-1,0),
				float3(0,-1,0),
				float3(-1,-1,0),

				//====================//

				float3(1,1,-1),
				float3(0,1,-1),
				float3(-1,1,-1),

				float3(1,0,-1),
				float3(0,0,-1),
				float3(-1,0,-1),

				float3(1,-1,-1),
				float3(0,-1,-1),
				float3(-1,-1,-1)
			};
			
			void frag_surf (v2f_surf i, out half4 outDiffuse : SV_Target0, out half4 outMST : SV_Target1, out half4 outNormal : SV_Target2, out half4 outEmission : SV_Target3) {
				// Albedo comes from a texture tinted by color
				
				fixed3 localNormal = float3(0,0,1);

				fixed3 worldNormalBaked = normalize( i.localNormal.xyz );
				fixed3 worldNormal = normalize( float3( i.tSpace0.z, i.tSpace1.z, i.tSpace2.z ) );
				
				float3 worldPos = float3(i.tSpace0.w, i.tSpace1.w, i.tSpace2.w);
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );


				float3 ambIBL = 0.0;
				for( int i = 0; i < BlurKernelSamples; i++ ){
					ambIBL += texCUBElod(_ProbeCubemap, half4( worldNormal + BlurKernel[i] * 0.025 , 1.0 ) ).xyz;
				}
				ambIBL *= 1.0 / BlurKernelSamples;

				#if _USE_BAKED_Cubemap_ON
					float3 ambIBLbaked = 0.0;
					for( int i = 0; i < BlurKernelSamples; i++ ){
						ambIBLbaked += texCUBElod(_GlobalCubemap, half4( worldNormalBaked + BlurKernel[i] * 0.025 , 3.0 ) ).xyz;
					}
					ambIBLbaked *= 1.0 / BlurKernelSamples;

					ambIBL = lerp( ambIBLbaked, ambIBL, _UseProbeTexture );
				#endif

				
				SurfaceOutputStandard surfOut;
				surfOut.Albedo = float3(0,0,0);
				surfOut.Normal = worldNormal;
				surfOut.Metallic = 0;
				surfOut.Smoothness = 0;
				surfOut.Transmission = 0;
				surfOut.Emission =  ambIBL * _Factor;
				surfOut.Motion = float2(1,1);//float2(0.5,0.5);
				surfOut.Alpha = 1.0;
				surfOut.Occlusion = 0;
				
				ReturnOutput ( surfOut, worldPos, worldViewDir, i, outDiffuse, outMST, outNormal, outEmission );
				
			}
			ENDCG
		} 
		*/
	}
	FallBack "Diffuse"
}

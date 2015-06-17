    Shader "Custom/VertexColorPlusDiffusePlusShadows" {
        Properties {
        _Color ("Diffuse Color", Color) = (1.0, 1.0, 1.0, 1.0)
        }
        SubShader {
        Tags { "RenderType" = "Opaque" }
        Pass {
            Tags { "LightMode" = "ForwardBase" }
     
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            //#pragma multi_compile_fwdbase
                #pragma multi_compile_fwdadd_fullshadows
            //#pragma fragmentoption ARB_precision_hint_fastest
     
     
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"
     
            uniform float4 _Color;
     
            struct vertex_input {
            float4 vertex : POSITION;
            float4 color : COLOR;
            float3 normal : NORMAL;
            };
     
            struct vertex_output {
            float4 pos : POSITION;
            float4 color : COLOR;
            //float4 _ShadowCoord : TEXCOORD3;
            LIGHTING_COORDS(3, 4)
            };
     
            struct fragment_output {
            float4 color : COLOR;
            };
     
            vertex_output vert(vertex_input v) {
            vertex_output o;
            // convert the local position to world position
            o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
     
            float3x3 model_matrix = _Object2World;
            float3x3 model_matrix_inverse = _World2Object;
            // calculate the diffuse lighting
            float3 normal_dir = normalize(
				float3(
					mul(
						float4(
							v.normal, 0.0
						),
						model_matrix_inverse
					)
				)
			);
            float3 light_dir = normalize(float3(_WorldSpaceLightPos0.xyz));
            float3 diffuse_reflection = float3(
				_LightColor0.xyz
			) * float3(
				_Color.xyz
			) * max(
				0.0, dot(
					normal_dir, light_dir
				)
			);
     
            // combine the diffuse lighting from the light source
                    // with the vertex color passed in by the mesh generator
            /* OUT.color = (IN.color + float4(diffuse_reflection, 1.0)) * 0.5; */
            o.color = lerp(v.color, float4(diffuse_reflection, 1.0), 0.5);
            TRANSFER_VERTEX_TO_FRAGMENT(o);
     
            return o;
            };
     
     
            fragment_output frag(vertex_output IN) {
            fragment_output OUT;
     
            float atten = LIGHT_ATTENUATION(IN);
            OUT.color = IN.color * atten;
     
            return OUT;
            };
            ENDCG
     
        }
        }
     
        Fallback "Diffuse"//, 2
    }

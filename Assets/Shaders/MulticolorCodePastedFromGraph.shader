Shader "Custom/MulticolorCodePastedFromGraph"
{
    Properties
    {
        [NoScaleOffset]_MainTex("Texture2D", 2D) = "white" {}
        _Color("Color", Color) = (0.8792453, 0.2770452, 0.7626457, 0)
        _Strength("Strength", Float) = 1
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue"="Transparent"
            // DisableBatching: <None>
            "ShaderGraphShader"="true"
            "ShaderGraphTargetId"="UniversalSpriteLitSubTarget"
        }
        Pass
        {
            Name "Sprite Lit"
            Tags
            {
                "LightMode" = "Universal2D"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define VARYINGS_NEED_SCREENPOSITION
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITELIT
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
             float4 screenPosition;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 WorldSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float4 screenPosition : INTERP2;
             float3 positionWS : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.screenPosition.xyzw = input.screenPosition;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.screenPosition = input.screenPosition.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _Color;
        float _Strength;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float3(float3 In, out float3 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_ReplaceColor_float(float3 In, float3 From, float3 To, float Range, out float3 Out, float Fuzziness)
        {
            float Distance = distance(From, In);
            Out = lerp(To, In, saturate((Distance - Range) / max(Fuzziness, 1e-5f)));
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float4 SpriteMask;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.tex, _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.samplerstate, _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_R_4_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.r;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_G_5_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.g;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_B_6_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.b;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_A_7_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.a;
            float _Property_792f2f07a10d474e95631bc7a4025ada_Out_0_Float = _Strength;
            float _Subtract_965772b390354b48829894c25ae4a013_Out_2_Float;
            Unity_Subtract_float(1, _Property_792f2f07a10d474e95631bc7a4025ada_Out_0_Float, _Subtract_965772b390354b48829894c25ae4a013_Out_2_Float);
            float4 _Multiply_ce455a6ee67b4e78bac6d2a1aea50f71_Out_2_Vector4;
            Unity_Multiply_float4_float4(_SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4, (_Subtract_965772b390354b48829894c25ae4a013_Out_2_Float.xxxx), _Multiply_ce455a6ee67b4e78bac6d2a1aea50f71_Out_2_Vector4);
            float3 _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3;
            Unity_Normalize_float3(IN.WorldSpacePosition, _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3);
            float _Split_ba8db03d16614505af00e880ae68c017_R_1_Float = _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3[0];
            float _Split_ba8db03d16614505af00e880ae68c017_G_2_Float = _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3[1];
            float _Split_ba8db03d16614505af00e880ae68c017_B_3_Float = _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3[2];
            float _Split_ba8db03d16614505af00e880ae68c017_A_4_Float = 0;
            float _Remap_7e4f2ccc7ff449f4a4ac54f27f0eb849_Out_3_Float;
            Unity_Remap_float(_Split_ba8db03d16614505af00e880ae68c017_G_2_Float, float2 (-1, 1), float2 (0, 1), _Remap_7e4f2ccc7ff449f4a4ac54f27f0eb849_Out_3_Float);
            float _Multiply_1c5192494227497a8de5b809dc2a1e2e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 2, _Multiply_1c5192494227497a8de5b809dc2a1e2e_Out_2_Float);
            float _Sine_c857f4fb8e094e739b463800f2df6ad5_Out_1_Float;
            Unity_Sine_float(_Multiply_1c5192494227497a8de5b809dc2a1e2e_Out_2_Float, _Sine_c857f4fb8e094e739b463800f2df6ad5_Out_1_Float);
            float _Subtract_d7010fa1ff7745878333ef06e1091b4a_Out_2_Float;
            Unity_Subtract_float(_Sine_c857f4fb8e094e739b463800f2df6ad5_Out_1_Float, 0.5, _Subtract_d7010fa1ff7745878333ef06e1091b4a_Out_2_Float);
            float _Multiply_5e382b0f91e54b92ab7be550188c439d_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 10, _Multiply_5e382b0f91e54b92ab7be550188c439d_Out_2_Float);
            float _Sine_5b449aec6a37416aac184ebc6d0c1e93_Out_1_Float;
            Unity_Sine_float(_Multiply_5e382b0f91e54b92ab7be550188c439d_Out_2_Float, _Sine_5b449aec6a37416aac184ebc6d0c1e93_Out_1_Float);
            float _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float;
            Unity_Add_float(_Subtract_d7010fa1ff7745878333ef06e1091b4a_Out_2_Float, _Sine_5b449aec6a37416aac184ebc6d0c1e93_Out_1_Float, _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float);
            float _Add_a78514c95c5a4be18b303c0e88c4e2be_Out_2_Float;
            Unity_Add_float(_Remap_7e4f2ccc7ff449f4a4ac54f27f0eb849_Out_3_Float, _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float, _Add_a78514c95c5a4be18b303c0e88c4e2be_Out_2_Float);
            float _Remap_a3700f835bb34e18b0c8951b07fb46eb_Out_3_Float;
            Unity_Remap_float(_Split_ba8db03d16614505af00e880ae68c017_B_3_Float, float2 (-1, 1), float2 (0, 1), _Remap_a3700f835bb34e18b0c8951b07fb46eb_Out_3_Float);
            float _Add_bf0343bea5b94fceafed14bce048d1ee_Out_2_Float;
            Unity_Add_float(_Remap_a3700f835bb34e18b0c8951b07fb46eb_Out_3_Float, _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float, _Add_bf0343bea5b94fceafed14bce048d1ee_Out_2_Float);
            float _Multiply_413c69145f7247449561d47f769b8393_Out_2_Float;
            Unity_Multiply_float_float(_Add_a78514c95c5a4be18b303c0e88c4e2be_Out_2_Float, _Add_bf0343bea5b94fceafed14bce048d1ee_Out_2_Float, _Multiply_413c69145f7247449561d47f769b8393_Out_2_Float);
            float4 _SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4;
            Unity_SampleGradientV1_float(NewGradient(0, 5, 2, float4(1, 0.09803922, 0.09803922, 0),float4(1, 0.8836132, 0.09803921, 0.2500038),float4(0.2596475, 1, 0.09803921, 0.5000076),float4(0.09803921, 0.4967062, 1, 0.7499962),float4(1, 0.09803921, 0.9012939, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0)), _Multiply_413c69145f7247449561d47f769b8393_Out_2_Float, _SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4);
            float3 _ReplaceColor_696516128de74b608ad9f511bc36a19b_Out_4_Vector3;
            Unity_ReplaceColor_float((_SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.xyz), (_SampleTexture2D_9913e521cadd47508329f9578e0c3630_B_6_Float.xxx), (_SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4.xyz), 0, _ReplaceColor_696516128de74b608ad9f511bc36a19b_Out_4_Vector3, 0);
            float4 _Multiply_a5b9b0cdde8a45d383731c79564eb0c5_Out_2_Vector4;
            Unity_Multiply_float4_float4(_SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4, float4(2, 2, 2, 2), _Multiply_a5b9b0cdde8a45d383731c79564eb0c5_Out_2_Vector4);
            float3 _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3;
            Unity_ReplaceColor_float(_ReplaceColor_696516128de74b608ad9f511bc36a19b_Out_4_Vector3, (_SampleTexture2D_9913e521cadd47508329f9578e0c3630_G_5_Float.xxx), (_Multiply_a5b9b0cdde8a45d383731c79564eb0c5_Out_2_Vector4.xyz), 0, _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3, 0);
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_R_1_Float = _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3[0];
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_G_2_Float = _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3[1];
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_B_3_Float = _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3[2];
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_A_4_Float = 0;
            float4 _Combine_29816d8381184fa58274f820a264c8c1_RGBA_4_Vector4;
            float3 _Combine_29816d8381184fa58274f820a264c8c1_RGB_5_Vector3;
            float2 _Combine_29816d8381184fa58274f820a264c8c1_RG_6_Vector2;
            Unity_Combine_float(_Split_ee582f67ce2b4621b00c2e3522ac9229_R_1_Float, _Split_ee582f67ce2b4621b00c2e3522ac9229_G_2_Float, _Split_ee582f67ce2b4621b00c2e3522ac9229_B_3_Float, 0, _Combine_29816d8381184fa58274f820a264c8c1_RGBA_4_Vector4, _Combine_29816d8381184fa58274f820a264c8c1_RGB_5_Vector3, _Combine_29816d8381184fa58274f820a264c8c1_RG_6_Vector2);
            float4 _Multiply_5de57d754ff443259fd0cc6e44deca99_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Combine_29816d8381184fa58274f820a264c8c1_RGBA_4_Vector4, (_Property_792f2f07a10d474e95631bc7a4025ada_Out_0_Float.xxxx), _Multiply_5de57d754ff443259fd0cc6e44deca99_Out_2_Vector4);
            float4 _Add_87b475af20a740fd948384c97cb88a03_Out_2_Vector4;
            Unity_Add_float4(_Multiply_ce455a6ee67b4e78bac6d2a1aea50f71_Out_2_Vector4, _Multiply_5de57d754ff443259fd0cc6e44deca99_Out_2_Vector4, _Add_87b475af20a740fd948384c97cb88a03_Out_2_Vector4);
            surface.BaseColor = (_Add_87b475af20a740fd948384c97cb88a03_Out_2_Vector4.xyz);
            surface.Alpha = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_A_7_Float;
            surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteLitPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Sprite Normal"
            Tags
            {
                "LightMode" = "NormalsRendering"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITENORMAL
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float3 normalWS;
             float4 tangentWS;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float3 WorldSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 tangentWS : INTERP0;
             float4 texCoord0 : INTERP1;
             float3 positionWS : INTERP2;
             float3 normalWS : INTERP3;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.tangentWS.xyzw = input.tangentWS;
            output.texCoord0.xyzw = input.texCoord0;
            output.positionWS.xyz = input.positionWS;
            output.normalWS.xyz = input.normalWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.tangentWS = input.tangentWS.xyzw;
            output.texCoord0 = input.texCoord0.xyzw;
            output.positionWS = input.positionWS.xyz;
            output.normalWS = input.normalWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _Color;
        float _Strength;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float3(float3 In, out float3 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_ReplaceColor_float(float3 In, float3 From, float3 To, float Range, out float3 Out, float Fuzziness)
        {
            float Distance = distance(From, In);
            Out = lerp(To, In, saturate((Distance - Range) / max(Fuzziness, 1e-5f)));
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.tex, _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.samplerstate, _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_R_4_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.r;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_G_5_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.g;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_B_6_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.b;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_A_7_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.a;
            float _Property_792f2f07a10d474e95631bc7a4025ada_Out_0_Float = _Strength;
            float _Subtract_965772b390354b48829894c25ae4a013_Out_2_Float;
            Unity_Subtract_float(1, _Property_792f2f07a10d474e95631bc7a4025ada_Out_0_Float, _Subtract_965772b390354b48829894c25ae4a013_Out_2_Float);
            float4 _Multiply_ce455a6ee67b4e78bac6d2a1aea50f71_Out_2_Vector4;
            Unity_Multiply_float4_float4(_SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4, (_Subtract_965772b390354b48829894c25ae4a013_Out_2_Float.xxxx), _Multiply_ce455a6ee67b4e78bac6d2a1aea50f71_Out_2_Vector4);
            float3 _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3;
            Unity_Normalize_float3(IN.WorldSpacePosition, _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3);
            float _Split_ba8db03d16614505af00e880ae68c017_R_1_Float = _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3[0];
            float _Split_ba8db03d16614505af00e880ae68c017_G_2_Float = _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3[1];
            float _Split_ba8db03d16614505af00e880ae68c017_B_3_Float = _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3[2];
            float _Split_ba8db03d16614505af00e880ae68c017_A_4_Float = 0;
            float _Remap_7e4f2ccc7ff449f4a4ac54f27f0eb849_Out_3_Float;
            Unity_Remap_float(_Split_ba8db03d16614505af00e880ae68c017_G_2_Float, float2 (-1, 1), float2 (0, 1), _Remap_7e4f2ccc7ff449f4a4ac54f27f0eb849_Out_3_Float);
            float _Multiply_1c5192494227497a8de5b809dc2a1e2e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 2, _Multiply_1c5192494227497a8de5b809dc2a1e2e_Out_2_Float);
            float _Sine_c857f4fb8e094e739b463800f2df6ad5_Out_1_Float;
            Unity_Sine_float(_Multiply_1c5192494227497a8de5b809dc2a1e2e_Out_2_Float, _Sine_c857f4fb8e094e739b463800f2df6ad5_Out_1_Float);
            float _Subtract_d7010fa1ff7745878333ef06e1091b4a_Out_2_Float;
            Unity_Subtract_float(_Sine_c857f4fb8e094e739b463800f2df6ad5_Out_1_Float, 0.5, _Subtract_d7010fa1ff7745878333ef06e1091b4a_Out_2_Float);
            float _Multiply_5e382b0f91e54b92ab7be550188c439d_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 10, _Multiply_5e382b0f91e54b92ab7be550188c439d_Out_2_Float);
            float _Sine_5b449aec6a37416aac184ebc6d0c1e93_Out_1_Float;
            Unity_Sine_float(_Multiply_5e382b0f91e54b92ab7be550188c439d_Out_2_Float, _Sine_5b449aec6a37416aac184ebc6d0c1e93_Out_1_Float);
            float _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float;
            Unity_Add_float(_Subtract_d7010fa1ff7745878333ef06e1091b4a_Out_2_Float, _Sine_5b449aec6a37416aac184ebc6d0c1e93_Out_1_Float, _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float);
            float _Add_a78514c95c5a4be18b303c0e88c4e2be_Out_2_Float;
            Unity_Add_float(_Remap_7e4f2ccc7ff449f4a4ac54f27f0eb849_Out_3_Float, _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float, _Add_a78514c95c5a4be18b303c0e88c4e2be_Out_2_Float);
            float _Remap_a3700f835bb34e18b0c8951b07fb46eb_Out_3_Float;
            Unity_Remap_float(_Split_ba8db03d16614505af00e880ae68c017_B_3_Float, float2 (-1, 1), float2 (0, 1), _Remap_a3700f835bb34e18b0c8951b07fb46eb_Out_3_Float);
            float _Add_bf0343bea5b94fceafed14bce048d1ee_Out_2_Float;
            Unity_Add_float(_Remap_a3700f835bb34e18b0c8951b07fb46eb_Out_3_Float, _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float, _Add_bf0343bea5b94fceafed14bce048d1ee_Out_2_Float);
            float _Multiply_413c69145f7247449561d47f769b8393_Out_2_Float;
            Unity_Multiply_float_float(_Add_a78514c95c5a4be18b303c0e88c4e2be_Out_2_Float, _Add_bf0343bea5b94fceafed14bce048d1ee_Out_2_Float, _Multiply_413c69145f7247449561d47f769b8393_Out_2_Float);
            float4 _SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4;
            Unity_SampleGradientV1_float(NewGradient(0, 5, 2, float4(1, 0.09803922, 0.09803922, 0),float4(1, 0.8836132, 0.09803921, 0.2500038),float4(0.2596475, 1, 0.09803921, 0.5000076),float4(0.09803921, 0.4967062, 1, 0.7499962),float4(1, 0.09803921, 0.9012939, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0)), _Multiply_413c69145f7247449561d47f769b8393_Out_2_Float, _SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4);
            float3 _ReplaceColor_696516128de74b608ad9f511bc36a19b_Out_4_Vector3;
            Unity_ReplaceColor_float((_SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.xyz), (_SampleTexture2D_9913e521cadd47508329f9578e0c3630_B_6_Float.xxx), (_SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4.xyz), 0, _ReplaceColor_696516128de74b608ad9f511bc36a19b_Out_4_Vector3, 0);
            float4 _Multiply_a5b9b0cdde8a45d383731c79564eb0c5_Out_2_Vector4;
            Unity_Multiply_float4_float4(_SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4, float4(2, 2, 2, 2), _Multiply_a5b9b0cdde8a45d383731c79564eb0c5_Out_2_Vector4);
            float3 _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3;
            Unity_ReplaceColor_float(_ReplaceColor_696516128de74b608ad9f511bc36a19b_Out_4_Vector3, (_SampleTexture2D_9913e521cadd47508329f9578e0c3630_G_5_Float.xxx), (_Multiply_a5b9b0cdde8a45d383731c79564eb0c5_Out_2_Vector4.xyz), 0, _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3, 0);
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_R_1_Float = _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3[0];
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_G_2_Float = _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3[1];
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_B_3_Float = _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3[2];
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_A_4_Float = 0;
            float4 _Combine_29816d8381184fa58274f820a264c8c1_RGBA_4_Vector4;
            float3 _Combine_29816d8381184fa58274f820a264c8c1_RGB_5_Vector3;
            float2 _Combine_29816d8381184fa58274f820a264c8c1_RG_6_Vector2;
            Unity_Combine_float(_Split_ee582f67ce2b4621b00c2e3522ac9229_R_1_Float, _Split_ee582f67ce2b4621b00c2e3522ac9229_G_2_Float, _Split_ee582f67ce2b4621b00c2e3522ac9229_B_3_Float, 0, _Combine_29816d8381184fa58274f820a264c8c1_RGBA_4_Vector4, _Combine_29816d8381184fa58274f820a264c8c1_RGB_5_Vector3, _Combine_29816d8381184fa58274f820a264c8c1_RG_6_Vector2);
            float4 _Multiply_5de57d754ff443259fd0cc6e44deca99_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Combine_29816d8381184fa58274f820a264c8c1_RGBA_4_Vector4, (_Property_792f2f07a10d474e95631bc7a4025ada_Out_0_Float.xxxx), _Multiply_5de57d754ff443259fd0cc6e44deca99_Out_2_Vector4);
            float4 _Add_87b475af20a740fd948384c97cb88a03_Out_2_Vector4;
            Unity_Add_float4(_Multiply_ce455a6ee67b4e78bac6d2a1aea50f71_Out_2_Vector4, _Multiply_5de57d754ff443259fd0cc6e44deca99_Out_2_Vector4, _Add_87b475af20a740fd948384c97cb88a03_Out_2_Vector4);
            surface.BaseColor = (_Add_87b475af20a740fd948384c97cb88a03_Out_2_Vector4.xyz);
            surface.Alpha = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_A_7_Float;
            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteNormalPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "SceneSelectionPass"
            Tags
            {
                "LightMode" = "SceneSelectionPass"
            }
        
        // Render State
        Cull Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENESELECTIONPASS 1
        
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _Color;
        float _Strength;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.tex, _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.samplerstate, _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_R_4_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.r;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_G_5_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.g;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_B_6_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.b;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_A_7_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.a;
            surface.Alpha = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_A_7_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "ScenePickingPass"
            Tags
            {
                "LightMode" = "Picking"
            }
        
        // Render State
        Cull Back
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        // PassKeywords: <None>
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
        #define SCENEPICKINGPASS 1
        
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DOTS.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float4 uv0;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _Color;
        float _Strength;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        // GraphFunctions: <None>
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float Alpha;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.tex, _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.samplerstate, _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_R_4_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.r;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_G_5_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.g;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_B_6_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.b;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_A_7_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.a;
            surface.Alpha = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_A_7_Float;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
        
        
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
        Pass
        {
            Name "Sprite Forward"
            Tags
            {
                "LightMode" = "UniversalForward"
            }
        
        // Render State
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off
        
        // Debug
        // <None>
        
        // --------------------------------------------------
        // Pass
        
        HLSLPROGRAM
        
        // Pragmas
        #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag
        
        // Keywords
        #pragma multi_compile_fragment _ DEBUG_DISPLAY
        // GraphKeywords: <None>
        
        // Defines
        
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEFORWARD
        
        
        // custom interpolator pre-include
        /* WARNING: $splice Could not find named fragment 'sgci_CustomInterpolatorPreInclude' */
        
        // Includes
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Input.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include_with_pragmas "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRenderingKeywords.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/FoveatedRendering.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        
        // --------------------------------------------------
        // Structs and Packing
        
        // custom interpolators pre packing
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */
        
        struct Attributes
        {
             float3 positionOS : POSITION;
             float3 normalOS : NORMAL;
             float4 tangentOS : TANGENT;
             float4 uv0 : TEXCOORD0;
             float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
             float4 positionCS : SV_POSITION;
             float3 positionWS;
             float4 texCoord0;
             float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
             float3 TangentSpaceNormal;
             float3 WorldSpacePosition;
             float4 uv0;
             float3 TimeParameters;
        };
        struct VertexDescriptionInputs
        {
             float3 ObjectSpaceNormal;
             float3 ObjectSpaceTangent;
             float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
             float4 positionCS : SV_POSITION;
             float4 texCoord0 : INTERP0;
             float4 color : INTERP1;
             float3 positionWS : INTERP2;
            #if UNITY_ANY_INSTANCING_ENABLED
             uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
             uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
             uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
             FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        
        PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            ZERO_INITIALIZE(PackedVaryings, output);
            output.positionCS = input.positionCS;
            output.texCoord0.xyzw = input.texCoord0;
            output.color.xyzw = input.color;
            output.positionWS.xyz = input.positionWS;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.texCoord0.xyzw;
            output.color = input.color.xyzw;
            output.positionWS = input.positionWS.xyz;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        
        
        // --------------------------------------------------
        // Graph
        
        // Graph Properties
        CBUFFER_START(UnityPerMaterial)
        float4 _MainTex_TexelSize;
        float4 _Color;
        float _Strength;
        CBUFFER_END
        
        
        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(_MainTex);
        SAMPLER(sampler_MainTex);
        
        // Graph Includes
        // GraphIncludes: <None>
        
        // -- Property used by ScenePickingPass
        #ifdef SCENEPICKINGPASS
        float4 _SelectionID;
        #endif
        
        // -- Properties used by SceneSelectionPass
        #ifdef SCENESELECTIONPASS
        int _ObjectId;
        int _PassValue;
        #endif
        
        // Graph Functions
        
        void Unity_Subtract_float(float A, float B, out float Out)
        {
            Out = A - B;
        }
        
        void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }
        
        void Unity_Normalize_float3(float3 In, out float3 Out)
        {
            Out = normalize(In);
        }
        
        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }
        
        void Unity_Multiply_float_float(float A, float B, out float Out)
        {
            Out = A * B;
        }
        
        void Unity_Sine_float(float In, out float Out)
        {
            Out = sin(In);
        }
        
        void Unity_Add_float(float A, float B, out float Out)
        {
            Out = A + B;
        }
        
        void Unity_SampleGradientV1_float(Gradient Gradient, float Time, out float4 Out)
        {
            float3 color = Gradient.colors[0].rgb;
            [unroll]
            for (int c = 1; c < Gradient.colorsLength; c++)
            {
                float colorPos = saturate((Time - Gradient.colors[c - 1].w) / (Gradient.colors[c].w - Gradient.colors[c - 1].w)) * step(c, Gradient.colorsLength - 1);
                color = lerp(color, Gradient.colors[c].rgb, lerp(colorPos, step(0.01, colorPos), Gradient.type));
            }
        #ifdef UNITY_COLORSPACE_GAMMA
            color = LinearToSRGB(color);
        #endif
            float alpha = Gradient.alphas[0].x;
            [unroll]
            for (int a = 1; a < Gradient.alphasLength; a++)
            {
                float alphaPos = saturate((Time - Gradient.alphas[a - 1].y) / (Gradient.alphas[a].y - Gradient.alphas[a - 1].y)) * step(a, Gradient.alphasLength - 1);
                alpha = lerp(alpha, Gradient.alphas[a].x, lerp(alphaPos, step(0.01, alphaPos), Gradient.type));
            }
            Out = float4(color, alpha);
        }
        
        void Unity_ReplaceColor_float(float3 In, float3 From, float3 To, float Range, out float3 Out, float Fuzziness)
        {
            float Distance = distance(From, In);
            Out = lerp(To, In, saturate((Distance - Range) / max(Fuzziness, 1e-5f)));
        }
        
        void Unity_Combine_float(float R, float G, float B, float A, out float4 RGBA, out float3 RGB, out float2 RG)
        {
            RGBA = float4(R, G, B, A);
            RGB = float3(R, G, B);
            RG = float2(R, G);
        }
        
        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }
        
        // Custom interpolators pre vertex
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */
        
        // Graph Vertex
        struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };
        
        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }
        
        // Custom interpolators, pre surface
        #ifdef FEATURES_GRAPH_VERTEX
        Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
        {
        return output;
        }
        #define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
        #endif
        
        // Graph Pixel
        struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
            float3 NormalTS;
        };
        
        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D = UnityBuildTexture2DStructNoScale(_MainTex);
            float4 _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4 = SAMPLE_TEXTURE2D(_Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.tex, _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.samplerstate, _Property_76e2b553c10540c4958294055505a908_Out_0_Texture2D.GetTransformedUV(IN.uv0.xy) );
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_R_4_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.r;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_G_5_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.g;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_B_6_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.b;
            float _SampleTexture2D_9913e521cadd47508329f9578e0c3630_A_7_Float = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.a;
            float _Property_792f2f07a10d474e95631bc7a4025ada_Out_0_Float = _Strength;
            float _Subtract_965772b390354b48829894c25ae4a013_Out_2_Float;
            Unity_Subtract_float(1, _Property_792f2f07a10d474e95631bc7a4025ada_Out_0_Float, _Subtract_965772b390354b48829894c25ae4a013_Out_2_Float);
            float4 _Multiply_ce455a6ee67b4e78bac6d2a1aea50f71_Out_2_Vector4;
            Unity_Multiply_float4_float4(_SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4, (_Subtract_965772b390354b48829894c25ae4a013_Out_2_Float.xxxx), _Multiply_ce455a6ee67b4e78bac6d2a1aea50f71_Out_2_Vector4);
            float3 _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3;
            Unity_Normalize_float3(IN.WorldSpacePosition, _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3);
            float _Split_ba8db03d16614505af00e880ae68c017_R_1_Float = _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3[0];
            float _Split_ba8db03d16614505af00e880ae68c017_G_2_Float = _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3[1];
            float _Split_ba8db03d16614505af00e880ae68c017_B_3_Float = _Normalize_7870035a32dd4bb2ba91160f6df825e6_Out_1_Vector3[2];
            float _Split_ba8db03d16614505af00e880ae68c017_A_4_Float = 0;
            float _Remap_7e4f2ccc7ff449f4a4ac54f27f0eb849_Out_3_Float;
            Unity_Remap_float(_Split_ba8db03d16614505af00e880ae68c017_G_2_Float, float2 (-1, 1), float2 (0, 1), _Remap_7e4f2ccc7ff449f4a4ac54f27f0eb849_Out_3_Float);
            float _Multiply_1c5192494227497a8de5b809dc2a1e2e_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 2, _Multiply_1c5192494227497a8de5b809dc2a1e2e_Out_2_Float);
            float _Sine_c857f4fb8e094e739b463800f2df6ad5_Out_1_Float;
            Unity_Sine_float(_Multiply_1c5192494227497a8de5b809dc2a1e2e_Out_2_Float, _Sine_c857f4fb8e094e739b463800f2df6ad5_Out_1_Float);
            float _Subtract_d7010fa1ff7745878333ef06e1091b4a_Out_2_Float;
            Unity_Subtract_float(_Sine_c857f4fb8e094e739b463800f2df6ad5_Out_1_Float, 0.5, _Subtract_d7010fa1ff7745878333ef06e1091b4a_Out_2_Float);
            float _Multiply_5e382b0f91e54b92ab7be550188c439d_Out_2_Float;
            Unity_Multiply_float_float(IN.TimeParameters.x, 10, _Multiply_5e382b0f91e54b92ab7be550188c439d_Out_2_Float);
            float _Sine_5b449aec6a37416aac184ebc6d0c1e93_Out_1_Float;
            Unity_Sine_float(_Multiply_5e382b0f91e54b92ab7be550188c439d_Out_2_Float, _Sine_5b449aec6a37416aac184ebc6d0c1e93_Out_1_Float);
            float _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float;
            Unity_Add_float(_Subtract_d7010fa1ff7745878333ef06e1091b4a_Out_2_Float, _Sine_5b449aec6a37416aac184ebc6d0c1e93_Out_1_Float, _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float);
            float _Add_a78514c95c5a4be18b303c0e88c4e2be_Out_2_Float;
            Unity_Add_float(_Remap_7e4f2ccc7ff449f4a4ac54f27f0eb849_Out_3_Float, _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float, _Add_a78514c95c5a4be18b303c0e88c4e2be_Out_2_Float);
            float _Remap_a3700f835bb34e18b0c8951b07fb46eb_Out_3_Float;
            Unity_Remap_float(_Split_ba8db03d16614505af00e880ae68c017_B_3_Float, float2 (-1, 1), float2 (0, 1), _Remap_a3700f835bb34e18b0c8951b07fb46eb_Out_3_Float);
            float _Add_bf0343bea5b94fceafed14bce048d1ee_Out_2_Float;
            Unity_Add_float(_Remap_a3700f835bb34e18b0c8951b07fb46eb_Out_3_Float, _Add_f57033a9b8574ccfa3ede8f3eb41fe0c_Out_2_Float, _Add_bf0343bea5b94fceafed14bce048d1ee_Out_2_Float);
            float _Multiply_413c69145f7247449561d47f769b8393_Out_2_Float;
            Unity_Multiply_float_float(_Add_a78514c95c5a4be18b303c0e88c4e2be_Out_2_Float, _Add_bf0343bea5b94fceafed14bce048d1ee_Out_2_Float, _Multiply_413c69145f7247449561d47f769b8393_Out_2_Float);
            float4 _SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4;
            Unity_SampleGradientV1_float(NewGradient(0, 5, 2, float4(1, 0.09803922, 0.09803922, 0),float4(1, 0.8836132, 0.09803921, 0.2500038),float4(0.2596475, 1, 0.09803921, 0.5000076),float4(0.09803921, 0.4967062, 1, 0.7499962),float4(1, 0.09803921, 0.9012939, 1),float4(0, 0, 0, 0),float4(0, 0, 0, 0),float4(0, 0, 0, 0), float2(1, 0),float2(1, 1),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0),float2(0, 0)), _Multiply_413c69145f7247449561d47f769b8393_Out_2_Float, _SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4);
            float3 _ReplaceColor_696516128de74b608ad9f511bc36a19b_Out_4_Vector3;
            Unity_ReplaceColor_float((_SampleTexture2D_9913e521cadd47508329f9578e0c3630_RGBA_0_Vector4.xyz), (_SampleTexture2D_9913e521cadd47508329f9578e0c3630_B_6_Float.xxx), (_SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4.xyz), 0, _ReplaceColor_696516128de74b608ad9f511bc36a19b_Out_4_Vector3, 0);
            float4 _Multiply_a5b9b0cdde8a45d383731c79564eb0c5_Out_2_Vector4;
            Unity_Multiply_float4_float4(_SampleGradient_5842b753ce9f47f4b722d388f4be19f2_Out_2_Vector4, float4(2, 2, 2, 2), _Multiply_a5b9b0cdde8a45d383731c79564eb0c5_Out_2_Vector4);
            float3 _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3;
            Unity_ReplaceColor_float(_ReplaceColor_696516128de74b608ad9f511bc36a19b_Out_4_Vector3, (_SampleTexture2D_9913e521cadd47508329f9578e0c3630_G_5_Float.xxx), (_Multiply_a5b9b0cdde8a45d383731c79564eb0c5_Out_2_Vector4.xyz), 0, _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3, 0);
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_R_1_Float = _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3[0];
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_G_2_Float = _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3[1];
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_B_3_Float = _ReplaceColor_690691b20d6b4ddca0dce394b0b6e974_Out_4_Vector3[2];
            float _Split_ee582f67ce2b4621b00c2e3522ac9229_A_4_Float = 0;
            float4 _Combine_29816d8381184fa58274f820a264c8c1_RGBA_4_Vector4;
            float3 _Combine_29816d8381184fa58274f820a264c8c1_RGB_5_Vector3;
            float2 _Combine_29816d8381184fa58274f820a264c8c1_RG_6_Vector2;
            Unity_Combine_float(_Split_ee582f67ce2b4621b00c2e3522ac9229_R_1_Float, _Split_ee582f67ce2b4621b00c2e3522ac9229_G_2_Float, _Split_ee582f67ce2b4621b00c2e3522ac9229_B_3_Float, 0, _Combine_29816d8381184fa58274f820a264c8c1_RGBA_4_Vector4, _Combine_29816d8381184fa58274f820a264c8c1_RGB_5_Vector3, _Combine_29816d8381184fa58274f820a264c8c1_RG_6_Vector2);
            float4 _Multiply_5de57d754ff443259fd0cc6e44deca99_Out_2_Vector4;
            Unity_Multiply_float4_float4(_Combine_29816d8381184fa58274f820a264c8c1_RGBA_4_Vector4, (_Property_792f2f07a10d474e95631bc7a4025ada_Out_0_Float.xxxx), _Multiply_5de57d754ff443259fd0cc6e44deca99_Out_2_Vector4);
            float4 _Add_87b475af20a740fd948384c97cb88a03_Out_2_Vector4;
            Unity_Add_float4(_Multiply_ce455a6ee67b4e78bac6d2a1aea50f71_Out_2_Vector4, _Multiply_5de57d754ff443259fd0cc6e44deca99_Out_2_Vector4, _Add_87b475af20a740fd948384c97cb88a03_Out_2_Vector4);
            surface.BaseColor = (_Add_87b475af20a740fd948384c97cb88a03_Out_2_Vector4.xyz);
            surface.Alpha = _SampleTexture2D_9913e521cadd47508329f9578e0c3630_A_7_Float;
            surface.NormalTS = IN.TangentSpaceNormal;
            return surface;
        }
        
        // --------------------------------------------------
        // Build Graph Inputs
        #ifdef HAVE_VFX_MODIFICATION
        #define VFX_SRP_ATTRIBUTES Attributes
        #define VFX_SRP_VARYINGS Varyings
        #define VFX_SRP_SURFACE_INPUTS SurfaceDescriptionInputs
        #endif
        VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);
        
            output.ObjectSpaceNormal =                          input.normalOS;
            output.ObjectSpaceTangent =                         input.tangentOS.xyz;
            output.ObjectSpacePosition =                        input.positionOS;
        
            return output;
        }
        SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
        
        #ifdef HAVE_VFX_MODIFICATION
        #if VFX_USE_GRAPH_VALUES
            uint instanceActiveIndex = asuint(UNITY_ACCESS_INSTANCED_PROP(PerInstance, _InstanceActiveIndex));
            /* WARNING: $splice Could not find named fragment 'VFXLoadGraphValues' */
        #endif
            /* WARNING: $splice Could not find named fragment 'VFXSetFragInputs' */
        
        #endif
        
            
        
        
        
            output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);
        
        
            output.WorldSpacePosition = input.positionWS;
        
            #if UNITY_UV_STARTS_AT_TOP
            #else
            #endif
        
        
            output.uv0 = input.texCoord0;
            output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
        #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
        #else
        #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        #endif
        #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
        
                return output;
        }
        
        // --------------------------------------------------
        // Main
        
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteForwardPass.hlsl"
        
        // --------------------------------------------------
        // Visual Effect Vertex Invocations
        #ifdef HAVE_VFX_MODIFICATION
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/VisualEffectVertex.hlsl"
        #endif
        
        ENDHLSL
        }
    }
    CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
    FallBack "Hidden/Shader Graph/FallbackError"
}
Shader "GridShader" {
	Properties{
		_LineColor("Line Color", Color) = (1,1,1,1)
		_CellColor("Cell Color", Color) = (0,0,0,0)
		_SelectedColor("Selected Color", Color) = (1,0,0,1)
		[PerRendererData] _MainTex("Color (RGB) Alpha (A)", 2D) = "white" {}
		[IntRange] _GridSize("Grid Size", Range(1,100)) = 10
		_LineSize("Line Size", Range(0,1)) = 0.15
		[IntRange] _SelectCell("Select Cell Toggle ( 0 = False , 1 = True )", Range(0,1)) = 0.0
		[IntRange] _SelectedCellX("Selected Cell X", Range(0,100)) = 0.0
		[IntRange] _SelectedCellY("Selected Cell Y", Range(0,100)) = 0.0
	}
		SubShader{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }


			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard alpha


			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;	
			};

			half _Glossiness = 0.0;
			half _Metallic = 0.0;
			float4 _LineColor;
			float4 _CellColor;
			float4 _SelectedColor;

			float _GridSize;
			float _LineSize;

			float _SelectCell;
			float _SelectedCellX;
			float _SelectedCellY;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o) {

				float2 uv = IN.uv_MainTex;

				float gridWidth = 23;
				float gridHeight = 14;
				float gsize = floor(_GridSize);

				gridWidth += _LineSize;
				gridHeight += _LineSize;
				gsize += _LineSize;

				float2 id;

				id.x = floor(uv.x / (1.0 / gridWidth));
				id.y = floor(uv.y / (1.0 / gridHeight));

				float4 color = _CellColor;

				_SelectedCellX = floor(_SelectedCellX);
				_SelectedCellY = floor(_SelectedCellY);
				//This checks that the cell is currently selected if the Select Cell slider is set to 1 ( True )
				if (round(_SelectCell) == 1.0 && id.x == _SelectedCellX && id.y == _SelectedCellY)
				{
					color = _SelectedColor;
				}

				if (frac(uv.x * gridWidth) <= _LineSize || frac(uv.y * gridHeight) <= _LineSize)
				{
					color = _LineColor;
				}

				//Clips pixel if alpha is 0
				if (color.w == 0.0) {
					clip(-1.0);
				}

				o.Albedo = color;
				// Metallic and smoothness come from slider variables
				o.Metallic = 0.0;
				o.Smoothness = 0.0;
				o.Alpha = 0.0;
			}
			ENDCG
		}
}

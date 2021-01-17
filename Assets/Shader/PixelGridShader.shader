// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "PixelGridShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_LineColor("Line Color", Color) = (1,1,1,1)
		_CellColor("Cell Color", Color) = (0,0,0,0)
		_SelectedColor("Selected Color", Color) = (1,0,0,1)
		[IntRange] gridWidth("Grid Width", Range(1,100)) = 10
		[IntRange] gridHeight("Grid Height", Range(1,100)) = 10
		_LineSize("Line Size", Range(0,1)) = 0.15
		[IntRange] _SelectCell("Select Cell Toggle ( 0 = False , 1 = True )", Range(0,1)) = 0.0
		[IntRange] _SelectedCellX("Selected Cell X", Range(0,100)) = 0.0
		[IntRange] _SelectedCellY("Selected Cell Y", Range(0,100)) = 0.0
	}

		SubShader
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Tags
			{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}
			Pass
			{
				CGPROGRAM

				#include "UnityCG.cginc"
				//#pragma surface surf Standard alpha
				#pragma vertex vert
				#pragma fragment frag

				struct v2f
				{
					float4 pos : SV_POSITION;
					float2 uv_MainTex : TEXCOORD0;
				};
				sampler2D _MainTex;


				half _Glossiness = 0.0;
				half _Metallic = 0.0;
				float4 _LineColor;
				float4 _CellColor;
				float4 _SelectedColor;
				uniform float myValue;

				float gridWidth;
				float gridHeight;
				float _LineSize;

				float _SelectCell;
				float _SelectedCellX;
				float _SelectedCellY;
				float4 _MainTex_ST;

				v2f vert(appdata_base v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
					return o;
				}
				float4 frag(v2f IN) : COLOR
				{
					float2 uv = IN.uv_MainTex;
					gridWidth += _LineSize;
					gridHeight += _LineSize;
					float2 id;
					id.x = floor(uv.x / (1.0 / gridWidth));
					id.y = floor(uv.y / (1.0 / gridHeight));

					float4 color = _CellColor;
					color.x = myValue;
					color.y = 0;
					color.z = 0;
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

					if (color.w == 0.0) {
						clip(-1.0);
					}

					return color;
				}
				ENDCG
			}

		}
}
//void surf(Input IN, inout SurfaceOutputStandard o) {
//	float2 uv = IN.uv_MainTex;

//	gridWidth += _LineSize;
//	gridHeight += _LineSize;

//	float2 id;
//	id.x = floor(uv.x / (1.0 / gridWidth));
//	id.y = floor(uv.y / (1.0 / gridHeight));

//	float4 color = _CellColor;
//	color.x = myValue;
//	color.y = 0;
//	color.z = 0;

//	_SelectedCellX = floor(_SelectedCellX);
//	_SelectedCellY = floor(_SelectedCellY);
//	//This checks that the cell is currently selected if the Select Cell slider is set to 1 ( True )
//	if (round(_SelectCell) == 1.0 && id.x == _SelectedCellX && id.y == _SelectedCellY)
//	{
//		color = _SelectedColor;
//	}

//	if (frac(uv.x * gridWidth) <= _LineSize || frac(uv.y * gridHeight) <= _LineSize)
//	{
//		color = _LineColor;
//	}

//	if (color.w == 0.0) {
//		clip(-1.0);
//	}

//	o.Albedo = color;
//	o.Metallic = 0.0;
//	o.Smoothness = 0.0;
//	o.Alpha = color.w;
//}



Shader "CellShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_LineColor("Line Color", Color) = (1,1,1,1)
		_CellColor("Cell Color", Color) = (0,0,0,0)
		_WalkAbleColor("Walkable Color", Color) = (0,0,0,0)
		_HighlightedColor("Highlighted Color", Color) = (0,0,0,0)
		_LineSize("Line Size", Range(0,1)) = 0.15

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
												#pragma vertex vert
												#pragma geometry geom
												#pragma fragment frag
												#pragma enable_d3d11_debug_symbols
												struct g2f
												{
													float4 pos : SV_POSITION;
													float2 uv_MainTex : TEXCOORD0;
													float4 worldPos : POS;
													float2 index : INDEX;
													float isWalkable : WALKABLE;
													float isHovered : HOVERED;
												};
												struct v2g
												{
													float4 pos : SV_POSITION;
													float2 uv_MainTex : TEXCOORD0;
													float4 worldPos : POS;
												};
												sampler2D _MainTex;

												float _LineSize;
												float4 _LineColor;
												float4 _CellColor;
												float4 _WalkAbleColor;
												float4 _HighlightedColor;
												float4 _MainTex_ST;

												uniform float4 playerPosition;
												uniform float4 playerCellIndex;
												uniform float4 mousePos;
												uniform float4 topLeftCellPos;
												uniform float4 cellsWithGlyph[1000];
												uniform float nrOfCellsWithGlyph;
												uniform float playerTakingWalkInput;
												uniform float cellSize;
												uniform float playerWalkRange;

												v2g vert(appdata_base v)
												{
													v2g o;
													o.pos = UnityObjectToClipPos(v.vertex);
													o.uv_MainTex = TRANSFORM_TEX(v.texcoord, _MainTex);
													o.worldPos = mul(unity_ObjectToWorld, v.vertex);
													return o;
												}

												float2 GetFourthPoint(triangle v2g input[3])
												{
													float2 fourthPoint = float2(1,1);

													if (input[0].worldPos.x != input[1].worldPos.x
														&& input[0].worldPos.x != input[2].worldPos.x)
													{
														fourthPoint.x = input[0].worldPos.x;
													}
													else if (input[1].worldPos.x != input[0].worldPos.x
														&& input[1].worldPos.x != input[2].worldPos.x)
													{
														fourthPoint.x = input[1].worldPos.x;
													}
													else if (input[2].worldPos.x != input[0].worldPos.x
														&& input[2].worldPos.x != input[1].worldPos.x)
													{
														fourthPoint.x = input[2].worldPos.x;
													}

													if (input[0].worldPos.y != input[1].worldPos.y
														&& input[0].worldPos.y != input[2].worldPos.y)
													{
														fourthPoint.y = input[0].worldPos.y;
													}
													else if (input[1].worldPos.y != input[0].worldPos.y
														&& input[1].worldPos.y != input[2].worldPos.y)
													{
														fourthPoint.y = input[1].worldPos.y;
													}
													else if (input[2].worldPos.y != input[0].worldPos.y
														&& input[2].worldPos.y != input[1].worldPos.y)
													{
														fourthPoint.y = input[2].worldPos.y;
													}

													return fourthPoint;
												}

												float2 GetBottomLeftVert(triangle v2g input[3], float2 fourthPoint)
												{
													float2 middleOfCell = (input[0].worldPos.xy + input[1].worldPos.xy + input[2].worldPos.xy + fourthPoint) / 4;

													float2 points[4] = { input[0].worldPos.xy, input[1].worldPos.xy, input[2].worldPos.xy, fourthPoint };
													for (int i = 0; i < 4; i++)
													{
														if (points[i].x < middleOfCell.x && points[i].y < middleOfCell.y)
														{
															return points[i];
														}
													}
													return float2(0, 0);
												}
												float2 GetTopLeftVert(triangle v2g input[3], float2 fourthPoint)
												{
													float2 middleOfCell = (input[0].worldPos.xy + input[1].worldPos.xy + input[2].worldPos.xy + fourthPoint) / 4;

													float2 points[4] = { input[0].worldPos.xy, input[1].worldPos.xy, input[2].worldPos.xy, fourthPoint };
													for (int i = 0; i < 4; i++)
													{
														if (points[i].x < middleOfCell.x && points[i].y > middleOfCell.y)
														{
															return points[i];
														}
													}
													return float2(0, 0);
												}
												float2 GetBottomRightVert(triangle v2g input[3], float2 fourthPoint)
												{
													float2 middleOfCell = (input[0].worldPos.xy + input[1].worldPos.xy + input[2].worldPos.xy + fourthPoint) / 4;

													float2 points[4] = { input[0].worldPos.xy, input[1].worldPos.xy, input[2].worldPos.xy, fourthPoint };
													for (int i = 0; i < 4; i++)
													{
														if (points[i].x > middleOfCell.x && points[i].y < middleOfCell.y)
														{
															return points[i];
														}
													}
													return float2(0, 0);
												}
												float2 GetTopRightVert(triangle v2g input[3], float2 fourthPoint)
												{
													float2 middleOfCell = (input[0].worldPos.xy + input[1].worldPos.xy + input[2].worldPos.xy + fourthPoint) / 4;

													float2 points[4] = { input[0].worldPos.xy, input[1].worldPos.xy, input[2].worldPos.xy, fourthPoint };
													for (int i = 0; i < 4; i++)
													{
														if (points[i].x > middleOfCell.x && points[i].y > middleOfCell.y)
														{
															return points[i];
														}
													}
													return float2(0, 0);
												}
												bool RectangleIntersection(float2 l1, float2 r1, float2 l2, float2 r2)
												{
													bool isOneRectangleToTheLeft = (l1.x >= r2.x || l2.x >= r1.x);
													bool isOneRectangleAbove = (l1.y <= r2.y || l2.y <= r1.y);

													return !isOneRectangleToTheLeft && !isOneRectangleAbove;
												}
												bool IsPointInsideRectangle(float2 bottomLeft, float2 topRight,float2 pointToCheck)
												{
													return pointToCheck.x > bottomLeft.x
														&& pointToCheck.x < topRight.x
														&& pointToCheck.y > bottomLeft.y
														&& pointToCheck.y < topRight.y;
												}
												bool IsInWalkableRange(float2 pos1Index, float2 pos2Index)
												{
													bool isXInRange = abs(pos1Index.x - pos2Index.x) <= playerWalkRange;
													bool isYInRange = abs(pos1Index.y - pos2Index.y) <= playerWalkRange;

													return isXInRange && isYInRange;
												}
												[maxvertexcount(3)]
												void geom(triangle v2g input[3], inout TriangleStream<g2f> tristream)
												{
													g2f o;

													float2 fourthPoint = GetFourthPoint(input);
													float2 bottomLeft = GetBottomLeftVert(input, fourthPoint);
													float2 topLeft = GetTopLeftVert(input, fourthPoint);
													float2 topRight = GetTopRightVert(input, fourthPoint);
													float2 bottomRight = GetBottomRightVert(input, fourthPoint);
													float2 centerOfQuad = (bottomLeft + topLeft + topRight + bottomRight) / 4;

													float2 index;
													index.x = (centerOfQuad.x - topLeftCellPos.x) / cellSize;
													index.y = (topLeftCellPos.y - centerOfQuad.y) / cellSize;

													bool isWalkable = false;
													bool isHovered = false;
													if (playerTakingWalkInput == 1)
													{
														isWalkable = IsInWalkableRange(index, playerCellIndex.xy);
														isHovered = IsPointInsideRectangle(bottomLeft, topRight, mousePos.xy);
													}

													for (int i = 0; i < 3; i++)
													{
														o.pos = input[i].pos;
														o.uv_MainTex = input[i].uv_MainTex;
														o.worldPos = input[i].worldPos;
														o.isWalkable = isWalkable;
														o.isHovered = isHovered;
														o.index = index;
														tristream.Append(o);
													}
												}

												float4 frag(g2f IN) : COLOR
												{
													float2 uv = IN.uv_MainTex;

													float4 color = _CellColor;
													bool hasTexture = false;
													for (int i = 0; i < nrOfCellsWithGlyph; i++)
													{
														if (IN.index.x == cellsWithGlyph[i].x && IN.index.y == cellsWithGlyph[i].y)
														{
															hasTexture = true;
														}
													}
													if (hasTexture)
													{
														color = tex2D(_MainTex, uv);
													}

													float size = _LineSize;
													if (IN.isWalkable)
													{
														size = size *2;
													}
													if (uv.x > 1 - size || uv.x < size || uv.y > 1 - size || uv.y < size)
													{
														if (IN.isWalkable)
														{
															color = _WalkAbleColor;
														}
														else
														{
															color = _LineColor;
														}
													}
													else if (IN.isWalkable)
													{
														if (IN.isHovered)
														{
															color = _HighlightedColor;
														}
														else
														{
															color *= (_WalkAbleColor);
															color.w = 1; 
														}
													}

													if (color.w == 0.0)
													{
														clip(-1.0);
													}
													return color;
												}

							ENDCG
							}
		}
}

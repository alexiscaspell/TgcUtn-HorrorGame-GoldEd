// ---------------------------------------------------------
// Ejemplo shader Minimo:
// ---------------------------------------------------------

/**************************************************************************************/
/* Variables comunes */
/**************************************************************************************/

//Matrices de transformacion
float4x4 matWorld; //Matriz de transformacion World
float4x4 matWorldView; //Matriz World * View
float4x4 matWorldViewProj; //Matriz World * View * Projection
float4x4 matInverseTransposeWorld; //Matriz Transpose(Invert(World))

//Textura para DiffuseMap
texture texDiffuseMap;
sampler2D diffuseMap = sampler_state
{
	Texture = (texDiffuseMap);
	ADDRESSU = WRAP;
	ADDRESSV = WRAP;
	MINFILTER = LINEAR;
	MAGFILTER = LINEAR;
	MIPFILTER = LINEAR;
};

float time = 0;


/**************************************************************************************/
/* RenderScene */
/**************************************************************************************/

//Input del Vertex Shader
struct VS_INPUT 
{
   float4 Position : POSITION0;
   float4 Color : COLOR0;
   float2 Texcoord : TEXCOORD0;
};

//Output del Vertex Shader
struct VS_OUTPUT 
{
   float4 Position :        POSITION0;
   float2 Texcoord :        TEXCOORD0;
   float4 Color :			COLOR0;
};



//Vertex Shader
VS_OUTPUT vs_main( VS_INPUT Input )
{
   VS_OUTPUT Output;
   //Proyectar posicion
   Output.Position = mul( Input.Position, matWorldViewProj);
   
   //Propago las coordenadas de textura
   Output.Texcoord = Input.Texcoord;

   //Propago el color x vertice
   Output.Color = Input.Color;

   return( Output );
   
}


// Ejemplo de un vertex shader que anima la posicion de los vertices 
// ------------------------------------------------------------------
VS_OUTPUT vs_main2( VS_INPUT Input )
{
   VS_OUTPUT Output;

   // Animar posicion
   /*Input.Position.x += sin(time)*30*sign(Input.Position.x);
   Input.Position.y += cos(time)*30*sign(Input.Position.y-20);
   Input.Position.z += sin(time)*30*sign(Input.Position.z);
   */

   // Animar posicion
   float Y = Input.Position.y;
   float Z = Input.Position.z;
   Input.Position.y = Y * cos(time) - Z * sin(time);
   Input.Position.z = Z * cos(time) + Y * sin(time);

   
   //Proyectar posicion
   Output.Position = mul( Input.Position, matWorldViewProj);
   
   //Propago las coordenadas de textura
   Output.Texcoord = Input.Texcoord;

	// Animar color
   Input.Color.r = abs(sin(time));
   Input.Color.g = abs(cos(time));
   
   //Propago el color x vertice
   Output.Color = Input.Color;

   return( Output );
   
}

//Pixel Shader
float4 ps_main( float2 Texcoord: TEXCOORD0, float4 Color:COLOR0) : COLOR0
{      
	// Obtener el texel de textura
	// diffuseMap es el sampler, Texcoord son las coordenadas interpoladas
	float4 fvBaseColor = tex2D( diffuseMap, Texcoord );
	// combino color y textura
	// en este ejemplo combino un 80% el color de la textura y un 20%el del vertice
	return 0.8*fvBaseColor + 0.2*Color;
}

float4 ps_nada( float2 Texcoord: TEXCOORD0, float4 Color:COLOR0) : COLOR0
{
	return Color;
}

VS_OUTPUT vs_hechoMierda( VS_INPUT Input)
{
	VS_OUTPUT Output;
	
	float X = Input.Position.x;
	float Y = Input.Position.y;
	
	Input.Position.x = X * cos(time) *time;
	Input.Position.y = Y * sin(time) *time;
	
	//Output.Position = mul( Input.Position, matWorldViewProj);
	Output.Position = Input.Position;
	
	Output.Texcoord = Input.Texcoord;
	
	Output.Color = Input.Color;
	
	return(Output);
}

float4 ps_gris( float2 Texcoord: TEXCOORD0, float4 Color:COLOR0) : COLOR0
{
	//Gris = R * 0.222 + G * 0.707 + B* 0.071 (Formula para escala de grises)
	/* Color.x = Color.x * 0.222 + cos(time);
	Color.y = Color.y * 0.707 + cos(time);
	Color.z = Color.z * 0.071 + cos(time); */

	float4 retColor = tex2D(diffuseMap,Texcoord);
	
	retColor.x = retColor.x *0.222 + cos(time);
	retColor.y = retColor.y *0.707 + cos(time);
	retColor.z = retColor.z *0.071 + cos(time);
	
	retColor = float4(0,0,0,0);
	
	return retColor; 
}

// ------------------------------------------------------------------
technique RenderScene
{
   pass Pass_0
   {
	  VertexShader = compile vs_2_0 vs_main2();
	  PixelShader = compile ps_2_0 ps_main();
   }

}

// -------------------------------------------------------------------
technique HechoMierdaTechnique
{
	pass Pass_0
	{
		VertexShader = compile vs_2_0 vs_hechoMierda();
		PixelShader = compile ps_2_0 ps_gris();
	}
}
float2 DrawSize;
sampler sampler0 : register(s0);
sampler sampler1 : register(s1); 

float4 Read(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float2 unitx = float2(1/DrawSize.x, 0);
	float2 unity = float2(0, 1/DrawSize.y);

	float4 c = tex2D(sampler0, coords);

	if(c.a < 1)
	{
		if(((coords.x * DrawSize.x) % 2 > 0.5) == ((coords.y * DrawSize.y) % 2 > 0.5)) 
		   c = float4(1, 1, 1, 1); else c = float4(0, 0, 0, 1);
	}
	else
	{
		float4 l = tex2D(sampler0, coords - unitx);
		float4 r = tex2D(sampler0, coords + unitx);
		float x = (l.x - r.x)*DrawSize.x;

		float4 u = tex2D(sampler0, coords - unity);
		float4 d = tex2D(sampler0, coords + unity);
		float y = (u.x - d.x)*DrawSize.y;

		//c = float4(c.x*4, c.y / 5, -c.x*4, 1);
		c = tex2D(sampler1, coords + float2(x,y)*0.0003)*((x+y)*0.03+0.7) + min(0.2, pow(2, -(x*y))*0.005);
	}
	return c;
}

int writemode;
//Height = 0, Velocity = 1, Tone = 2, Wall = 3, Clear = 4;

float4 Write(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float2 unitx = float2(1/DrawSize.x, 0);
	float2 unity = float2(0, 1/DrawSize.y);

	float4 c = tex2D(sampler0, coords);

	if(c.x == 0 && c.y == 0 && c.z == 0 && c.w == 0) return tex2D(sampler1, coords);
	
	if(writemode == 0)
		return float4((c.x+c.y+c.z)/3, 0, 0, 1);
	else if(writemode == 1)
		return float4(0, (c.x+c.y+c.z)/3, 0, 1);
	else if(writemode == 2)
		return float4(0, 0, 0, (c.x+c.y+c.z)/3 - 0.00001);
	else if(writemode == 3)
		{if(c.x == 0 && c.y == 0 && c.z == 0) return float4(0, 0, 0, 0); else return float4(0, 0, 1, 0);}
	else
		return float4(0, 0, 0, 1);
}

float4 WriteToField(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(sampler0, coords);
	if(c.x == 0 && c.y == 0 && c.z == 0 && c.w == 0)
	return (0,0,0,1);
	else
	return c;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        PixelShader = compile ps_4_0_level_9_3 Read();
    }

	pass Pass2
	{
		PixelShader = compile ps_4_0_level_9_3 Write();
	}

	pass Pass3
	{
		PixelShader = compile ps_4_0_level_9_3 WriteToField();
	}
}

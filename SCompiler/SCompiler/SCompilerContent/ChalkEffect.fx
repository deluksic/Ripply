sampler InputSampler : register(s0);
sampler TextureSampler : register(s1);

float4 write(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    // Look up the texture color.
    float4 inp = tex2D(InputSampler, texCoord);
	float4 tex = tex2D(TextureSampler, texCoord);
	float a = (inp.a - tex.r);

	if(a > 0)
	return float4(inp.r*a, inp.g*a, inp.b*a, inp.a*a);
	else 
	return float4(0, 0, 0, 0);
}

technique Technique1
{
    pass Write
    {
        PixelShader = compile ps_4_0_level_9_3 write();
    }
}

float dt;
float2 one;
float2 source = float2(-0.5, -0.5);
static float speed = 0.05f;
static float k = 40;

sampler s0;
float t;

float4 PixelShaderFunction(float4 color : COLOR0, float2 coords : TEXCOORD0) : COLOR0
{
	float4 c = tex2D(s0, coords);
	float q = length((coords-source)/one);
	if(q < 3 && c.a == 1) return float4(sin(t*20)*0.2, 0, 0, 1);

	//return the boundaries
	if(c.a  == 0) return float4(0, 0, 0, 0);
	else if(c.a < 1) return float4(sin(t*20)*c.a*0.2, 0, 0, c.a);

	float dx = one.x*one.x;

	float4 r = tex2D(s0, coords + float2(one.x, 0));
	float4 l = tex2D(s0, coords - float2(one.x, 0));
	float4 d = tex2D(s0, coords + float2(0, one.y));
	float4 u = tex2D(s0, coords - float2(0, one.y));
	float rldu = r.x + l.x + d.x + u.x;

	float f = speed * ((rldu - 4*c.x)/dx) - c.y*k*dt;
	c.y += f*dt;
	c.x += c.y*dt;
		
	return c;
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        PixelShader = compile ps_4_0_level_9_3 PixelShaderFunction();
    }
}

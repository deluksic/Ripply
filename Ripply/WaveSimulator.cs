using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace WaveSimulation
{
    public enum WriteMode
    { 
        Height,
        Velocity,
        Tone,
        Wall,
        Clear,
    }

    class WaveSimulator
    {
        private GraphicsDevice graphicsDevice;
        private SpriteBatch spriteBatch;

        RenderTarget2D[] RenderTargets;
        bool RenderTargetSwitch = false;
        public RenderTarget2D TargetField
        {
            get { return RenderTargetSwitch ? RenderTargets[1] : RenderTargets[0]; }
        }
        public RenderTarget2D TargetTemp
        {
            get { return RenderTargetSwitch ? RenderTargets[0] : RenderTargets[1]; }
        }

        public Effect simulationEffect;
        public Effect drawingEffect;

        public int Width, Height;
        public float Time;

        public WaveSimulator(GraphicsDevice gD, SpriteBatch sB, int W, int H, Effect sim, Effect draw)
        {
            graphicsDevice = gD;
            spriteBatch = sB;
            Width = W; Height = H;

            //set simulation effect
            simulationEffect = sim;
            simulationEffect.Parameters["one"].SetValue(new Vector2(1f / Width, 1f / Height));

            //set drawing effect
            drawingEffect = draw;
            drawingEffect.Parameters["DrawSize"].SetValue(new Vector2(Width, Height));

            //set rendertargets
            RenderTargets = new RenderTarget2D[]{
                new RenderTarget2D(graphicsDevice, Width, Height, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents),
                new RenderTarget2D(graphicsDevice, Width, Height, false, SurfaceFormat.Vector4, DepthFormat.None, 0, RenderTargetUsage.PreserveContents)};

            //clear the field
            graphicsDevice.SetRenderTarget(TargetField);
            graphicsDevice.Clear(Color.Black);
            graphicsDevice.SetRenderTarget(null);

            //set the sampler states
            graphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            graphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
        }

        public void BeginWrite()
        {
            graphicsDevice.SetRenderTarget(TargetField);
            graphicsDevice.Textures[1] = TargetTemp;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullNone);
        }

        public void Write(Texture2D tex, Rectangle rec, WriteMode wm)
        {
            drawingEffect.Parameters["writemode"].SetValue((int)wm);
            drawingEffect.CurrentTechnique.Passes[1].Apply();
            spriteBatch.Draw(tex, rec, Color.White);
        }

        public void Write(Texture2D tex, Rectangle rec, Color c, WriteMode wm)
        {
            drawingEffect.Parameters["writemode"].SetValue((int)wm);
            drawingEffect.CurrentTechnique.Passes[1].Apply();
            spriteBatch.Draw(tex, rec, c);
        }

        public void Write(Texture2D tex, Vector2 vec, WriteMode wm)
        {
            drawingEffect.Parameters["writemode"].SetValue((int)wm);
            drawingEffect.CurrentTechnique.Passes[1].Apply();
            spriteBatch.Draw(tex, vec, Color.White);
        }

        public void Write(Texture2D tex, Vector2 vec, Color c, WriteMode wm)
        {
            drawingEffect.Parameters["writemode"].SetValue((int)wm);
            drawingEffect.CurrentTechnique.Passes[1].Apply();
            spriteBatch.Draw(tex, vec, c);
        }

        public void EndWrite()
        {
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
        }

        public void Simulate(float timestep, float nstep)
        {
            for (int i = 0; i < nstep; i++)
                SimulateStep(timestep);
        }

        public void SimulateStep(float dt)
        {
            //switch field and temp
            RenderTargetSwitch = !RenderTargetSwitch;
            //set render target to the field
            graphicsDevice.SetRenderTarget(TargetField);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            simulationEffect.Parameters["dt"].SetValue(dt);
            simulationEffect.Parameters["t"].SetValue(Time);
            //draw to field, it will become temp next time around
            simulationEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(TargetTemp, Vector2.Zero, Color.White);
            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);

            Time += dt;
        }

        public void Draw(Rectangle rec)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            drawingEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(TargetField, rec, Color.White);
            spriteBatch.End();
        }

        public void ClearField()
        {
            graphicsDevice.SetRenderTarget(TargetField);
            graphicsDevice.Clear(Color.Black);
            graphicsDevice.SetRenderTarget(null);
        }

        public void SetSourcePosition(Vector2 vec)
        {
            simulationEffect.Parameters["source"].SetValue(vec * new Vector2(1f / Width, 1f / Height));
        }
    }
}

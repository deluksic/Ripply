using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using WaveSimulation;
using Microsoft.Xna.Framework.Input.Touch;

namespace Ripply
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        TouchCollection Touches;
        Random random = new Random();
        Texture2D Pixel;
        int scale = 3; //how many logical pixels per real pixel
        public WriteMode Mode = WriteMode.Height;

        WaveSimulator simulator;

        Texture2D Background;
        Texture2D Source;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 1000;
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            simulator = new WaveSimulator(GraphicsDevice, spriteBatch, GraphicsDevice.Viewport.Width / scale, GraphicsDevice.Viewport.Height / scale, Content.Load<Effect>("SimulationEffect"), Content.Load<Effect>("DrawingEffect"));

            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData<Color>(new Color[] { Color.White });

            Background = Content.Load<Texture2D>("Pebbles");
            Source = Content.Load<Texture2D>("Source");

            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Touches = TouchPanel.GetState();

            simulator.BeginWrite();

            simulator.SetSourcePosition(-Vector2.One*2);
            if (Touches.Count > 0)
                HandleInput(Mode);

            //add rain
            //simulator.Write(Source, new Rectangle(random.Next(0, simulator.Width), random.Next(0, simulator.Height), 10, 10), new Color(1, 0, 0, 1), WriteMode.Height);

            simulator.EndWrite();

            simulator.Simulate(1f / 120, 4);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Textures[1] = Background;
            simulator.Draw(new Rectangle(0, 0, simulator.Width * scale, simulator.Height * scale));

            //uncomment to show debug
            //spriteBatch.Begin();
            //spriteBatch.Draw(simulator.TargetField, Vector2.UnitY * simulator.Height, Color.White);
            //spriteBatch.Draw(simulator.TargetTemp, Vector2.UnitY * simulator.Height * 2, Color.White);
            //spriteBatch.End();

            base.Draw(gameTime);
        }

        private void HandleInput(WriteMode mode)
        {
            var pos = Touches[0].Position / scale;

            //point
            simulator.SetSourcePosition(pos);
            //simulator.Write(Source, new Rectangle((int)pos.X-5, (int)pos.Y-5, 10, 10), mode);
        }
    }
}

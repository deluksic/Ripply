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
using MouseControl;
using KeyboardControl;
using WaveSimulation;

namespace SCompiler
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D Pixel;
        MouseObject mouse;
        KeyboardObject keyboard;

        WaveSimulator simulator;

        Texture2D Circle;
        Texture2D Picture;

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

            simulator = new WaveSimulator(GraphicsDevice, spriteBatch, 500, 500, Content.Load<Effect>("simulation"), Content.Load<Effect>("drawing"));

            Pixel = new Texture2D(GraphicsDevice, 1, 1);
            Pixel.SetData<Color>(new Color[] { Color.White });

            Circle = Content.Load<Texture2D>("circle");
            Picture = Content.Load<Texture2D>("pic");

            keyboard = new KeyboardObject();
            mouse = new MouseObject(Content, "mouse");

            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            mouse.Update();
            keyboard.Update();

            HandleInput(keyboard.Key(Keys.LeftShift) ? WriteMode.Tone : WriteMode.Wall);

            if (!keyboard.Key(Keys.P))
                simulator.Simulate(1f / 30, keyboard.Key(Keys.O) ? 1 : 60);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            simulator.Draw(new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));

            mouse.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        private void HandleInput(WriteMode mode)
        {
            simulator.BeginWrite();

            var pos = mouse.Position * simulator.Width / graphics.PreferredBackBufferWidth;

            //point
            if (mouse.LeftClick)
                simulator.SetSourcePosition(pos);
            else
                simulator.SetSourcePosition(Vector2.One*-10);
            if (mouse.RightClick)
                simulator.Write(Pixel, new Rectangle((int)pos.X - 20, (int)pos.Y - 20, 40, 40), WriteMode.Clear);

            //clear
            if (keyboard.KeyPressed(Keys.R))
                simulator.ClearField();

            //half-circle
            if (keyboard.Key(Keys.C))
                simulator.Write(Circle, pos - Vector2.One * Circle.Width / 2, mode);

            //square
            if (keyboard.Key(Keys.S))
                simulator.Write(Pixel, new Rectangle((int)pos.X, (int)pos.Y, 5, 5), mode);

            //horizontal
            if (keyboard.Key(Keys.H))
                simulator.Write(Pixel, new Rectangle((int)pos.X, (int)pos.Y, 200, 2), mode);

            //vertical
            if (keyboard.Key(Keys.V))
                simulator.Write(Pixel, new Rectangle((int)pos.X, (int)pos.Y, 2, 200), mode);

            simulator.EndWrite();
        }
    }
}

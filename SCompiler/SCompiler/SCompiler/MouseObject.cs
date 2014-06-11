using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MouseControl
{
    public class MouseObject
    {
        /// <summary>
        /// Holds the previous and current state of the mouse.
        /// </summary>
        MouseState previousMouseState, currentMouseState;

        /// <summary>
        /// Holds the current position of the mouse pointer.
        /// </summary>
        Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// Holds the texture of the mouse object.
        /// </summary>
        public Texture2D texture;

        /// <summary>
        /// Position of our object + dimensions
        /// </summary>
        Rectangle rectangle;
        public Rectangle Rectangle
        {
            get { return rectangle; }
        }

        /// <summary>
        /// Is the left button pressed down?
        /// </summary>
        public bool LeftClick
        {
            get { return currentMouseState.LeftButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Is the left button newly clicked?  The left button is clicked now, but not previously
        /// </summary>
        public bool NewLeftClick
        {
            get { return LeftClick && previousMouseState.LeftButton == ButtonState.Released; }
        }

        /// <summary>
        /// The left button was pressed, but we released it
        /// </summary>
        public bool LeftRelease
        {
            get { return !LeftClick && previousMouseState.LeftButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Is the right button pressed down?
        /// </summary>
        public bool RightClick
        {
            get { return currentMouseState.RightButton == ButtonState.Pressed; }
        }

        /// <summary>
        /// Is the right button newly clicked?  The right button is clicked now, but not previously
        /// </summary>
        public bool NewRightClick
        {
            get { return RightClick && previousMouseState.RightButton == ButtonState.Released; }
        }

        /// <summary>
        /// The right button was pressed, but we released it
        /// </summary>
        public bool RightRelease
        {
            get { return !RightClick && previousMouseState.RightButton == ButtonState.Pressed; }
        }

        public bool WheelUp
        {
            get { return (currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue>0); }
        }

        public bool WheelDown
        {
            get { return (currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue < 0); }
        }

        /// <summary>
        /// Creates a new Mouse object.  Empty constructor for later logic.
        /// </summary>
        public MouseObject() { }

        /// <summary>
        /// Creates a new Mouse object, uses passed texture for the mouse.
        /// </summary>
        /// <param name="texture">Texture you wish to make the mouse use.</param>
        public MouseObject(Texture2D texture)
        {
            this.texture = texture;
        }

        /// <summary>
        /// Creates a new Mouse object, uses passed content manager and assetname to load the texture.
        /// </summary>
        /// <param name="content">The active content manager found in the Game1.cs or ScreenManager</param>
        /// <param name="assetName">The asset name of the texture you wish to use.</param>
        public MouseObject(ContentManager content, string assetName)
        {
            texture = content.Load<Texture2D>(assetName);
        }

        /// <summary>
        /// Update the mouse state and position.
        /// </summary>
        public void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            position = new Vector2(currentMouseState.X, currentMouseState.Y);
        }

        /// <summary>
        /// Draw the mouse on the screen, if a texture exists
        /// </summary>
        /// <param name="spriteBatch">The desired spriteBatch you wish to use.  This SpriteBatch object CANNOT be between begin and end calls.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (texture != null)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(texture, position, Color.White);
                spriteBatch.End();
            }
        }
    }
}

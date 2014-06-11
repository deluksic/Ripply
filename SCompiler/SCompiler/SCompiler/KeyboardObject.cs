using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace KeyboardControl
{
    public class KeyboardObject
    {
        /// <summary>
        /// Holds the previous and current state of the keyboard.
        /// </summary>
        KeyboardState currentKeyState, pastKeyState;

        public KeyboardObject(){ }

        public void Update()
        {
            pastKeyState = currentKeyState;
            currentKeyState = Keyboard.GetState();
        }

        /// <summary>
        /// Is the key down?
        /// </summary>
        /// /// <param name="k">Key to check.</param>
        public bool Key(Keys k)
        {
            return currentKeyState.IsKeyDown(k);
        }

        /// <summary>
        /// Is the key pressed just now?
        /// </summary>
        /// /// <param name="k">Key to check.</param>
        public bool KeyPressed(Keys k)
        {
            return currentKeyState.IsKeyDown(k) && pastKeyState.IsKeyUp(k);
        }

        /// <summary>
        /// Is the key released?
        /// </summary>
        /// /// <param name="k">Key to check.</param>
        public bool KeyReleased(Keys k)
        {
            return currentKeyState.IsKeyUp(k) && pastKeyState.IsKeyDown(k);
        }
    }
}

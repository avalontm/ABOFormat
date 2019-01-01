using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABOFormat
{
    public static class ABOEngine
    {
        public static Game Game { private set; get; }
        public static SpriteBatch SpriteBatch { private set; get; }

        public static void Init(Game game)
        {
            Game = game;
            Game.TargetElapsedTime = TimeSpan.FromTicks(166666);
            Game.IsFixedTimeStep = true;

            SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
        }
    }
}

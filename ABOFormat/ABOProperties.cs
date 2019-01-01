using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ABOFormat
{
    public class SpriteProperties
    {
        public Texture2D Texture { set; get; }
        public Vector2 Position { set; get; }
        public int Wait { set; get; }
        public bool TurnX { set; get; }
        public bool TurnY { set; get; }
    }

    public class CharProperties
    {
        public string Name { set; get; }
        public bool Loop { set; get; }
        public List<SpriteProperties> Textures { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlockStack
{
    public class Block
    {
        public Vector2 position;
        public Color tint;
        public int width;
        public int hight;
        public Texture2D texture;
        public Rectangle bounding;

        public Block()
        {

        }

        public Block(Vector2 position, Color tint, int width, int hight, Texture2D texture)
        {
            this.position = position;
            this.tint = tint;
            this.width = width;
            this.hight = hight;
            this.texture = texture;
        }



    }
}

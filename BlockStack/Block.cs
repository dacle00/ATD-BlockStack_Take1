///////////////////////////////////////////////////////////////////////////////////////////
// Block class represents each square tile within a tetromino.
//
// AUTHORS: F1tZ, DoubleMintBen, CptSpaceToaster, Dacle
// COMPANY: AfterThough Digital
// STARTED: October, 2014
//

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
        public Texture2D texture;
        public Rectangle bounding;

        public Block()
        {

        }

        public Block(Vector2 position, Color tint, int width, Texture2D texture)
        {
            this.position = position;
            this.tint = tint;
            this.width= width;
            this.texture = texture;
        }



    }
}

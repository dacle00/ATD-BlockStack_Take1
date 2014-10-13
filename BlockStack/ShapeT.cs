///////////////////////////////////////////////////////////////////////////////////////////
// ShapeT class is a data type for a tetromino piece
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

namespace BlockStack
{
    class ShapeT : Tetromino
    {
        public List<Block> up = new List<Block>();
        public List<Block> left = new List<Block>();
        public List<Block> down = new List<Block>();
        public List<Block> right = new List<Block>();

        public override void constructPiece(Microsoft.Xna.Framework.Color tint, int width, Microsoft.Xna.Framework.Graphics.Texture2D texture)
        {
            up.Add(new Block(new Vector2(1f, 1f), tint, width, texture));
            up.Add(new Block(new Vector2(2f, 1f), tint, width, texture));
            up.Add(new Block(new Vector2(3f, 1f), tint, width, texture));
            up.Add(new Block(new Vector2(2f, 2f), tint, width, texture));

            left.Add(new Block(new Vector2(2f, 0f), tint, width, texture));
            left.Add(new Block(new Vector2(3f, 1f), tint, width, texture));
            left.Add(new Block(new Vector2(2f, 1f), tint, width, texture));
            left.Add(new Block(new Vector2(2f, 2f), tint, width, texture));

            down.Add(new Block(new Vector2(1f, 1f), tint, width, texture));
            down.Add(new Block(new Vector2(2f, 1f), tint, width, texture));
            down.Add(new Block(new Vector2(3f, 1f), tint, width, texture));
            down.Add(new Block(new Vector2(2f, 0f), tint, width, texture));

            right.Add(new Block(new Vector2(2f, 0f), tint, width, texture));
            right.Add(new Block(new Vector2(1f, 1f), tint, width, texture));
            right.Add(new Block(new Vector2(2f, 1f), tint, width, texture));
            right.Add(new Block(new Vector2(2f, 2f), tint, width, texture));

            blockList = up;
        }

        public override void Rotate(bool reverseRot = false)
        {
            base.toggleRotationValues(reverseRot);

            if (rotation == blockRotation.up)
            {
                blockList = up;
            }
            else if (rotation == blockRotation.left)
            {
                blockList = left;
            }
            else if (rotation == blockRotation.down)
            {
                blockList = down;
            }
            else
            {
                blockList = right;
            }
        }
    }
}

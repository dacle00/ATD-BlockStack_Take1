using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BlockStack
{
    class ShapeO : Tetromino
    {
        public List<Block> upDownLeftRight = new List<Block>();

        public override void constructPiece(Microsoft.Xna.Framework.Color tint, int width, int hight, Microsoft.Xna.Framework.Graphics.Texture2D texture)
        {
            upDownLeftRight.Add(new Block(new Vector2(1f, 1f), tint, width, hight, texture));
            upDownLeftRight.Add(new Block(new Vector2(2f, 1f), tint, width, hight, texture));
            upDownLeftRight.Add(new Block(new Vector2(2f, 2f), tint, width, hight, texture));
            upDownLeftRight.Add(new Block(new Vector2(1f, 2f), tint, width, hight, texture));

            blockList = upDownLeftRight;
        }

        public override void Rotate(bool reverseRot = false)
        {
            base.toggleRotationValues(reverseRot);
            blockList = upDownLeftRight;
        }
    }
}

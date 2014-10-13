using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BlockStack
{
    class ShapeS : Tetromino
    {
        public List<Block> upDown = new List<Block>();
        public List<Block> leftRight = new List<Block>();

        public override void constructPiece(Microsoft.Xna.Framework.Color tint, int width, int hight, Microsoft.Xna.Framework.Graphics.Texture2D texture)
        {
            upDown.Add(new Block(new Vector2(1f, 2f), tint, width, hight, texture));
            upDown.Add(new Block(new Vector2(2f, 2f), tint, width, hight, texture));
            upDown.Add(new Block(new Vector2(2f, 1f), tint, width, hight, texture));
            upDown.Add(new Block(new Vector2(3f, 1f), tint, width, hight, texture));

            leftRight.Add(new Block(new Vector2(2f, 0f), tint, width, hight, texture));
            leftRight.Add(new Block(new Vector2(2f, 1f), tint, width, hight, texture));
            leftRight.Add(new Block(new Vector2(3f, 1f), tint, width, hight, texture));
            leftRight.Add(new Block(new Vector2(3f, 2f), tint, width, hight, texture));

            blockList = upDown;
        }

        public override void Rotate(bool reverseRot = false)
        {
            base.toggleRotationValues(reverseRot);

            if (rotation == blockRotation.up || rotation == blockRotation.down)
            {
                blockList = upDown;
            }
            else
            {
                blockList = leftRight;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BlockStack
{
    class ShapeJ : Tetromino
    {
        public List<Block> up = new List<Block>();
        public List<Block> left = new List<Block>();
        public List<Block> down= new List<Block>();
        public List<Block> right = new List<Block>();

        public override void constructPiece(Microsoft.Xna.Framework.Color tint, int width, int hight, Microsoft.Xna.Framework.Graphics.Texture2D texture)
        {
            // Up (Default Rotation)
            up.Add(new Block(new Vector2(1f, 1f), tint, width, hight, texture));
            up.Add(new Block(new Vector2(2f, 1f), tint, width, hight, texture));
            up.Add(new Block(new Vector2(3f, 1f), tint, width, hight, texture));
            up.Add(new Block(new Vector2(3f, 2f), tint, width, hight, texture));

            // Left
            left.Add(new Block(new Vector2(2f, 0f), tint, width, hight, texture));
            left.Add(new Block(new Vector2(3f, 0f), tint, width, hight, texture));
            left.Add(new Block(new Vector2(2f, 1f), tint, width, hight, texture));
            left.Add(new Block(new Vector2(2f, 2f), tint, width, hight, texture));

            // Down
            down.Add(new Block(new Vector2(1f, 0f), tint, width, hight, texture));
            down.Add(new Block(new Vector2(1f, 1f), tint, width, hight, texture));
            down.Add(new Block(new Vector2(2f, 1f), tint, width, hight, texture));
            down.Add(new Block(new Vector2(3f, 1f), tint, width, hight, texture));

            // Right
            right.Add(new Block(new Vector2(2f, 0f), tint, width, hight, texture));
            right.Add(new Block(new Vector2(2f, 1f), tint, width, hight, texture));
            right.Add(new Block(new Vector2(1f, 2f), tint, width, hight, texture));
            right.Add(new Block(new Vector2(2f, 2f), tint, width, hight, texture));

            blockList = up;
        }


        public override void Rotate(bool reverseRot = false)
        {
            base.toggleRotationValues(reverseRot);

            if (rotation == blockRotation.up )
                blockList = up;
            else if (rotation == blockRotation.left)
                blockList = left;
            else if (rotation == blockRotation.down)
                blockList = down;
            else
                blockList = right;

        }

    }
}

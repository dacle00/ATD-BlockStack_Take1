///////////////////////////////////////////////////////////////////////////////////////////
// ShapeI class is a data type for a tetromino piece
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
    
  

    class ShapeI : Tetromino
    {


        public List<Block> updown = new List<Block>();
        public List<Block> leftright = new List<Block>();


        public override void constructPiece(Microsoft.Xna.Framework.Color tint, int width, Microsoft.Xna.Framework.Graphics.Texture2D texture)
        {
            // Up / Down ( Default Rotation)
            updown.Add(new Block(new Vector2(0f, 1f), tint, width, texture));
            updown.Add(new Block(new Vector2(1f, 1f), tint, width, texture));
            updown.Add(new Block(new Vector2(2f, 1f), tint, width, texture));
            updown.Add(new Block(new Vector2(3f, 1f), tint, width, texture));

            // Left / Right
            leftright.Add(new Block(new Vector2(2f, 0f), tint, width, texture));
            leftright.Add(new Block(new Vector2(2f, 1f), tint, width, texture));
            leftright.Add(new Block(new Vector2(2f, 2f), tint, width, texture));
            leftright.Add(new Block(new Vector2(2f, 3f), tint, width, texture));

            blockList = updown;
        }


        public override void Rotate(bool reverseRot = false)
        {
            base.toggleRotationValues(reverseRot);

            if (rotation == blockRotation.up || rotation == blockRotation.down)
                blockList = updown;
            else
                blockList = leftright;

        }

    }
}

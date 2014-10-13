///////////////////////////////////////////////////////////////////////////////////////////
// Tetromino abstract class allows childs classes to model specific Tetromino shapes 
// (I, J, S, etc)
//
// AUTHORS: F1tZ, DoubleMintBen, CptSpaceToaster, Dacle
// COMPANY: AfterThough Digital
// STARTED: October, 2014
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BlockStack
{
    public enum blockRotation
    {
        up, left, down, right
    }

    public abstract class Tetromino
    {
        public Rectangle bounding;
        public Vector2 position;
        public blockRotation rotation;
        public int speed;
        public List<Block> blockList;
        List<blockRotation> waffleWithSprinkles = new List<blockRotation>( new blockRotation[] {blockRotation.up, blockRotation.left, blockRotation.down, blockRotation.right});
    
   

        public Tetromino()
        {
            blockList = new List<Block>();
        }


        public void Draw(GameTime gt, SpriteBatch sb)
        {
            foreach (Block currentBlock in blockList)
            {
                sb.Draw(currentBlock.texture, position + (currentBlock.position *32), currentBlock.tint);
            }
        }

        public virtual void constructPiece(Color tint, int width, Texture2D texture)
        {
            blockList.Add(new Block(new Vector2(1f, 1f), tint, width, texture));
            blockList.Add(new Block(new Vector2(1f, 2f), tint, width, texture));
            blockList.Add(new Block(new Vector2(2f, 1f), tint, width, texture));
            blockList.Add(new Block(new Vector2(2f, 2f), tint, width, texture));
        }

        public abstract void Rotate(bool counterClockwise = true);
 

        public void toggleRotationValues(bool counterClockwise = true)
        {
            // cycle the rotation
            int currIndex = waffleWithSprinkles.IndexOf(rotation);
            currIndex = (currIndex + (counterClockwise ? 1 : -1)) % 3;
            if (currIndex < 0)
            {
                currIndex = 3;
            }
            rotation = waffleWithSprinkles[currIndex];
        }
           
    }
}

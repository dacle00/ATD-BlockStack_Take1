﻿///////////////////////////////////////////////////////////////////////////////////////////
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
        public Vector2 screenPosition;
        public Vector2 boardPosition;
        public blockRotation rotation;
        public int speed;
        public List<Block> blockList;
        List<blockRotation> ListOfRotations = new List<blockRotation>( new blockRotation[] {blockRotation.up, blockRotation.left, blockRotation.down, blockRotation.right});
    
   

        public Tetromino()
        {
            blockList = new List<Block>(4);  //TETROminos are limited to four pieces
        }


        public void Draw(GameTime gt, SpriteBatch sb)
        {
            foreach (Block currentBlock in blockList)
            {
                sb.Draw(currentBlock.texture, screenPosition + (currentBlock.position *32), currentBlock.tint);
            }
        }

        public virtual void constructPiece(Color tint, int width, Texture2D texture)
        {
            // X shape 
            blockList.Add(new Block(new Vector2(0f, 0f), tint, width, texture));
            blockList.Add(new Block(new Vector2(3f, 0f), tint, width, texture));
            blockList.Add(new Block(new Vector2(0f, 3f), tint, width, texture));
            blockList.Add(new Block(new Vector2(3f, 3f), tint, width, texture));
        }

        public abstract void Rotate(bool counterClockwise = true);
 

        public void toggleRotationValues(bool counterClockwise = true)
        {
            // cycle the rotation
            int currIndex = ListOfRotations.IndexOf(rotation);
            currIndex = (currIndex + (counterClockwise ? 1 : -1)) % 3;
            if (currIndex < 0)
            {
                currIndex = 3;
            }
            rotation = ListOfRotations[currIndex];
        }

        /// <summary>
        /// using the bounds in the parameter, check each block of the piece to see if that block can move Left
        /// </summary>
        /// <returns></returns>
        public bool CanMoveLeft(int minXPos)
        {
            bool retVal = true;

            foreach (Block b in blockList)
            {
                //determine sub-block's position relative to tetrominos position
                int xPos = Convert.ToInt16(boardPosition.X + b.position.X);

                if ((xPos - 1) < minXPos)
                    retVal = false;
            }

            return retVal;
        }


        /// <summary>
        /// using the bounds in the parameter, check each block of the piece to see if that block can move Left
        /// </summary>
        /// <returns></returns>
        public bool CanMoveRight(int maxXPos)
        {
            bool retVal = true;

            foreach (Block b in blockList)
            {
                //determine sub-block's position relative to tetrominos position
                int xPos = Convert.ToInt16(boardPosition.X + b.position.X);

                if ((xPos + 1) > maxXPos)
                    retVal = false;
            }

            return retVal;
        }


        /// <summary>
        /// using the bounds in the parameter, check each block of the piece to see if that block can move Left
        /// </summary>
        /// <returns></returns>
        public bool CanMoveDown(int maxYPos)
        {
            bool retVal = true;

            foreach (Block b in blockList)
            {
                //determine sub-block's position relative to tetrominos position
                int yPos = Convert.ToInt16(boardPosition.Y + b.position.Y);

                if ((yPos + 1) > maxYPos)
                    retVal = false;
            }

            return retVal;
        }


    }
}

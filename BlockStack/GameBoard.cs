///////////////////////////////////////////////////////////////////////////////////////////
// GameBoard class holds all the information for played tetromino pieces which have already
// landed.
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
    class GameBoard
    {
        
        public GameBoardRow[] pile;
        int width;
        int height;
        int screenOffsetX;
        int screenOffsetY;


        public GameBoard(int w, int h, int offsetX, int offsetY)
        {
            width = w;
            height = h;
            screenOffsetX = offsetX;
            screenOffsetY = offsetY;
            pile = new GameBoardRow[height];
            for (int i = 0; i < height; i++)
                pile[i] = new GameBoardRow(width);
        }


        public void AddLandedPiece(Tetromino t)
        {
            // add each block of the Tetromino into the pile's cells
            foreach (Block b in t.blockList)
            {
                //todo: add in TETRIMINO position and SUB-BLOCK position

                int xPos = Convert.ToInt16(t.boardPosition.X + b.position.X);
                int yPos = Convert.ToInt16(t.boardPosition.Y + b.position.Y);
                pile[yPos].data[xPos].block = b;
                pile[yPos].data[xPos].isFilled = true;

/*              pile[Convert.ToInt16(b.position.Y)].data[Convert.ToInt16(b.position.X)].block = b;
                pile[Convert.ToInt16(b.position.Y)].data[Convert.ToInt16(b.position.X)].isFilled = true;
 * */
            }
        }



        public void Draw(GameTime gt, SpriteBatch sb)
        {
            Block b = new Block();
            //draw a grid for every cell on the board.
            for (int y=0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (pile[y].data[x].isFilled)
                    {
                        //TODO:  make this work.  make this make sense.  make this not hideous.
                        //draw the block
                        b = new Block();
                        b.position = new Vector2(screenOffsetX+(x * 32), screenOffsetY+(y * 32));
                        b.tint = pile[y].data[x].block.tint;
                        b.width = 32;
                        b.texture = pile[y].data[x].block.texture;

                        sb.Draw(b.texture, new Rectangle(Convert.ToInt16(b.position.X), Convert.ToInt16(b.position.Y), b.width, b.width), b.tint);
                    }
                }

        }


    }
}

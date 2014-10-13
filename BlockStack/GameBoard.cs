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

        
        BlockRow[] pile;
        int width;
        int height;


        public GameBoard(int w, int h)
        {
            width = w;
            height = h;
            pile = new BlockRow[height];
            for (int i = 0; i < height; i++)
                pile[i] = new BlockRow(width);
        }


        public void Draw(GameTime gt, SpriteBatch sb)
        {
            Block b = new Block();
            //draw a grid for every cell on the board.
            for (int y=0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    if (pile[y].row[x].isFilled)
                    {
                        //TODO:  make this work.  make this make sense.  make this not hideous.
                        //draw the block
                        b = new Block(new Vector2(x * 32, y * 32), pile[y].row[x].block.tint, 32, pile[y].row[x].block.texture);
                        sb.Draw(b.texture, new Rectangle(Convert.ToInt16(b.position.X), Convert.ToInt16(b.position.Y), b.width, b.width), b.tint);
                    }
                }

        }


    }
}

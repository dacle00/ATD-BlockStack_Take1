///////////////////////////////////////////////////////////////////////////////////////////
// GameBoardCell class represents a single block of the gameboard and is used to store 
// information as to whether a piece has landed within the cell, or not.
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
	class GameBoardCell
	{

        public bool isFilled;  // is a block resting in this cell
        public Block block;    // if block is resting in this cell, store details (texture, tint)

        public GameBoardCell()
        {
            isFilled = false;
            block = null;
        }


        public void Draw(GameTime gt, SpriteBatch sb)
        {

#if DEBUG
            //draw a grid for every cell on the board.
                
#endif
        }


	}
}

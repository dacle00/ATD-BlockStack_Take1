///////////////////////////////////////////////////////////////////////////////////////////
// BlockRow class stores all the tetromino parts (Blocks) which have been played and have
// fallen onto the gameboard.
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
    class BlockRow
    {

        public GameBoardCell[] row;

        public BlockRow(int width)
        {
            row = new GameBoardCell[width];

            for (int i = 0; i<width; i++)
                row[i] = new GameBoardCell();

        }

    }
}

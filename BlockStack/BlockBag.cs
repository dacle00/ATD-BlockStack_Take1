///////////////////////////////////////////////////////////////////////////////////////////
// BlockBag class acts as a tetromino generating factory, handling the "random" aspect of
// presenting the game with the next piece for play.
//
// AUTHORS: F1tZ, DoubleMintBen, CptSpaceToaster, Dacle
// COMPANY: AfterThough Digital
// STARTED: October, 2014
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BlockStack
{
    // designed to hold 7 pieces (one of each tetrimino shape)
    class BlockBag
    {
        private List<Tetromino> suitCaseOne;
        private List<Tetromino> suitCaseTwo = new List<Tetromino>();

        bool whichCase = true;
        Random rand = new Random();

        public BlockBag( List<Tetromino> pAvailablePieces)
        {
            suitCaseOne = pAvailablePieces;
        }

        public Tetromino GetNextPiece()
        {
            Tetromino nextPiece;
            if (whichCase)
            {
                int index = rand.Next(0, suitCaseOne.Count());
                nextPiece = suitCaseOne[index];
                suitCaseTwo.Add(suitCaseOne[index]);
                suitCaseOne.RemoveAt(index);

            }
            else
            {
                int index = rand.Next(0, suitCaseTwo.Count());
                nextPiece = suitCaseTwo[index];
                suitCaseOne.Add(suitCaseTwo[index]);
                suitCaseTwo.RemoveAt(index);
            }
            return nextPiece;
        }
    }
}

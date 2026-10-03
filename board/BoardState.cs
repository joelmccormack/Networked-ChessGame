using System.Data.SqlTypes;
using System.Xml;

namespace ChessGame
{
    public class BoardState
    {
        public  Dictionary<Position, Piece> allPieces; 
        public BoardState()
        {
            allPieces = new Dictionary<Position, Piece>();
            addPawnsAtRow(6, true); 
            addPawnsAtRow(1, false);
            addRooks();
            addBishops();
            addQueens();
            addKings();
            addKnights();     
            
        }

        public Piece? getPieceAt(Position pos) // method to get the piece at a specified position
        {
            return allPieces.GetValueOrDefault(pos, null);
        }

        private void addKings() //method to add a black and white King in its starting position
        {
            Position pos = new Position(4, 0);
            allPieces.Add(pos, new King(pos, false));
            pos = new Position(4, 7);
            allPieces.Add(pos, new King(pos, true));
        }

        private void addQueens() //method to add a black and white queen in its starting position
        {
            Position pos = new Position(3, 0);
            allPieces.Add(pos, new Queen(pos, false));
            pos = new Position(3, 7);
            allPieces.Add(pos, new Queen(pos, true));
        }

        private void addKnights() //method to add the black and white knights in their starting positions
        {
            Position pos = new Position(1, 0);
            allPieces.Add(pos, new Knight(pos, false));
            pos = new Position(6, 0);
            allPieces.Add(pos, new Knight(pos, false));
            pos = new Position(1, 7);
            allPieces.Add(pos, new Knight(pos, true));
            pos = new Position(6, 7);
            allPieces.Add(pos, new Knight(pos, true));
        }

        private void addBishops() //method to add the black and white bishops in their starting positions
        {
            Position pos = new Position(2, 0);
            allPieces.Add(pos, new Bishop(pos, false));
            pos = new Position(5, 0);
            allPieces.Add(pos, new Bishop(pos, false));
            pos = new Position(2, 7);
            allPieces.Add(pos, new Bishop(pos, true));
            pos = new Position(5, 7);
            allPieces.Add(pos, new Bishop(pos, true));
        }

        private void addRooks() //method to add the black and white rooks in their starting positions
        {
            Position pos = new Position(0, 0);
            allPieces.Add(pos, new Rook(pos, false));
            pos = new Position(7, 0);
            allPieces.Add(pos, new Rook(pos, false));
            pos = new Position(0, 7);
            allPieces.Add(pos, new Rook(pos, true));
            pos = new Position(7, 7);
            allPieces.Add(pos, new Rook(pos, true));
        }

        private void addPawnsAtRow(int row, bool isWhite) //method to add a row of pawns for either white or black  
        {
            Position pos = new Position(0, row);
            for (int i = 0; i < 8; i++)
            {
                allPieces.Add(pos, new Pawn(pos, isWhite));

                pos = pos.addX(1);
            }

        }

        public void removeAt(Position currentPosition) 
        {
           allPieces.Remove(currentPosition);            
        }

        public void addAt(Position position, Piece piece)
        {
            allPieces.Add(position, piece);
        }

        public List<Position> getPosOfPiece(int value)
        {
            List<Position> piecesPos = new List<Position>();
            foreach(KeyValuePair<Position, Piece> pair in allPieces)
            {
                Position position = pair.Key;
                Piece piece = pair.Value;
                if (piece != null)
                {
                    if (piece.value == value)
                    {
                        piecesPos.Add(position);
                    }
                }
            }
            if(piecesPos.Count() == 0)
            {
                piecesPos.Add(new Position(-1, -1));
            }
            return piecesPos;
     
        }

        public bool inCheckMate(bool isWhite)
        {
            List<Position> allLegalMoves = getValidMoves(isWhite);
            if (allLegalMoves.Count == 0)
            {
                return true;
            }
            return false;
        }

        private List<Position> getValidMoves(bool isWhite)
        {
            List<Position> allValidNextPositions = new List<Position>();
            foreach(KeyValuePair<Position, Piece> entry in allPieces.ToList())
            {
                Position posOfPiece = entry.Key;
                Piece piece = entry.Value;
                if (piece != null)
                {
                    
                    if (piece.isWhite() != isWhite)
                    {
                        List<Position> nextpositions = piece.nextPositions(this);
                        allValidNextPositions.AddRange(TestMove(nextpositions, posOfPiece, piece, isWhite));
                        if (allValidNextPositions.Count > 0)
                        {
                            return allValidNextPositions;
                        }
                    }


                }
            }         
            return allValidNextPositions;
        }
        private List<Position> TestMove(List<Position> nextPositions,Position posOfPiece, Piece piece, bool isWhite)
        {
            List<Position> AllLegalMoves = new List<Position>();
            foreach(Position pos in nextPositions)
            {
                Piece pieceAtNext = getPieceAt(pos);
                removeAt(posOfPiece);
                removeAt(pos);
                addAt(pos, piece);
                               
                if (!piece.inCheck(this, !isWhite))
                {
                    AllLegalMoves.Add(pos);
                    removeAt(pos);
                    addAt(posOfPiece, piece);
                    addAt(pos, pieceAtNext);
                    return AllLegalMoves; //only need to find one legal move so return as soon as found one
                }
                removeAt(pos);
                addAt(posOfPiece, piece);
                addAt(pos, pieceAtNext);
            }
            return AllLegalMoves;
            
        }


        public void finishGame(bool surrender, bool isWhite, IEnumerable<KeyValuePair<Position, Button>> allButtons)
        {
            foreach (KeyValuePair<Position, Button> entry in allButtons)
            {
                Button button = entry.Value;
                button.Enabled = false;
            }
            if (isWhite)
            {
                if (surrender)
                {
                    MessageBox.Show("Black Surrendered");
                }
                else
                {
                    MessageBox.Show("Black in checkmate, White wins ");
                }
            }
            else
            {

                if (surrender)
                {
                    MessageBox.Show("White Surrendered");
                }
                else
                {
                    MessageBox.Show("White in Checkmate, black wins");
                }
            }
        }

        internal void StaleMate()
        {
            MessageBox.Show("Draw, stale mate");
        }
    }

}
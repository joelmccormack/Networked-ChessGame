using System.Configuration;
using System.Windows.Forms;

namespace ChessGame
{
    internal class King : Piece
    {
        static readonly string BLACK_KING = "\u265A";
        static readonly string WHITE_KING = "\u2654";

        public King(Position pos, bool isWhite) : base(pos, isWhite)
        {
            if (isWhite) { value = 11; }
            else { value = 12; }
            moved = false;
        }
        public override string unicode()
        {
            if (isWhite())
            {
                return WHITE_KING;
            }
            else
            { return BLACK_KING; }
        }
        public override List<Position> nextPositions(BoardState boardstate) // every position in square around the King
        {
            List<Position> result = kingMoves(boardstate, getPosition(), isWhite());
            result.AddRange(castleMoves(boardstate));
            return result;

        }
        private List<Position> castleMoves(BoardState boardState)
        {
            List<Position> move = new List<Position>();
            Position kingPosition = getPosition();

            int rookValue = 3;
            bool isPiece = false;

            if (!isWhite())
            {
                rookValue = 4;
            }
            
            if (!moved)
            {
                List<Position> rooks = boardState.getPosOfPiece(rookValue);
                foreach(Position rookpos in rooks)
                {
                   Piece rook = boardState.getPieceAt(rookpos);
                    if (rook != null && !rook.moved)
                    {
                        
                        if(kingPosition.addX(3).getX() == rookpos.getX() && kingPosition.getY() == rookpos.getY())
                        { 
                            for(int i = 1;i<3; i++)
                            {
                                isPiece = false;
                                Position checkPos = kingPosition.addX(i);
                                Piece piece = boardState.getPieceAt(checkPos);
                                if(piece != null)
                                {
                                    isPiece = true;
                                    break;                                  
                                }
                            }
                            if (!isPiece && validateCastle(boardState, kingPosition, 1))
                            {
                                move.Add(kingPosition.addX(2));
                            }
                        }
                        if (kingPosition.addX(-4).getX() == rookpos.getX() && kingPosition.getY() == rookpos.getY())
                        {
                            for (int i = -1; i > -4; i--)
                            {
                                isPiece = false;
                                Position checkPos = kingPosition.addX(i);
                                Piece piece = boardState.getPieceAt(checkPos);
                                if (piece != null)
                                {
                                    isPiece = true;
                                    break;
                                }
                            }
                            if (!isPiece && validateCastle(boardState, kingPosition, 2))
                            {
                                move.Add(kingPosition.addX(-2));
                            }
                        }

                    }
                }
            }
            if(move == null)
            {
                move.Add(new Position(-1, -1));
            }
            return move;
        }

        private bool validateCastle(BoardState boardState, Position current, int direction)
        {
            if(inCheck(boardState, this.isWhite()))
            {
                return false;
            }
            int num = 1;
            if (direction == 2)
            {
                num = -1;
            }
            boardState.removeAt(current);
            boardState.removeAt(current.addX(num));
            boardState.addAt(current.addX(num), this);
            if (inCheck(boardState, this.isWhite()))
            {
                boardState.removeAt(current.addX(num));
                boardState.addAt(current, this);
                return false;
            }
            boardState.removeAt(current.addX(num));
            boardState.addAt(current, this);
            return true;
        } 
    }
}
namespace ChessGame
{
    public class Pawn : Piece
    {
        static readonly string BLACK_PAWN = "\u265F";
        static readonly string WHITE_PAWN = "\u2659";
        public Pawn(Position pos, bool isWhite) : base(pos,  isWhite)
        {
            if (isWhite) { value = 1; }
            else { value = 2; }
            moved = false;
        }

        public override string unicode()
        {
            if (isWhite())
            {
                return WHITE_PAWN;
            }
            else
            {
                return BLACK_PAWN;
            }
        }
        public override List<Position> nextPositions(BoardState boardstate)
        {
            List<Position> result = new List<Position>();
            Position myPosition = getPosition();
            if (isWhite())
            {
                Position next = myPosition.addY(-1);
                Piece piece = boardstate.getPieceAt(next);
                if (piece == null)
                {
                    result.Add(next);
                    if (!moved)
                    {
                        next = myPosition.addY(-2);
                        piece = boardstate.getPieceAt(next);
                        if (piece == null)                    
                        {
                            result.Add(next);

                        }
                    }
                }
                if (myPosition.getX() > 0)
                {
                    next = myPosition.addY(-1).addX(-1);
                    piece = boardstate.getPieceAt(next);
                    if (piece != null)
                    {
                        if (piece.isWhite() != this.isWhite())
                        {
                            result.Add(next);
                        }
                    }
                }
                if (myPosition.getX() < 7)
                {
                    next = myPosition.addY(-1).addX(1);
                    piece = boardstate.getPieceAt(next);
                    if (piece != null)
                    {
                        if(piece.isWhite() != this.isWhite())
                        {
                            result.Add(next);
                        }            
                    }      
                }              

            }
            else
            {
                Position next = myPosition.addY(1);
                Piece piece = boardstate.getPieceAt(next);
                if (piece == null)
                {
                    result.Add(next);
                    if (!moved)
                    {
                        next = myPosition.addY(2);
                        piece = boardstate.getPieceAt(next);
                        if (piece == null)
                        {
                            result.Add(next);
                        }
                    }
                }
                if (myPosition.getX() > 0)
                {
                    next = myPosition.addY(1).addX(-1);
                    piece = boardstate.getPieceAt(next);
                    if (piece != null)
                    {
                        if (piece.isWhite() != this.isWhite())
                        {     
                            result.Add(next);
                        }
                    }
                }
                if (myPosition.getX() < 7)
                {
                    next = myPosition.addY(1).addX(1);
                    piece = boardstate.getPieceAt(next);
                    if (piece != null)
                    {
                        if (piece.isWhite() != this.isWhite())
                        {
                            result.Add(next);
                        }
                    }
                }
            }
            return result;

        }    
    }
}
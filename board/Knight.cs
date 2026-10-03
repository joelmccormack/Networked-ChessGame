namespace ChessGame
{
    internal class Knight : Piece
    {
        //unicode for black and white knight
        static readonly string BLACK_KNIGHT = "\u265E"; 
        static readonly string WHITE_KNIGHT = "\u2658";
        public Knight(Position pos, bool isWhite) : base(pos, isWhite)
        {
            if (isWhite) { value = 5; }
            else { value = 6; }
            moved = false;
        }
        public override string unicode()
        {
            if (isWhite())
            {
                return WHITE_KNIGHT;
            }
            else
            { return BLACK_KNIGHT; }
        }
        public override List<Position> nextPositions(BoardState boardstate)
        {

            List<Position> result = knightMoves(boardstate, getPosition(), isWhite());            
            return result;

        }

        
    }
}
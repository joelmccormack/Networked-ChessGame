namespace ChessGame
{
    internal class Rook : Piece
    {
        static readonly string BLACK_ROOK = "\u265C"; //unicode for a black rook
        static readonly string WHITE_ROOK = "\u2656"; //unicode for a white rook
        public Rook(Position pos, bool isWhite) : base(pos, isWhite)
        {
            if (isWhite) { value = 3; }
            else { value = 4; }
            moved = false;
        }
        public override string unicode() //return the unicode required
        {
            if (isWhite())
            {
                return WHITE_ROOK;
            }
            else
            {
                return BLACK_ROOK;
            }
        }

        public override List<Position> nextPositions(BoardState boardState)
        {
            List<Position> result = new List<Position>();
            List<Position> horizontal = horizontalmoves(boardState, getPosition(), isWhite());
            List<Position> vertical = verticalmoves(boardState, getPosition(), isWhite());
            result.AddRange(horizontal);
            result.AddRange(vertical);
            return result;
            
        }
    }
}
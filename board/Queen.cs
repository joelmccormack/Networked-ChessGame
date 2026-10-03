namespace ChessGame
{
    internal class Queen : Piece
    {
        static readonly string BLACK_QUEEN = "\u265B";
        static readonly string WHITE_QUEEN = "\u2655";
        public Queen(Position pos, bool isWhite) : base(pos, isWhite)
        {
            if (isWhite) { value = 9; }
            else { value = 10; }
            moved = false;
        }
        public override string unicode()
        {
            if (isWhite())
            {
                return WHITE_QUEEN;
            }
            else
            { return BLACK_QUEEN; }
        }
        public override List<Position> nextPositions(BoardState boardstate)
        {
            List<Position> result = new List<Position>();
            List<Position> diagonal = diagonalmove(boardstate, getPosition(), isWhite());
            List<Position> horizontal = horizontalmoves(boardstate, getPosition(), isWhite());
            List<Position> vertical = verticalmoves(boardstate, getPosition(), isWhite());
            result.AddRange(diagonal);
            result.AddRange(horizontal);
            result.AddRange(vertical);
            return result;
        }
    }
}
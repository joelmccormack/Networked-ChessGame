namespace ChessGame
{
    public struct Position
    {
        private int x;
        private int y;

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        internal int getY()
        {
            return y;
        }
        internal int getX()
        {
            return x;
        }

        internal Position addX(int v)
        {
            return new Position(this.x + v, y);
        }

        internal Position addY(int v)
        {

            return new Position(this.x , y + v);
        }

        public string getString()
        {
            return "X =" + x + " y=" + y ;
        }
    }
}
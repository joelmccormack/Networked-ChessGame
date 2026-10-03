namespace ChessGame
{
    public abstract class Piece
    {
        private readonly bool white;
        public Position currentposition;
        public int value;
        public bool moved;
        public Piece(Position pos, bool isWhite)
        {
            this.currentposition = pos;
            this.white = isWhite;
        }
        public Position getPosition()
        {
            return this.currentposition;
        }
        public bool isWhite()
        {
            return this.white;
        }

        protected List<Position> horizontalmoves(BoardState boardstate, Position myPosition, bool colour) // check all spaces to left and right of piece
        {
            List<Position> result = new List<Position>();

            for (int i = myPosition.getX() + 1; i < 8; i++)
            {
                Position nextPos = new Position(i, myPosition.getY()); // check all spaces going left
                Piece piece = boardstate.getPieceAt(nextPos);
                if (piece == null)
                {
                    result.Add(nextPos);
                }
                else if (piece.isWhite() != colour)
                {
                    result.Add(nextPos);
                    break;
                }
                else
                {
                    break;
                }
            }
            for (int i = myPosition.getX() - 1; i >= 0; i--) // check all spaces going right
            {
                Position nextPos = new Position(i, myPosition.getY());
                Piece piece = boardstate.getPieceAt(nextPos);
                if (piece == null)
                {
                    result.Add(nextPos);
                }
                else if (piece.isWhite() != colour)
                {
                    result.Add(nextPos);
                    break;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        protected List<Position> verticalmoves(BoardState boardstate, Position myPosition, bool colour) // check all spaces above and below piece
        {
            List<Position> result = new List<Position>();
            
            for (int i = myPosition.getY() + 1; i < 8; i++) // check all spaces going down
            {
                Position nextPos = new Position(myPosition.getX(), i);
                Piece piece = boardstate.getPieceAt(nextPos);
                if (piece == null)
                {
                    result.Add(nextPos);
                }
                else if (piece.isWhite() != colour)
                {
                    result.Add(nextPos);
                    break;
                }
                else
                {
                    break;
                }
            }
            for (int i = myPosition.getY() - 1; i >= 0; i--) // check all spaces going up
            {
                Position nextPos = new Position(myPosition.getX(), i);
                Piece piece = boardstate.getPieceAt(nextPos);
                if (piece == null)
                {
                    result.Add(nextPos);
                }
                else if (piece.isWhite() != colour)
                {
                    result.Add(nextPos);
                    break;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        protected List<Position> diagonalmove(BoardState boardstate, Position myPosition, bool colour) //check all spaces in all diagonal directions
        {
            List<Position> result = new List<Position>();
           
            for (int i = myPosition.getY() + 1, j = myPosition.getX() + 1; i < 8 && j < 8; i++, j++)
            {
                Position nextPos = new Position(j, i);
                Piece piece = boardstate.getPieceAt(nextPos);
                if (piece == null)
                {
                    result.Add(nextPos);

                }
                else if (piece.isWhite() != colour)
                {
                    result.Add(nextPos);
                    break;
                }
                else
                {
                    break;
                }

            }
            for (int i = myPosition.getY() + 1, j = myPosition.getX() - 1; i < 8 && j >= 0; i++, j--)
            {
                Position nextPos = new Position(j, i);
                Piece piece = boardstate.getPieceAt(nextPos);
                if (piece == null)
                {
                    result.Add(nextPos);

                }
                else if (piece.isWhite() != colour)
                {
                    result.Add(nextPos);
                    break;
                }
                else
                {
                    break;
                }
            }
            for (int i = myPosition.getY() - 1, j = myPosition.getX() + 1; i >= 0 && j < 8; i--, j++)
            {
                Position nextPos = new Position(j, i);
                Piece piece = boardstate.getPieceAt(nextPos);
                if (piece == null)
                {
                    result.Add(nextPos);

                }
                else if (piece.isWhite() != colour)
                {
                    result.Add(nextPos);
                    break;
                }
                else
                {
                    break;
                }
            }
            for (int i = myPosition.getY() - 1, j = myPosition.getX() - 1; i >= 0 && j >= 0; i--, j--)
            {
                Position nextPos = new Position(j, i);
                Piece piece = boardstate.getPieceAt(nextPos);
                if (piece == null)
                {
                    result.Add(nextPos);

                }
                else if (piece.isWhite() != colour)
                {
                    result.Add(nextPos);
                    break;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        protected List<Position> knightMoves(BoardState boardstate, Position myPosition, bool colour)//find all of the knights next positions 
        {
            List<Position> result = new List<Position>();
            Position nextPos = myPosition.addY(-2).addX(-1);
            CheckSpace(nextPos, boardstate, result, colour);
            nextPos = myPosition.addY(-1).addX(-2);
            CheckSpace(nextPos, boardstate, result, colour);
            nextPos = myPosition.addY(1).addX(-2);
            CheckSpace(nextPos, boardstate, result, colour);
            nextPos = myPosition.addY(2).addX(-1);
            CheckSpace(nextPos, boardstate, result, colour);
            nextPos = myPosition.addY(2).addX(1);
            CheckSpace(nextPos, boardstate, result, colour);
            nextPos = myPosition.addY(1).addX(2);
            CheckSpace(nextPos, boardstate, result, colour);
            nextPos = myPosition.addY(-1).addX(2);
            CheckSpace(nextPos, boardstate, result, colour);
            nextPos = myPosition.addY(-2).addX(1);
            CheckSpace(nextPos, boardstate, result, colour);
            return result;
        }
        private void CheckSpace(Position position, BoardState boardstate, List<Position> result, bool isWhite) // to make sure next position is on the board and the colour piece at that position to determine if it can be taken or not
        {
            if (position.getX() >= 0 && position.getX() <= 7 && position.getY() >= 0 && position.getY() <= 7) //check the nextPos is inside the board parameters
            {
                Piece piece = boardstate.getPieceAt(position);
                if (piece == null)
                {
                    result.Add(position);

                }
                else if (piece.isWhite() != isWhite)
                {
                    result.Add(position);
                }
            }
        }

        protected List<Position> kingMoves(BoardState boardstate, Position myPosition, bool isWhite)//find all of the Kings next positions
        {
            List<Position> result = new List<Position>();
            Position nextPos = myPosition.addY(-1);
            checkForPiece(nextPos, boardstate, result, isWhite);
            nextPos = myPosition.addY(-1).addX(-1);
            checkForPiece(nextPos, boardstate, result, isWhite);
            nextPos = myPosition.addX(-1);
            checkForPiece(nextPos, boardstate, result, isWhite);
            nextPos = myPosition.addY(1).addX(-1);
            checkForPiece(nextPos, boardstate, result, isWhite);
            nextPos = myPosition.addY(1);
            checkForPiece(nextPos, boardstate, result, isWhite);
            nextPos = myPosition.addY(1).addX(1);
            checkForPiece(nextPos, boardstate, result, isWhite);
            nextPos = myPosition.addX(1);
            checkForPiece(nextPos, boardstate, result, isWhite);
            nextPos = myPosition.addY(-1).addX(1);
            checkForPiece(nextPos, boardstate, result, isWhite);
            return result;
        }
        private void checkForPiece(Position position, BoardState boardstate, List<Position> result, bool isWhite) //check that nextPos is allowed
        {

            if (position.getX() >= 0 && position.getX() <= 7 && position.getY() >= 0 && position.getY() <= 7) //check the nextPos is inside the board parameters
            {
                Piece piece = boardstate.getPieceAt(position);
                if (piece == null || piece.isWhite() != isWhite)
                {
                    result.Add(position);
                }

            }

        }
       
        public abstract string unicode(); //the method each inherited class uses to return the unicode text of that piece

        public abstract List<Position> nextPositions(BoardState boardstate); //create a list of every legal next position for each piece

        public bool movePiece(Position position, BoardState boardstate)//to try and move a piece to square clicked
        {
           
            //get position of the square clicked last time. So where the piece is, before being moved
            Position currentPosition = getPosition(); 
            
            //get the piece at currentPosition
            Piece pieceAtCurrent = boardstate.getPieceAt(currentposition);

            //get colour of piece, true = white, false = black
            bool colour = pieceAtCurrent.isWhite();
            

            //try to get piece at position other piece is trying to move to
            Piece pieceAtNext = boardstate.getPieceAt(position);

            if (valid(position, boardstate))
            {
                bool castledRight = false;
                bool castledLeft = false;
                //move the piece
                if((pieceAtCurrent.value == 11 || pieceAtCurrent.value == 12) && (currentposition.addX(2).getX() == position.getX()))
                {
                    castlemove(1, currentposition, position, boardstate);
                    castledRight = true;
                }
                if ((pieceAtCurrent.value == 11 || pieceAtCurrent.value == 12) && (currentposition.addX(-2).getX() == position.getX()))
                {
                    castlemove(2, currentposition, position, boardstate);
                    castledLeft = true;
                }
                else
                {
                    boardstate.removeAt(currentPosition);
                    boardstate.removeAt(position);
                    boardstate.addAt(position, this);
                }

                // if not in check then piece has succesfully been moved and return true
                if (!inCheck(boardstate, colour))
                {
                    pieceAtCurrent.moved = true;
                    pieceAtCurrent.currentposition = position;
                    if (pieceAtCurrent.value == 1 || pieceAtCurrent.value == 2)
                    {
                        promotePawn(boardstate, pieceAtCurrent);
                    }

                    return true;
                    
                }
                //if in check then reverse move
                if (castledRight)
                {
                    reverseCastle(1, currentPosition, position, boardstate);
                }
                else if (castledLeft)
                {
                    reverseCastle(2,currentPosition, position, boardstate);
                }
                else
                {
                    boardstate.removeAt(position);
                    boardstate.addAt(currentPosition, this);
                    boardstate.addAt(position, pieceAtNext);
                }
            }
            //if not valid or in check then return false as piece is unable to move to the chosen square
            return false;
            
        }

        private void promotePawn(BoardState boardstate, Piece pawn)
        {
            List<Position> pawns = boardstate.getPosOfPiece(pawn.value);
            foreach (Position pos in pawns)
            {
                if (pos.getY() == 0)
                {
                    boardstate.removeAt(pos);
                    boardstate.addAt(pos, new Queen(pos, true));
                }
                
                if (pos.getY() == 7)
                {
                    boardstate.removeAt(pos);
                    boardstate.addAt(pos, new Queen(pos, false));
                }
            }
        }

        private bool valid(Position position, BoardState boardstate) //to ensure move being made is valid
        {
            List<Position> valid = nextPositions(boardstate);
            if(valid.Contains(position))
            {
                return true;
            }

            else
            { 
                return false;
            }

        } 
        //need to clean up code/ dhorten code.
        public bool inCheck(BoardState boardstate, bool isWhite)//checks if the king of the piece trying to move is theatened by another piece
        {
            //get colour of piece just moved, check the king of that colour to see if any piece is threateninng ir and if so return true
            int i = 0;
            int j = 1;
            if (!isWhite)
            {
                i = 1;
                j = 0;
            }//Position of white King
            List<Position> KingList = boardstate.getPosOfPiece(11 + i);
            Position kingPos = KingList.FirstOrDefault();

            //List of possible positions king could be threatended from
            List<Position> kingMove = kingMoves(boardstate, kingPos, isWhite);
            List<Position> knightMove = knightMoves(boardstate, kingPos, isWhite);
            List<Position> diagonal = diagonalmove(boardstate, kingPos, isWhite);
            List<Position> horizontalAndVertical = horizontalmoves(boardstate, kingPos, isWhite);
            horizontalAndVertical.AddRange(verticalmoves(boardstate, kingPos, isWhite));

            //List of positions of where the black pieces are
            List<Position> oppnentKingList = boardstate.getPosOfPiece(11 + j);
            List<Position> opponentKnightList = boardstate.getPosOfPiece(5 + j);
            List<Position> opponentBishList = boardstate.getPosOfPiece(7 + j);
            List<Position> opponentRookList = boardstate.getPosOfPiece(3 + j);

            //positions from the list of positions of the black pieces
            Position opponentKingPos = oppnentKingList.FirstOrDefault();
            Position opponentRookPos1 = opponentRookList.FirstOrDefault();
            Position opponentRookPos2 = opponentRookList.Skip(1).FirstOrDefault();
            if (opponentRookList.Count() < 2)
            {
                opponentRookPos2 = new Position(-1, -1);
            }
            Position opponentBishPos1 = opponentBishList.FirstOrDefault();
            Position opponentBishPos2 = opponentBishList.Skip(1).FirstOrDefault();
            if (opponentBishList.Count() < 2)
            {
                opponentBishPos2 = new Position(-1, -1);
            }
            Position opponentKnightPos1 = opponentKnightList.FirstOrDefault();
            Position opponentKnightPos2 = opponentKnightList.Skip(1).FirstOrDefault();
            if (opponentKnightList.Count() < 2)
            {
                opponentKnightPos2 = new Position(-1, -1);
            }

            //checking if the positions of the black pieces are in the list of the threatening positions for each piece              
            if (diagonal.Contains(opponentBishPos1) || diagonal.Contains(opponentBishPos2) || knightMove.Contains(opponentKnightPos1)
                || knightMove.Contains(opponentKnightPos2) || kingMove.Contains(opponentKingPos) || horizontalAndVertical.Contains(opponentRookPos1)
                || horizontalAndVertical.Contains(opponentRookPos2))
            {
                return true;
            }
            if (isWhite)
            {
                List<Position> blackPawnsList = boardstate.getPosOfPiece(2);
                List<Position> blackQueenList = boardstate.getPosOfPiece(10);

                // cheking for black pawns
                foreach (Position position in blackPawnsList) 
                {
                    Position potentialPawn = kingPos.addX(1).addY(-1);
                    Position potentialPawn2 = kingPos.addX(-1).addY(-1);
                    if (diagonal.Contains(position) && ((position.getX() == potentialPawn.getX() && position.getY() == potentialPawn.getY()) || position.getX() == potentialPawn2.getX() && position.getY() == potentialPawn2.getY()))
                    {
                        return true;
                    }               
                }
                foreach(Position pos in blackQueenList)
                {
                    if(diagonal.Contains(pos) || horizontalAndVertical.Contains(pos))
                    {
                        return true;
                    }
                }
            }
            else
            {
                // cheking for white pawns
                List<Position> whitePawnsList = boardstate.getPosOfPiece(1);
                List<Position> whiteQueenList = boardstate.getPosOfPiece(9);

                foreach (Position position in whitePawnsList)
                {
                    Position potentialPawn = kingPos.addX(1).addY(1);
                    Position potentialPawn2 = kingPos.addX(-1).addY(1);
                    if (diagonal.Contains(position) && ((position.getX() == potentialPawn.getX() && position.getY() == potentialPawn.getY()) || position.getX() == potentialPawn2.getX() && position.getY() == potentialPawn2.getY()))
                    {
                        return true;
                    }
                }
                foreach(Position pos in whiteQueenList)
                {
                    if(diagonal.Contains(pos) || horizontalAndVertical.Contains(pos))
                    {
                        return true;
                    }
                }
            }

            return false;
        
        }

        private void castlemove(int direction, Position kingPos, Position kingNextPos, BoardState boardstate )
        {
            if (direction == 1) 
            {
                Position rookPos = kingPos.addX(3);
                Piece rook = boardstate.getPieceAt(rookPos);
                boardstate.removeAt(kingPos);
                boardstate.removeAt(rookPos);
                boardstate.removeAt(kingNextPos);
                boardstate.addAt(kingNextPos.addX(-1), rook);               
                boardstate.addAt(kingNextPos, this);
                rook.currentposition = kingNextPos.addX(-1);
            }
            if(direction == 2)
            {
                Position rookPos = kingPos.addX(-4);
                Piece rook = boardstate.getPieceAt(rookPos);
                boardstate.removeAt(kingPos);
                boardstate.removeAt(rookPos);
                boardstate.removeAt(kingNextPos);
                boardstate.addAt(kingNextPos.addX(1), rook);
                boardstate.addAt(kingNextPos, this);
                rook.currentposition = kingNextPos.addX(1);
            }
        }
        private void reverseCastle(int direction,Position kingLastPos, Position kingNewPosition, BoardState boardstate)
        {
            if (direction == 1)
            {
                Piece rook = boardstate.getPieceAt(kingNewPosition.addX(-1));
                boardstate.removeAt(kingNewPosition.addX(-1));
                boardstate.removeAt(kingNewPosition);
                boardstate.addAt(kingLastPos, this);
                boardstate.addAt(kingLastPos.addX(3), rook);
                rook.currentposition = kingLastPos.addX(3);
            }
            if (direction == 2)
            {
                Piece rook = boardstate.getPieceAt(kingNewPosition.addX(1));
                boardstate.removeAt(kingNewPosition.addX(1));
                boardstate.removeAt(kingNewPosition);
                boardstate.addAt(kingLastPos, this);
                boardstate.addAt(kingLastPos.addX(-4), rook);
                rook.currentposition = kingLastPos.addX(-4);
            }

        }
    }
}
namespace chess_openings
{
    public class ChessBoard
    {

        //the computational version of the chess Table 

        const int X = 8;
        const int Y = 8;
        string[,] BOARD = new string[X, Y];


        enum Pieces { Pawn, Rook, Knight, Bishop, Queen, King }

        public void CreateBoard()
        {

            for (int i = 1; i <= 8; i++)
                for (int j = 1; j <= 2; j++)
                {
                    if (j == 2) BOARD[i, j] = Pieces.Pawn.ToString();

                }

        }

    }
}

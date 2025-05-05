namespace _4_Gewinnt_Logik
{
    public class LogicMaster
    {
        public int Xlen { get; private set; } = -1;
        public int Ylen { get; private set; } = -1;
        public List<List<int>> GameField { get; private set; }

        public event Action<int>? OnWin;
        public LogicMaster(int Xlen, int Ylen)
        {
            this.Xlen = Xlen;
            this.Ylen = Ylen;

            GameField = new List<List<int>>();
            for (int X = 0; X < Xlen; X++)
            {
                GameField.Add(new List<int>());
                for (int Y = 0; Y < Ylen; Y++)
                {
                    GameField[X].Add(0);
                }
            }
        }


        public void Play(int X, int Player)
        {
            if (X < 0 || X >= Xlen)
                throw new ArgumentOutOfRangeException("X or Y is out of range");
            int Y = GetLowestFreeRow(X);
            if (Y == -1)
                throw new InvalidOperationException("Column is full");
            if (GameField[X][Y] != 0)
                throw new InvalidOperationException("Field is already occupied");

            GameField[X][Y] = Player;
            CheckWinner();
        }


        private int GetLowestFreeRow(int X)
        {
            for (int Y = Ylen-1; Y >= 0; Y--)
            {
                if (GameField[X][Y] == 0)
                    return Y;
            }
            return -1;
        }

        private int CheckWinner()
        {
            for (int x = 0; x < Xlen; x++)
            {
                for (int y = 0; y < Ylen; y++)
                {
                    int player = GameField[x][y];
                    if (player == 0)
                        continue;

                    // Horizontal
                    if (x + 3 < Xlen &&
                        GameField[x + 1][y] == player &&
                        GameField[x + 2][y] == player &&
                        GameField[x + 3][y] == player)
                    {
                        OnWin?.Invoke(player);
                        return player;
                    }

                    // Vertikal
                    if (y + 3 < Ylen &&
                        GameField[x][y + 1] == player &&
                        GameField[x][y + 2] == player &&
                        GameField[x][y + 3] == player)
                    {
                        OnWin?.Invoke(player);
                        return player;
                    }

                    // Diagonal rechts oben
                    if (x + 3 < Xlen && y + 3 < Ylen &&
                        GameField[x + 1][y + 1] == player &&
                        GameField[x + 2][y + 2] == player &&
                        GameField[x + 3][y + 3] == player)
                    {
                        OnWin?.Invoke(player);
                        return player;
                    }

                    // Diagonal links oben
                    if (x - 3 >= 0 && y + 3 < Ylen &&
                        GameField[x - 1][y + 1] == player &&
                        GameField[x - 2][y + 2] == player &&
                        GameField[x - 3][y + 3] == player)
                    {
                        OnWin?.Invoke(player);
                        return player;
                    }
                }
            }

            return -1;
        }
    }
}

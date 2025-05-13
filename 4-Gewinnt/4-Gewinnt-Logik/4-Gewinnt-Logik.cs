using System.Diagnostics;
using System.Numerics;

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

        public void debug_Field()
        {
            Debug.WriteLine("Start-");
            for (int Y = Ylen - 1; Y >= 0; Y--)
            {
                for (int X = 0; X < Xlen; X++)
                {
                    Debug.Write(GameField[X][Y] + " ");
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("-End");
        }

        public void Play(int X, int Player)
        {
            if (X < 0 || X >= Xlen)
                throw new ArgumentOutOfRangeException("X is out of range");
            int Y = GetFreeYforX(X);
            if (Y == -1)
                throw new InvalidOperationException("Column is full");
            if (GameField[X][Y] != 0)
                throw new InvalidOperationException("Field is already occupied");

            SetField(X, Y, Player);
            int player = CheckWinner();
            if (player != -1)
                OnWin?.Invoke(player);
        }

        public void SetField(int X, int Y, int Player)
        {
            if (X < 0 || X >= Xlen || Y < 0 || Y >= Ylen)
                throw new ArgumentOutOfRangeException("X or Y is out of range");
            GameField[X][Y] = Player;
        }


        public int GetFreeYforX(int X)
        {
            for (int Y = Ylen-1; Y >= 0; Y--)
            {
                if (GameField[X][Y] == 0)
                    return Y;
            }
            return -1;
        }

        public int CheckWinner()
        {
            //debug_Field();
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
                        return player;
                    }

                    // Vertikal
                    if (y + 3 < Ylen &&
                        GameField[x][y + 1] == player &&
                        GameField[x][y + 2] == player &&
                        GameField[x][y + 3] == player)
                    {
                        return player;
                    }

                    // Diagonal rechts oben
                    if (x + 3 < Xlen && y + 3 < Ylen &&
                        GameField[x + 1][y + 1] == player &&
                        GameField[x + 2][y + 2] == player &&
                        GameField[x + 3][y + 3] == player)
                    {
                        return player;
                    }

                    // Diagonal links oben
                    if (x - 3 >= 0 && y + 3 < Ylen &&
                        GameField[x - 1][y + 1] == player &&
                        GameField[x - 2][y + 2] == player &&
                        GameField[x - 3][y + 3] == player)
                    {
                        return player;
                    }
                }
            }

            return -1;
        }
    }
}

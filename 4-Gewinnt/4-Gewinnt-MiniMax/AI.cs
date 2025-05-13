using _4_Gewinnt_Logik;
using System.ComponentModel;
using System.Diagnostics;

namespace _4_Gewinnt_MiniMax
{
    public class AI
    {
        private LogicMaster myGame;
        private int myMarker;
        private int emtyMarker;
        private int enimyMarker;
        private readonly int maxDepth = 6;
        public AI(LogicMaster game, int myMarker,int enimyMarker, int emtyMarker)
        {
            myGame = game;
            this.myMarker = myMarker;
            this.emtyMarker = emtyMarker;
            this.enimyMarker = enimyMarker;
        }

        public void makeMove()
        {
            List<int> move = getBestMove();
            myGame.Play(move[0], myMarker);
        }
        private List<int> getBestMove()
        {
            List<int> bestMove = [-1,-1];
            int bestValue = int.MinValue;
            for (int X = 0; X < myGame.Xlen; X++)
            {
                int Y = myGame.GetFreeYforX(X);
                if (Y >= 0)
                {
                    
                    myGame.SetField(X, Y, myMarker);
                    int moveValue = MiniMax(maxDepth, false);
                    myGame.SetField(X, Y, emtyMarker);

                    if(moveValue >= bestValue)
                    {
                        bestValue = moveValue;
                        bestMove[0] = X;
                        bestMove[1] = Y;
                    }
                }
            }
            return bestMove;
        }

        private int MiniMax(int Depth, bool isMax)
        {
            int boardVal = bewerten();
            if (Math.Abs(boardVal) == 10 || Depth == 0)
            {
                return boardVal;
            }
            if (isMax)
            {
                int best = int.MinValue;
                for (int X = 0; X < myGame.Xlen; X++)
                {
                    int Y = myGame.GetFreeYforX(X);
                    if (Y >= 0)
                    {
                        myGame.SetField(X, Y, myMarker);
                        best = Math.Max(best, MiniMax(Depth - 1, false));
                        myGame.SetField(X, Y, emtyMarker);
                    }
                }
                return best;
            }
            else
            {
                int best = int.MaxValue;
                for (int X = 0; X < myGame.Xlen; X++)
                {
                    int Y = myGame.GetFreeYforX(X);
                    if (Y >= 0)
                    {
                        myGame.SetField(X, Y, enimyMarker);
                        best = Math.Min(best, MiniMax(Depth - 1, true));
                        myGame.SetField(X, Y, emtyMarker);
                    }
                }
                return best;
            }
        }

        private int bewerten()
        {
            int val = myGame.CheckWinner();

            if (val == myMarker)
            {
                return 10;
            }
            else if (val == -1)
            {
                return 0;
            }
            else
            {
                return -10;
            }
        }
    }
}

// for i in Xlen 7
// for i in range(maxdepth): 6
// for i in XLen 7
// CheckWinner
// for i in XLen 7
// for i in YLen 6

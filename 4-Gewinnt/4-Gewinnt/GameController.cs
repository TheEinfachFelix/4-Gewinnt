using _4_Gewinnt_Logik;
using _4_Gewinnt_MiniMax;
using System;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _4_Gewinnt_WPF
{
    public class GameController
    {
        private LogicMaster logic;
        private readonly Grid boardGrid;
        private readonly int cellSize = 40;
        private readonly Ellipse[,] cellVisuals;
        private int xLen;
        private int yLen;
        private AI ai;

        public GameController(Grid boardGrid, int xLen, int yLen)
        {
            this.boardGrid = boardGrid;
            this.xLen = xLen;
            this.yLen = yLen;
            newGame();

            cellVisuals = new Ellipse[xLen, yLen];
            CreateBoardVisuals(xLen, yLen);
        }

        private void newGame()
        {
            logic = new LogicMaster(xLen, yLen);
            logic.OnWin += HandleWin;
            ai = new(logic, 2,1, 0);
        }

        private void CreateBoardVisuals(int xLen, int yLen)
        {
            boardGrid.Children.Clear();
            boardGrid.RowDefinitions.Clear();
            boardGrid.ColumnDefinitions.Clear();

            for (int y = 0; y < yLen + 1; y++)
                boardGrid.RowDefinitions.Add(new RowDefinition());

            for (int x = 0; x < xLen; x++)
                boardGrid.ColumnDefinitions.Add(new ColumnDefinition());

            // Spielfeld-Zellen
            for (int x = 0; x < xLen; x++)
            {
                for (int y = 0; y < yLen; y++)
                {
                    var ellipse = new Ellipse
                    {
                        Width = cellSize,
                        Height = cellSize,
                        Stroke = Brushes.Black,
                        Fill = Brushes.White,
                        Margin = new System.Windows.Thickness(2)
                    };

                    Grid.SetColumn(ellipse, x);
                    Grid.SetRow(ellipse, y);
                    boardGrid.Children.Add(ellipse);
                    cellVisuals[x, y] = ellipse;
                }

                // Button unter jeder Spalte
                var btn = new Button
                {
                    Content = "↓",
                    Tag = x
                };
                btn.Click += OnColumnClick;
                Grid.SetColumn(btn, x);
                Grid.SetRow(btn, yLen);
                boardGrid.Children.Add(btn);
            }
        }

        private int currentPlayer = 1;

        private void OnColumnClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int x)
            {
                try
                {
                    logic.Play(x, currentPlayer);
                    UpdateBoard();
                    Thread.Sleep(500);
                    ai.makeMove();
                    UpdateBoard();
                    //currentPlayer = currentPlayer == 1 ? 2 : 1;
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(ex.Message);
                }
            }
        }

        private void UpdateBoard()
        {
            for (int x = 0; x < logic.Xlen; x++)
            {
                for (int y = 0; y < logic.Ylen; y++)
                {
                    int player = logic.GameField[x][y];
                    cellVisuals[x, y].Fill = player switch
                    {
                        1 => Brushes.Red,
                        2 => Brushes.Yellow,
                        _ => Brushes.White
                    };
                }
            }
        }

        private void HandleWin(int player)
        {
            UpdateBoard();
            //MessageBox.Show($"Spieler {player} hat gewonnen!");
            MessageBoxResult dialogResult = MessageBox.Show(
            $"Spieler {player} hat gewonnen!",
            "Ende",
            MessageBoxButton.OK);

            newGame();

        }
    }
}

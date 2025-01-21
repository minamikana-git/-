using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace テトリス
{
    public partial class TetrisForm : Form
    {
        private PictureBox gamePictureBox;
        private Label scoreLabel;
        private Label levelLabel;
        private Label linesLabel; // ライン数の表示
        private List<PictureBox> nextBlockPictureBoxes = new List<PictureBox>(); // ネクストブロックの表示
        private Timer gameTimer;

        private const int GridWidth = 10;
        private const int GridHeight = 20;
        private int[,] grid = new int[GridHeight, GridWidth];

        private int[,] currentBlock;
        private int blockX = 0;
        private int blockY = 0;
        private Queue<int> nextBlocks = new Queue<int>(); // ネクストブロックのキュー

        private readonly int[][,] blocks = new int[][,]
        {
            new int[,] {{1, 1, 1, 1}}, // I
            new int[,] {{1, 1}, {1, 1}}, // O
            new int[,] {{0, 1, 0}, {1, 1, 1}}, // T
            new int[,] {{1, 0}, {1, 1}, {0, 1}}, // S
            new int[,] {{0, 1}, {1, 1}, {1, 0}}, // Z
            new int[,] {{1, 1, 1}, {1, 0, 0}}, // L
            new int[,] {{1, 1, 1}, {0, 0, 1}}  // J
        };

        private readonly Brush[] blockColors = new Brush[]
        {
            Brushes.Cyan,    // I
            Brushes.Yellow,  // O
            Brushes.Purple,  // T
            Brushes.Green,   // S
            Brushes.Red,     // Z
            Brushes.Orange,  // L
            Brushes.Blue     // J
        };

        private int currentBlockColorIndex;

        private int score = 0;
        private int level = 1;
        private int linesCleared = 0;

        private bool isPaused = false; // ポーズ状態を管理

        public TetrisForm()
        {
            InitializeComponent();
            this.Load += TetrisForm_Load;
        }

        private void TetrisForm_Load(object sender, EventArgs e)
        {
            InitializeGameComponents();
            InitializeGame();
        }

        private void InitializeGameComponents()
        {

            this.Text = "テトリス";

            // ゲーム画面
            gamePictureBox = new PictureBox
            {
                Size = new Size(200, 400),
                Location = new Point(20, 20),
                BackColor = Color.Gray
            };
            Controls.Add(gamePictureBox);

            // スコア表示
            scoreLabel = new Label
            {
                Text = "スコア: 0",
                Location = new Point(240, 20),
                Font = new Font("Arial", 14)
            };
            Controls.Add(scoreLabel);

            // レベル表示
            levelLabel = new Label
            {
                Text = "レベル: 1",
                Location = new Point(240, 60),
                Font = new Font("Arial", 14)
            };
            Controls.Add(levelLabel);

            // ライン数表示
            linesLabel = new Label
            {
                Text = "ライン: 0",
                Location = new Point(240, 100),
                Font = new Font("Arial", 14)
            };
            Controls.Add(linesLabel);

            // 次のブロック表示 (4つ)
            for (int i = 0; i < 4; i++)
            {
                var nextBlockBox = new PictureBox
                {
                    Size = new Size(60, 60), // サイズを小さく調整
                    Location = new Point(240, 140 + (i * 70)), // 縦の間隔を狭める
                    BackColor = Color.LightGray
                };
                Controls.Add(nextBlockBox);
                nextBlockPictureBoxes.Add(nextBlockBox);
            }
        }

        private void InitializeGame()
        {
            InitializeGameTimer();
            InitializeNextBlocks();
            GenerateNewBlock();
        }

        private void InitializeGameTimer()
        {
            gameTimer = new Timer();
            gameTimer.Interval = 500; // 初期速度 (ミリ秒)
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
        }

        private void InitializeNextBlocks()
        {
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                nextBlocks.Enqueue(random.Next(blocks.Length));
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            if (isPaused) return; // ポーズ中は処理をスキップ

            blockY++; // ブロックを1段下に移動

            if (CheckCollision())
            {
                blockY--; // 衝突したら元に戻す
                PlaceBlock(); // グリッドにブロックを固定
                GenerateNewBlock(); // 新しいブロックを生成
            }

            DrawGrid(); // 描画を更新
        }

        private void GenerateNewBlock()
        {
            Random random = new Random();
            int blockIndex = nextBlocks.Dequeue();
            currentBlock = blocks[blockIndex];
            currentBlockColorIndex = blockIndex; // 色インデックスを設定
            blockX = GridWidth / 2 - currentBlock.GetLength(1) / 2;
            blockY = 0;

            // 次のブロックを補充
            nextBlocks.Enqueue(random.Next(blocks.Length));
            UpdateNextBlockDisplay();

            // ゲームオーバー判定
            if (CheckCollision())
            {
                gameTimer.Stop();
                MessageBox.Show("ゲームオーバー！ スコア: " + score, "ゲームオーバー");
                ResetGame();
            }
        }

        private void UpdateNextBlockDisplay()
        {
            int index = 0;
            foreach (var blockIndex in nextBlocks)
            {
                if (index >= nextBlockPictureBoxes.Count) break;

                var bitmap = new Bitmap(nextBlockPictureBoxes[index].Width, nextBlockPictureBoxes[index].Height);
                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.Clear(Color.LightGray);
                    var block = blocks[blockIndex];
                    var color = blockColors[blockIndex];
                    int cellSize = nextBlockPictureBoxes[index].Width / 4;

                    for (int y = 0; y < block.GetLength(0); y++)
                    {
                        for (int x = 0; x < block.GetLength(1); x++)
                        {
                            if (block[y, x] == 1)
                            {
                                g.FillRectangle(color, x * cellSize, y * cellSize, cellSize, cellSize);
                                g.DrawRectangle(Pens.Black, x * cellSize, y * cellSize, cellSize, cellSize);
                            }
                        }
                    }
                }
                nextBlockPictureBoxes[index].Image = bitmap;
                index++;
            }
        }

        private void DrawGrid()
        {
            Bitmap bitmap = new Bitmap(gamePictureBox.Width, gamePictureBox.Height);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                int cellSize = gamePictureBox.Width / GridWidth;

                // 点線のペン
                Pen dashedPen = new Pen(Color.Black, 1) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };

                // グリッドを描画
                for (int y = 0; y < GridHeight; y++)
                {
                    for (int x = 0; x < GridWidth; x++)
                    {
                        if (grid[y, x] > 0)
                        {
                            // セルの色を塗る
                            g.FillRectangle(blockColors[grid[y, x] - 1], x * cellSize, y * cellSize, cellSize, cellSize);

                            // 点線の境界線を描く
                            g.DrawRectangle(dashedPen, x * cellSize, y * cellSize, cellSize, cellSize);
                        }
                    }
                }

                // 現在のブロックを描画
                for (int y = 0; y < currentBlock.GetLength(0);y++)
                {
                    for (int x = 0; x < currentBlock.GetLength(1); x++)
                    {
                        if (currentBlock[y, x] == 1)
                        {
                            // セルの色を塗る
                            g.FillRectangle(blockColors[currentBlockColorIndex], (blockX + x) * cellSize, (blockY + y) * cellSize, cellSize, cellSize);

                            // 点線の境界線を描く
                            g.DrawRectangle(dashedPen, (blockX + x) * cellSize, (blockY + y) * cellSize, cellSize, cellSize);
                        }
                    }
                }
            }

            gamePictureBox.Image = bitmap;
        }

        private bool CheckCollision()
        {
            for (int y = 0; y < currentBlock.GetLength(0); y++)
            {
                for (int x = 0; x < currentBlock.GetLength(1); x++)
                {
                    if (currentBlock[y, x] == 1)
                    {
                        int newX = blockX + x;
                        int newY = blockY + y;

                        // フィールドの境界または既存のブロックと衝突
                        if (newX < 0 || newX >= GridWidth || newY >= GridHeight || (newY >= 0 && grid[newY, newX] != 0))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void PlaceBlock()
        {
            for (int y = 0; y < currentBlock.GetLength(0);y++)
            {
                for (int x = 0; x < currentBlock.GetLength(1); x++)
                {
                    if (currentBlock[y, x] == 1)
                    {
                        int newX = blockX + x;
                        int newY = blockY + y;
                        grid[newY, newX] = currentBlockColorIndex + 1; // 色インデックスを格納
                    }
                }
            }

            ClearLines(); // ライン消去処理
        }

        private void ClearLines()
        {
            int linesClearedThisTurn = 0;

            for (int y = 0; y < GridHeight; y++)
            {
                bool fullLine = true;
                for (int x = 0; x < GridWidth; x++)
                {
                    if (grid[y, x] == 0)
                    {
                        fullLine = false;
                        break;
                    }
                }

                if (fullLine)
                {
                    linesClearedThisTurn++;
                    // 上の行を1段ずつ下げる
                    for (int row = y; row > 0; row--)
                    {
                        for (int col = 0; col < GridWidth; col++)
                        {
                            grid[row, col] = grid[row - 1, col];
                        }
                    }

                    // 最上段をクリア
                    for (int col = 0; col < GridWidth; col++)
                    {
                        grid[0, col] = 0;
                    }
                }
            }

            if (linesClearedThisTurn > 0)
            {
                linesCleared += linesClearedThisTurn; // 合計ライン数を更新
                UpdateScore(linesClearedThisTurn);
                linesLabel.Text = $"ライン: {linesCleared}"; // ライン数を表示
            }
        }

        private void UpdateScore(int linesClearedThisTurn)
        {
            // ライン消去に応じたスコア加算
            int[] scoreTable = { 0, 100, 300, 500, 800 }; // 1~4行消去時のスコア
            if (linesClearedThisTurn > 0 && linesClearedThisTurn < scoreTable.Length)
            {
                score += scoreTable[linesClearedThisTurn];
            }
            scoreLabel.Text = $"スコア: {score}";

            // レベルアップ処理 (例: 10行ごとにレベルアップ)
            int newLevel = (linesCleared / 10) + 1;
            if (newLevel > level)
            {
                level = newLevel;
                levelLabel.Text = $"レベル: {level}";

                // ゲーム速度を上げる
                gameTimer.Interval = Math.Max(100, gameTimer.Interval - 50);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    blockX--;
                    if (CheckCollision()) blockX++;
                    break;
                case Keys.Right:
                    blockX++;
                    if (CheckCollision()) blockX--;
                    break;
                case Keys.Down:
                    blockY++;
                    if (CheckCollision()) blockY--;
                    break;
                case Keys.Z:
                    RotateBlock();
                    if (CheckCollision()) RotateBlock(false); // 右回転を元に戻す
                    break;
                case Keys.X:
                    RotateBlock(false); // 左回転
                    if (CheckCollision()) RotateBlock(); // 左回転を元に戻す
                    break;
                case Keys.Up:
                    while (!CheckCollision())
                    {
                        blockY++;
                    }
                    blockY--;
                    PlaceBlock();
                    GenerateNewBlock();
                    break;
                case Keys.P:
                    TogglePause(); // ポーズを切り替え
                    break;
            }

            DrawGrid();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                gameTimer.Stop();
                MessageBox.Show("ポーズ中", "ポーズ");
            }
            else
            {
                gameTimer.Start();
            }
        }

        private void RotateBlock(bool clockwise = true)
        {
            int[,] rotatedBlock = new int[currentBlock.GetLength(1), currentBlock.GetLength(0)];
            for (int y = 0; y < currentBlock.GetLength(0); y++)
            {
                for (int x = 0; x < currentBlock.GetLength(1); x++)
                {
                    rotatedBlock[x, y] = clockwise ? currentBlock[currentBlock.GetLength(0) - y - 1, x] : currentBlock[y, currentBlock.GetLength(1) - x - 1];
                }
            }
            currentBlock = rotatedBlock;
        }

        private void ResetGame()
        {
            grid = new int[GridHeight, GridWidth];
            score = 0;
            level = 1;
            linesCleared = 0;
            gameTimer.Interval = 500;
            scoreLabel.Text = "スコア: 0";
            levelLabel.Text = "レベル: 1";
            linesLabel.Text = "ライン: 0";
            InitializeNextBlocks();
            GenerateNewBlock();
            gameTimer.Start();
        }
    }
}

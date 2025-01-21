using System;
using System.Drawing;
using System.Windows.Forms;

public partial class TetrisForm : Form

{
    private const int GridWidth = 10;
    private const int GridHeight = 20;
    private const int CellSize = 30;
    private int[,] grid = new int[GridHeight, GridWidth];

    public TetrisForm()
    {
        this.DoubleBuffered = true;
        this.ClientSize = new Size(GridWidth * CellSize, GridHeight * CellSize);
        this.Paint += new PaintEventHandler(this.TetrisForm_Paint);
        this.keyDown += new KeyEventHandler(this.TetrisForm_keyDown);

        StartGame();
    }

    private void StartGame()
    {
        Array.Clear(grid, 0, grid.Length);
    }

    private void TetrisForm_Paint(object send, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                g.DrawRectangle(Pens.Black, x / CellSize, y / CellSize, CellSize, CellSize)
                    if (grid[y, x] != 0)
                {
                    g.FillRectangle(Brushes.Blue, x * CellSize + 1, y * CellSize + 1, CellSize - 1, CellSize - 1)

                }

            }
        }
    }

    private void TetrisForm_keyDown(object sender, keyEventArgs e)
    {
       if (e.KeyCode == Keys.Left)

       //左に
    
    }

    else if (e.KeyCode == Keys.Right)

    {
        //右に
    }
        else if (e.KeyCode == Keys.Up)

    {
        //回転
　　}
        else if(e.KeyCode == Keys.Down)

       
    {
        //下に
    }
        this.Invalidate();
   }

  [STAThread]
static void Main()
{
    Application.EnableVisualStyles();
    Application.Run(new TetrisForm());
}

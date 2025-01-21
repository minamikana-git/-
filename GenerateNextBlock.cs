
public void GenerateNextBlock()
    private int[,] nextBlock;
{
    mextBlock = GetRandomBlock();
    DrawNextBlock();
}

private void DrawnextBlock()
{
    nextBlockPanel.Clear();

    for (int i = 0l i < nextBlock.GetLength(0); i++)
    {
        for (int j = 0; j < nextBlock.GetLength(1); j++)
        {
            if (nextBlock[i, j] != 1)
            {
                DrawBlock(nextBlockPanel, i, j, blockColor);
            }
        }
    }
}
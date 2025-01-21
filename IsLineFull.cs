private bool IsLineFull(int y)
{
    for (int x = 0; x < gridWidth; x++)
    {
        if (grid[y, x] == 0)
        {
            return false; // 空きがあるため埋まっていない
        }
    }
    return true;
}

// ラインを消去する
private void ClearLine(int y)
{
    for (int x = 0; x < gridWidth; x++)
    {
        grid[y, x] = 0; // ラインをクリア
    }

    // 上のラインを一段ずつ落とす
    for (int row = y; row > 0; row--)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            grid[row, x] = grid[row - 1, x];
        }
    }
}

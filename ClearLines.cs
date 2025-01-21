public void ClearLines()
{
    int linesCleared = 0;

    // グリッドのチェックと消去処理
    for (int y = gridHeight - 1; y >= 0; y--)
    {
        if (IsLineFull(y))
        {
            ClearLine(y);
            linesCleared++;
        }
    }

    if (linesCleared > 0)
    {
        UpdateScore(linesCleared); // スコア更新
    }
}

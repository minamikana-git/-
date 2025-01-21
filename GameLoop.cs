

private void GameLoop(object sender, EventArgs e)
{
   if (CheckGameOver())
    {
        GameOver();
        return;
    }

    MoveBlockDown();
    
    if (IsBlockLanded())
    {
        ClearLines();
        CheckLevelUp(lineCleared);
        GenerateNextBlock();
    }

    UpdateGameBoard();

}
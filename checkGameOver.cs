

public bool checkGameOver()
{ for (int i = 0; i < gridWidgth; i++)
    {
        if (grid[0, 1] == 1)
        {
            return true;
        }
    }
    return false;
}    
	
private void GameOver()
{
    timer.Stop();
    MessageBox.Show("ゲームオーバー");
    ResetGame();
}

private void ResetGame()
{
    Array.Clear(grid,0,grid.Length);
    score = 0;
    level = 1;
    lineClearedTotal = 0;
    dropSpeed = 500;


    StartGame();

}
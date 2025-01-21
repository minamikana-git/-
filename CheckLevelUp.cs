
public void CheckLevelUp(int linesCleared)

    private int level = 1;
    private int lineClearedTotal = 0;
    private int dropSpeed = 500;

{
    lineClearedTotal += linesCleared;
    if (lineClearedTotal / 10 >= level)
    {
        level++;
        dropSpeed = Math.Max(100, dropSpeed - 50);
        UpdateLevelLabel();

    }
}

private void UpdateLevelLabel()
{
    levelLabel.Text = $"ƒŒƒxƒ‹: {level}";
}
using System;

private Timer gameTimer;

private void InitializeGameTimer()
{
    gameTimer = new Timer();
    gameTimer.Interval = 500; // 0.5秒ごとに更新
    gameTimer.Tick += new EventHandler(GameTimer_Tick);
    gameTimer.Start();
}

private void GameTimer_Tick(object sender, EventArgs e)
{
    // ブロックを下に移動
    MoveBlockDown();
    this.Invalidate(); // 再描画
}

private void MoveBlockDown()
{
    // 落下処理
}

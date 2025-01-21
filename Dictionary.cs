// スコア変数の宣言
private int score = 0;

// スコアテーブル（基本スコア）
private readonly Dictionary<int, int> scoreTable = new Dictionary<int, int>
{
    { 1, 100 }, // シングル
    { 2, 300 }, // ダブル
    { 3, 500 }, // トリプル
    { 4, 800 }  // テトリス
};

// スコア更新メソッド
public void UpdateScore(int linesCleared)
{
    // スコアテーブルに基づいて加算
    if (scoreTable.ContainsKey(linesCleared))
    {
        score += scoreTable[linesCleared];
    }

    // スコアを表示
    UpdateScoreLabel();
}

// スコア表示の更新
private void UpdateScoreLabel()
{
    scoreLabel.Text = $"Score: {score}";
}

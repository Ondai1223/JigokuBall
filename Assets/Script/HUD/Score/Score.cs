namespace HUD.Score
{
    public class Score
    {
        public static Score Instance;
        private int scoreNum;
        public int ScoreNum => scoreNum;

        public static Score GetInstance()
        {
            if (Instance == null)
            {
                Instance = new Score();
                Instance.scoreNum = 0;
            }
            return Instance;
        }

        public void AddScore(int newScoreNum)
        {
            scoreNum += newScoreNum;
            UnityEngine.Debug.Log("Score Added: " + newScoreNum.ToString() + ", Total Score: " + scoreNum.ToString());
        }

        public void ResetScore()
        {
            scoreNum = 0;
        }
    }
}
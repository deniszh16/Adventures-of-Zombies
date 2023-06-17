namespace Services.Achievements
{
    public interface IAchievementsService
    {
        public bool CheckAchievementCompletion(int number);
        public void RunAchievementCheck();
    }
}
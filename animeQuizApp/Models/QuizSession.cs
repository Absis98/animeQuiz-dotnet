public class QuizSession
{
    public int Id { get; set; }
    public DateTime CreateTime { get; set; }
    public string QuizName { get; set; }
    public string Questions { get; set; }
    public string? Answers { get; set; }
    public int? Score { get; set; }
}

public class QuizType
{
    public int Id { get; set; }
    public string QuizName { get; set; }
    public string SubText { get; set; }
    public int TotalAttempts { get; set; } = 0;
    public string ImageURL { get; set; }
}

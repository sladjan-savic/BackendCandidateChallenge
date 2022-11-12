namespace QuizService.Model;

public class QuestionUpdateModel
{
    public string Text { get; set; }
    // TODO: In my opinion, the correct answer should be stored as text,
    // and then the answers text could be compared to it in lowercase.
    // It makes no sense to mark one answer as correct,
    // since whoever creates the Answer resource with such id wins (regardless of what was the answer or the question),
    // while evryone lese is wrong, which cannot be right.
    public int CorrectAnswerId { get; set; }
}
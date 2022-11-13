namespace QuizService.Model.Domain;

public class Answer
{
    // TODO: Properties like this, which repeat on every entity could be moved into parent class
    // (BaseEntity, AuditableEntity. Useful if we want to audit entites with data like creation time, modification time and so on).
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; }
}
public class Feedback
{

    public int Rating { get; set; }
    public string Comment { get; set; }


    public Feedback(Entity.StudentFeedback feedback)
    {
        Rating = feedback.Rating;
        Comment = feedback.Comment;
    }

    public Feedback(Entity.CompanyFeedback feedback)
    {
        Rating = feedback.Rating;
        Comment = feedback.Comment;
    }

    public Feedback(DTO.Feedback feedback)
    {
        Rating = feedback.Rating;
        Comment = feedback.Comment;
    }

    public Entity.StudentFeedback ToStudentFeedbackEntity()
    {
        return new Entity.StudentFeedback
        {
            Rating = Rating,
            Comment = Comment,
        };
    }

    public Entity.CompanyFeedback ToCompanyFeedbackEntity()
    {
        return new Entity.CompanyFeedback
        {
            Rating = Rating,
            Comment = Comment,
        };
    }

    public DTO.Feedback ToDto()
    {
        return new DTO.Feedback
        {
            Rating = Rating,
            Comment = Comment,
        };
    }
}

using AutoMapper;

namespace QuizCraft.Domain.API.Profiles;

public class QuizzesProfiles : Profile
{
    public QuizzesProfiles()
    {
        CreateMap<Entities.Quiz, Models.QuizDto>();

        CreateMap<Entities.Question, Models.QuestionDto>();
        
        CreateMap<Entities.Answer, Models.AnswerDto>();


        CreateMap<Models.QuizDto, Entities.Quiz>();

        CreateMap<Models.QuestionDto, Entities.Question>();

        CreateMap<Models.AnswerDto, Entities.Answer>();
    }
}
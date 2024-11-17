using AutoMapper;

namespace QuizCraft.Domain.API.Profiles;

public class QuizzesProfiles : Profile
{
    public QuizzesProfiles()
    {
        CreateMap<Models.QuizDtoForCreation, Entities.Quiz>();
        CreateMap<Models.QuestionForCreationDto, Entities.Question>();
        CreateMap<Models.AnswerDto, Entities.Answer>();
        CreateMap<Models.AnswerForCreationDto, Entities.Answer>();

        CreateMap<Entities.Quiz, Models.QuizDto>()
            .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Questions.Count()));

        CreateMap<Entities.Question, Models.QuestionDto>();
        CreateMap<Entities.Answer, Models.AnswerDto>();

        CreateMap<Entities.QuizAttempt, Models.QuizAttemptDto>()
            .ForMember(dest => dest.CorrectAnswers, opt => opt.MapFrom(src => src.QuizAnswerAttempts.Count(a => a.Answer.IsCorrect)));
    }
}
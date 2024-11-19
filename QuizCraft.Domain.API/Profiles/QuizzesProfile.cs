using AutoMapper;

namespace QuizCraft.Domain.API.Profiles;

public class QuizzesProfiles : Profile
{
    public QuizzesProfiles()
    {
        CreateMap<Models.QuizDtoForCreation, Entities.Quiz>()
            .ForMember(dest => dest.CreatedBy, opt => opt.Ignore());
        CreateMap<Models.QuestionForCreationDto, Entities.Question>();
        CreateMap<Models.AnswerDto, Entities.Answer>();
        CreateMap<Models.AnswerForCreationDto, Entities.Answer>();

        CreateMap<Entities.Quiz, Models.QuizForTransferDto>()
            .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Questions.Count()))
            .ForMember(dest => dest.CompletedBy, opt => opt.MapFrom(src =>
                src.QuizAttempts
                    .GroupBy(x => x.UserEmail)
                    .Count(group => group.Any(x => x.IsCompleted))
            ))
            .ForMember(dest => dest.AverageScore, opt => opt.MapFrom(src =>
                (int) Math.Round(
                    src.QuizAttempts
                        .Where(x => x.IsCompleted)
                        .Select(x => x.QuizAnswerAttempts.Count(a => a.Answer.IsCorrect))
                        .Average()
                )
            ))
            .ForMember(dest => dest.HighScore, opt => opt.MapFrom(src =>
                src.QuizAttempts
                    .Where(x => x.IsCompleted)
                    .Select(x => x.QuizAnswerAttempts.Count(a => a.Answer.IsCorrect))
                    .Max()
            ))
            .ForMember(dest => dest.IsOwner, opt => opt.Ignore());

        CreateMap<Entities.Question, Models.QuestionDto>();
        CreateMap<Entities.Answer, Models.AnswerDto>();

        CreateMap<Entities.QuizAttempt, Models.QuizAttemptDto>()
            .ForMember(dest => dest.CorrectAnswers, opt => opt.MapFrom(src => src.QuizAnswerAttempts.Count(a => a.Answer.IsCorrect)));
    }
}
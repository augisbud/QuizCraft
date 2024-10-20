using AutoMapper;

namespace QuizCraft.Domain.API.Profiles;

public class QuizzesProfiles : Profile
{
    public QuizzesProfiles()
    {
        CreateMap<Models.QuizDtoForCreation, Entities.Quiz>();
        CreateMap<Models.QuestionForCreationDto, Entities.Question>();
        CreateMap<Models.AnswerDto, Entities.Answer>();

        CreateMap<Entities.Quiz, Models.QuizDto>()
            .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Questions.Count()));

        CreateMap<Entities.Question, Models.QuestionDto>()
            .ForMember(dest => dest.Answers, opt => opt.MapFrom(src => src.Answers.Select(a => a.Text)));

        CreateMap<Entities.Answer, Models.AnswerDto>();
    }
}
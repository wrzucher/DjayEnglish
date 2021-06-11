// -----------------------------------------------------------------------
// <copyright file="EntityFrameworkMappingProfile.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.Core.EntityFrameworkCore
{
    using AutoMapper;
    using DjayEnglish.Server.ObjectModels;

    /// <summary>
    /// Profile for mapping from entity framework models to object models.
    /// </summary>
    public class EntityFrameworkMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkMappingProfile"/> class.
        /// </summary>
        public EntityFrameworkMappingProfile()
        {
            this.CreateMap<EntityFramework.QuizAnswerOption, QuizAnswerOption>();
            this.CreateMap<EntityFramework.QuizExample, QuizExample>();
            this.CreateMap<EntityFramework.Quiz, Quiz>()
                .ForMember(dest => dest.QuestionType, opts => opts.MapFrom(_ => (QuestionType)_.QuestionType))
                .ForMember(dest => dest.AnswerShowType, opts => opts.MapFrom(_ => (ShowType)_.AnswerShowType))
                .ForMember(dest => dest.ExampleShowType, opts => opts.MapFrom(_ => (ShowType)_.ExampleShowType))
                .ForMember(dest => dest.QuestionShowType, opts => opts.MapFrom(_ => (ShowType)_.QuestionShowType));
            this.CreateMap<EntityFramework.TranslationUnitUsage, TranslationUnitUsage>();
            this.CreateMap<EntityFramework.TranslationUnitDefinition, TranslationUnitDefinition>();
            this.CreateMap<EntityFramework.TranslationUnit, TranslationUnit>()
                .ForMember(dest => dest.PartOfSpeech, opts => opts.MapFrom(_ => (PartOfSpeechType)_.PartOfSpeech))
                .ForMember(dest => dest.Language, opts => opts.MapFrom(_ => (LanguageType)_.Language));
        }
    }
}

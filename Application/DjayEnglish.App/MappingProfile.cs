// -----------------------------------------------------------------------
// <copyright file="MappingProfile.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.App
{
    using AutoMapper;
    using DjayEnglish.Administration.Models;

    /// <summary>
    /// Profile for mapping from entity framework models to object models.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile()
        {
            this.CreateMap<Server.ObjectModels.QuizAnswerOption, QuizAnswerOption>();
            this.CreateMap<Server.ObjectModels.QuizExample, QuizExample>();
            this.CreateMap<Server.ObjectModels.Quiz, Quiz>();
            this.CreateMap<Server.ObjectModels.TranslationUnitUsage, TranslationUnitUsage>();
            this.CreateMap<Server.ObjectModels.TranslationUnit, TranslationUnit>();
            this.CreateMap<Server.ObjectModels.TranslationUnitDefinition, TranslationUnitDefinition>();
        }
    }
}

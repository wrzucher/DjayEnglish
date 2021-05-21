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
            this.CreateMap<Server.ObjectModels.QuizeAnswerOption, QuizeAnswerOption>();
            this.CreateMap<Server.ObjectModels.QuizeExample, QuizeExample>();
            this.CreateMap<Server.ObjectModels.Quize, Quize>();
            this.CreateMap<Server.ObjectModels.WordUsage, WordUsage>();
        }
    }
}

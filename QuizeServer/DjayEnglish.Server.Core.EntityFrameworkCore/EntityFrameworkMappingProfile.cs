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
            this.CreateMap<EntityFramework.QuizeAnswerOption, QuizeAnswerOption>();
            this.CreateMap<EntityFramework.QuizeExample, QuizeExample>()
                .ForMember(dest => dest.WordUsage, opts => opts.MapFrom(_ => _.WordUsage));
            this.CreateMap<EntityFramework.Quiz, Quize>();
            this.CreateMap<EntityFramework.WordUsage, WordUsage>();
        }
    }
}

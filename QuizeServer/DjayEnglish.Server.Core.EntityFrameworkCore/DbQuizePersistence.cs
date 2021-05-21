// -----------------------------------------------------------------------
// <copyright file="DbQuizePersistence.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.Core.EntityFrameworkCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using DjayEnglish.EntityFramework;
    using DjayEnglish.Server.ObjectModels;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Quize persistence.
    /// </summary>
    public class DbQuizePersistence
    {
        private readonly DjayEnglishDBContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQuizePersistence"/> class.
        /// </summary>
        /// <param name="dbContext">Database context to use.</param>
        /// <param name="mapper">Map entity models to object models.</param>
        public DbQuizePersistence(
            DjayEnglishDBContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Check if chat with id exist.
        /// </summary>
        /// <param name="chatId">Id of the chat.</param>
        /// <returns>Indicate that chat with requested id exist.</returns>
        public bool IsChatExist(long chatId)
        {
            var chat = this.dbContext.Chats.FirstOrDefault(_ => _.Id == chatId);
            return chat != null;
        }

        /// <summary>
        /// Register new chat.
        /// </summary>
        /// <param name="chatId">Id of the new chat.</param>
        public void RegisterNewChat(long chatId)
        {
            var newChat = new Chat()
            {
                Id = chatId,
            };

            this.dbContext.Chats.Add(newChat);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Add new quize to user chat.
        /// </summary>
        /// <param name="chatId">Id of the chat for which quize added.</param>
        /// <param name="quizeId">Id of the quize which will be added to chat.</param>
        /// <param name="created">Date when quize added to chat.</param>
        /// <returns>Model of chat quize.</returns>
        public ChatQuiz AddQuizeToChat(
            long chatId,
            int quizeId,
            DateTimeOffset created)
        {
            var newChatQuiz = new ChatQuiz()
            {
                ChatId = chatId,
                QuizeId = quizeId,
                State = (byte)ChatQuizeState.IsActive,
                Created = created,
            };

            var entityChatQuiz = this.dbContext.ChatQuizzes.Add(newChatQuiz).Entity;
            this.dbContext.SaveChanges();
            return entityChatQuiz;
        }

        /// <summary>
        /// Get quize by id.
        /// </summary>
        /// <param name="quizeId">Id of the requested quize.</param>
        /// <returns>Quize model if it found in database.</returns>
        public Quize? GetQuize(int quizeId)
        {
            var quizeEntity = this.dbContext.Quizzes
                .Include(_ => _.QuizeAnswerOptions)
                .Include(_ => _.QuizeExamples)
                    .ThenInclude(_ => _.WordUsage)
                .FirstOrDefault(_ => _.Id == quizeId);
            var quizeModel = this.mapper.Map<Quize?>(quizeEntity);
            return quizeModel;
        }
    }
}

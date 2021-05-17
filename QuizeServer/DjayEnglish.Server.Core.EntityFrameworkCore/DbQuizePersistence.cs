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
    using DjayEnglish.EntityFramework;
    using DjayEnglish.Server.ObjectModels;

    /// <summary>
    /// Quize persistence.
    /// </summary>
    public class DbQuizePersistence
    {
        private readonly DjayEnglishDBContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQuizePersistence"/> class.
        /// </summary>
        /// <param name="dbContext">Database context to use.</param>
        public DbQuizePersistence(
            DjayEnglishDBContext dbContext)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
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
        /// <param name="quizeId">Id of the quize.</param>
        /// <returns>Quize if it found.</returns>
        public Quize? GetQuize(int quizeId)
        {
            var quiestion = "What does 'admire' mean?";
            var exemples = new string[]
            {
                "I want a guy I can look up to and admire.",
                "Most of all, they will admire.",
                "I cannot help but admire her dignity.",
            };
            var answerOptions = new Dictionary<string, AnswerOption>();
            answerOptions.Add("1", new AnswerOption("To find someone or something attractive and pleasant to look at.", true));
            answerOptions.Add("2", new AnswerOption("To dislike someone or something very much.", false));
            answerOptions.Add("3", new AnswerOption("With little or no light.", false));
            answerOptions.Add("4", new AnswerOption("Nearer to black than white in colour.", false));

            return new Quize(quizeId, quiestion, exemples, answerOptions);
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="DbQuizPersistence.cs" company="DjayEnglish">
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
    /// Quiz persistence.
    /// </summary>
    public class DbQuizPersistence
    {
        private readonly DjayEnglishDBContext dbContext;
        private readonly IMapper mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbQuizPersistence"/> class.
        /// </summary>
        /// <param name="dbContext">Database context to use.</param>
        /// <param name="mapper">Map entity models to object models.</param>
        public DbQuizPersistence(
            DjayEnglishDBContext dbContext,
            IMapper mapper)
        {
            this.dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get quizzes without audio files.
        /// </summary>
        /// <returns>Quiz collection which does not have audio.</returns>
        public IEnumerable<ObjectModels.Quiz> GetQuizzesWithoutAudio()
        {
            var quizzesEntity = this.dbContext.Quizzes
                .Include(_ => _.QuizAnswerOptions)
                .Include(_ => _.QuizExamples)
                    .ThenInclude(_ => _.TranslationUnitUsage)
                .Where(_ =>
                    !_.HasAudioFiles
                  && (_.ExampleShowType == (byte)ShowType.Audio
                   || _.QuestionShowType == (byte)ShowType.Audio
                   || _.AnswerShowType == (byte)ShowType.Audio));
            var quizzesModel = this.mapper.Map<IEnumerable<ObjectModels.Quiz>>(quizzesEntity);
            return quizzesModel;
        }

        /// <summary>
        /// Set quiz audio Enable.
        /// </summary>
        /// /<param name="quizId">Id of quiz for which set enable.</param>
        /// /<param name="hasAudio">Indicate that quiz has audio.</param>
        public void SetQuizAudioEnable(int quizId, bool hasAudio)
        {
            var quiz = this.dbContext.Quizzes.FirstOrDefault(_ => _.Id == quizId);
            if (quiz == null)
            {
                throw new InvalidOperationException(
                    $"Quiz {quizId} does not exist in database and audio flag couldn't ne set");
            }

            quiz.HasAudioFiles = hasAudio;
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Check if chat with id exist.
        /// </summary>
        /// <param name="chatId">Id of the chat.</param>
        /// <returns>Indicate that chat with requested id exist.</returns>
        public bool IsChatExist(long chatId)
        {
            var chat = this.dbContext.Users.FirstOrDefault(_ => _.ChatId == chatId);
            return chat != null;
        }

        /// <summary>
        /// Register new chat.
        /// </summary>
        /// <param name="chatId">Id of the new chat.</param>
        public void RegisterNewChat(long chatId)
        {
            var newChat = new User()
            {
                ChatId = chatId,
                Id = Guid.NewGuid().ToString(),
            };

            this.dbContext.Users.Add(newChat);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Add new quiz to user chat.
        /// </summary>
        /// <param name="chatId">Id of the chat for which quiz added.</param>
        /// <param name="quizId">Id of the quiz which will be added to chat.</param>
        /// <param name="created">Date when quiz added to chat.</param>
        /// <returns>Model of chat quiz.</returns>
        public ChatQuiz AddQuizToUser(
            long chatId,
            int quizId,
            DateTimeOffset created)
        {
            var newChatQuiz = new ChatQuiz()
            {
                ChatId = chatId,
                QuizId = quizId,
                State = (byte)QuizState.IsActive,
                Created = created,
            };

            var entityChatQuiz = this.dbContext.ChatQuizzes.Add(newChatQuiz).Entity;
            this.dbContext.SaveChanges();
            return entityChatQuiz;
        }

        /// <summary>
        /// Get active chat quize.
        /// </summary>
        /// <param name="chatId">Id of the chat for which return quiz.</param>
        /// <returns>Model of chat quiz.</returns>
        public ChatQuiz? GetActiveChatQuiz(long chatId)
        {
            var entityChatQuiz = this.dbContext.ChatQuizzes
                .Where(_ => _.ChatId == chatId && _.State == (byte)QuizState.IsActive)
                .OrderByDescending(_ => _.Created)
                .FirstOrDefault();
            this.dbContext.SaveChanges();
            return entityChatQuiz;
        }

        /// <summary>
        /// Register user answer on quiz.
        /// </summary>
        /// <param name="chatQuizId">Id of the chat quize where user give answer.</param>
        /// <param name="answerId">Id of the quiz answer.</param>
        public void RegisterQuizAnswer(
            int chatQuizId,
            int answerId)
        {
            var newChatQuizAnswer = new ChatQuizAnswer()
            {
                ChatQuizId = chatQuizId,
                AnswerId = answerId,
            };

            this.dbContext.ChatQuizAnswers.Add(newChatQuizAnswer);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Get quiz by id.
        /// </summary>
        /// <param name="quizId">Id of the requested quiz.</param>
        /// <returns>Quiz model if it found in database.</returns>
        public ObjectModels.Quiz? GetQuiz(int quizId)
        {
            var quizEntity = this.dbContext.Quizzes
                .Include(_ => _.QuizAnswerOptions)
                .Include(_ => _.QuizExamples)
                    .ThenInclude(_ => _.TranslationUnitUsage)
                .FirstOrDefault(_ => _.Id == quizId);
            var quizModel = this.mapper.Map<ObjectModels.Quiz?>(quizEntity);
            return quizModel;
        }

        /// <summary>
        /// Get new quiz for chat.
        /// </summary>
        /// <param name="chatId">Id of the chat which identify user.</param>
        /// <returns>Quiz model for user.</returns>
        public ObjectModels.Quiz? GetNewForChatActiveQuiz(long chatId)
        {
            var firstQuiz = this.dbContext.Quizzes
                .Include(_ => _.ChatQuizzes)
                .Include(_ => _.QuizAnswerOptions)
                .Include(_ => _.QuizExamples)
                    .ThenInclude(_ => _.TranslationUnitUsage)
                .FirstOrDefault(_ =>
                    _.ChatQuizzes.Where(_ => _.ChatId == chatId).Count() == 0
                 && _.IsActive);
            var firstQuizModel = this.mapper.Map<ObjectModels.Quiz?>(firstQuiz);
            return firstQuizModel;
        }

        /// <summary>
        /// Get oldest quiz id for user chat.
        /// </summary>
        /// <param name="chatId">Id of the chat which identify user.</param>
        /// <returns>Oldest for chat quiz id.</returns>
        public int? GetOldestActiveQuizId(long chatId)
        {
            var oldestChatQuiz = this.dbContext.ChatQuizzes
                .Include(_ => _.Quiz)
                .Where(_ => _.ChatId == chatId && _.Quiz.IsActive)
                .OrderByDescending(_ => _.Created)
                .FirstOrDefault();
            return oldestChatQuiz?.QuizId;
        }

        /// <summary>
        /// Get random quiz.
        /// </summary>
        /// <returns>Random quiz.</returns>
        public ObjectModels.Quiz? GetRandomActiveQuizId()
        {
            var activeQuizzes = this.dbContext.Quizzes.Where(_ => _.IsActive);
            var skip = (int)(new Random().NextDouble() * activeQuizzes.Count());
            var randomQuiz = activeQuizzes
                .Include(_ => _.ChatQuizzes)
                .Include(_ => _.QuizAnswerOptions)
                .Include(_ => _.QuizExamples)
                    .ThenInclude(_ => _.TranslationUnitUsage)
                .OrderBy(o => o.Id).Skip(skip).Take(1).FirstOrDefault();
            var randomQuizModel = this.mapper.Map<ObjectModels.Quiz?>(randomQuiz);
            return randomQuizModel;
        }

        /// <summary>
        /// Get quizzes by requested parameters.
        /// </summary>
        /// <param name="fromDate">Date from which quiz was created.</param>
        /// <param name="toDate">Date before which quiz was created.</param>
        /// <param name="isActive">Indicate that quiz is active.</param>
        /// <returns>Quiz model if it found in database.</returns>
        public IEnumerable<ObjectModels.Quiz> GetQuizzes(
            DateTimeOffset fromDate,
            DateTimeOffset toDate,
            bool? isActive)
        {
            var quizzesEntity = this.dbContext.Quizzes
                .Include(_ => _.TranslationUnitDefinition)
                    .ThenInclude(_ => _.TranslationUnit)
                .Include(_ => _.QuizAnswerOptions)
                .Include(_ => _.QuizExamples)
                    .ThenInclude(_ => _.TranslationUnitUsage)
                .Where(_ => _.Created >= fromDate && _.Created <= toDate);
            if (isActive != null)
            {
                quizzesEntity = quizzesEntity.Where(_ => _.IsActive == isActive.Value);
            }

            var quizzesModel = this.mapper.Map<IEnumerable<ObjectModels.Quiz>>(quizzesEntity);
            return quizzesModel;
        }
    }
}

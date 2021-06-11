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
        /// Get user by chat id.
        /// </summary>
        /// <param name="chatId">Id of the chat.</param>
        /// <returns>User model.</returns>
        public ObjectModels.User? GetUser(long chatId)
        {
            var user = this.dbContext.Users.FirstOrDefault(_ => _.ChatId == chatId);
            var userModel = this.mapper.Map<ObjectModels.User?>(user);
            return userModel;
        }

        /// <summary>
        /// Get user by user id.
        /// </summary>
        /// <param name="userId">Id of the user.</param>
        /// <returns>User model.</returns>
        public ObjectModels.User? GetUser(string userId)
        {
            var user = this.dbContext.Users.FirstOrDefault(_ => _.Id == userId);
            var userModel = this.mapper.Map<ObjectModels.User?>(user);
            return userModel;
        }

        /// <summary>
        /// Register new chat.
        /// </summary>
        /// <param name="chatId">Id of the new chat.</param>
        /// <returns>Model of created user.</returns>
        public ObjectModels.User RegisterNewChat(long chatId)
        {
            var newChat = new EntityFramework.User()
            {
                ChatId = chatId,
                Id = Guid.NewGuid().ToString(),
            };

            var entityUser = this.dbContext.Users.Add(newChat);
            this.dbContext.SaveChanges();
            var userModel = this.mapper.Map<ObjectModels.User>(entityUser.Entity);
            return userModel;
        }

        /// <summary>
        /// Add new quiz to user.
        /// </summary>
        /// <param name="userId">Id of the user for which quiz added.</param>
        /// <param name="quizId">Id of the quiz which will be added to user.</param>
        /// <param name="started">Date when quiz added to user.</param>
        /// <returns>Model of user quiz.</returns>
        public ObjectModels.UserQuiz AddQuizToUser(
            string userId,
            int quizId,
            DateTimeOffset started)
        {
            var newChatQuiz = new EntityFramework.UserQuiz()
            {
                UserId = userId,
                QuizId = quizId,
                State = (byte)QuizState.IsActive,
                Started = started,
            };

            var entityChatQuiz = this.dbContext.UserQuizzes.Add(newChatQuiz).Entity;
            this.dbContext.SaveChanges();
            var chatQuizModel = this.mapper.Map<ObjectModels.UserQuiz>(entityChatQuiz);
            return chatQuizModel;
        }

        /// <summary>
        /// Get active user quize.
        /// </summary>
        /// <param name="userId">Id of the user for which return quiz.</param>
        /// <returns>Model of chat quiz.</returns>
        public ObjectModels.UserQuiz? GetActiveUserQuiz(string userId)
        {
            var entityChatQuiz = this.dbContext.UserQuizzes
                .Where(_ => _.UserId == userId && _.State == (byte)QuizState.IsActive)
                .OrderByDescending(_ => _.Started)
                .FirstOrDefault();
            this.dbContext.SaveChanges();
            var chatQuizModel = this.mapper.Map<ObjectModels.UserQuiz?>(entityChatQuiz);
            return chatQuizModel;
        }

        /// <summary>
        /// Register user answer on quiz.
        /// </summary>
        /// <param name="userQuizId">Id of the user's quize where user give answer.</param>
        /// <param name="answerId">Id of the quiz answer.</param>
        public void RegisterQuizAnswer(
            int userQuizId,
            int answerId)
        {
            var newChatQuizAnswer = new EntityFramework.UserQuizAnswer()
            {
                UserQuizeId = userQuizId,
                AnswerId = answerId,
            };

            this.dbContext.UserQuizAnswers.Add(newChatQuizAnswer);
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
        /// Get new quiz for user.
        /// </summary>
        /// <param name="userId">Id which identify user.</param>
        /// <returns>Quiz model for user.</returns>
        public ObjectModels.Quiz? GetNewQuiz(string userId)
        {
            var firstQuiz = this.dbContext.Quizzes
                .Include(_ => _.UserQuizzes)
                .Include(_ => _.QuizAnswerOptions)
                .Include(_ => _.QuizExamples)
                    .ThenInclude(_ => _.TranslationUnitUsage)
                .FirstOrDefault(_ =>
                    !_.UserQuizzes.Where(_ => _.UserId == userId).Any()
                 && _.IsActive);
            var firstQuizModel = this.mapper.Map<ObjectModels.Quiz?>(firstQuiz);
            return firstQuizModel;
        }

        /// <summary>
        /// Get oldest quiz id for user.
        /// </summary>
        /// <param name="userId">Id which identify user.</param>
        /// <returns>Oldest for user quiz id.</returns>
        public int? GetOldestActiveQuizId(string userId)
        {
            var oldestChatQuiz = this.dbContext.UserQuizzes
                .Include(_ => _.Quiz)
                .Where(_ => _.UserId == userId && _.Quiz.IsActive)
                .OrderByDescending(_ => _.Started)
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
                .Include(_ => _.QuizAnswerOptions)
                .Include(_ => _.QuizExamples)
                    .ThenInclude(_ => _.TranslationUnitUsage)
                .OrderBy(o => o.Id).Skip(skip).Take(1).FirstOrDefault();
            var randomQuizModel = this.mapper.Map<ObjectModels.Quiz?>(randomQuiz);
            return randomQuizModel;
        }

        /// <summary>
        /// Create new quiz.
        /// </summary>
        /// <param name="question">Text of quiz question.</param>
        /// <param name="questionType">Type of question.</param>
        /// <param name="translationUnitDefinitionId">Id of translation unit related to quiz.</param>
        /// <param name="questionShowType">Question show type.</param>
        /// <param name="exampleShowType">Example show type.</param>
        /// <param name="answerShowType">Answer show type.</param>
        /// <param name="created">Date of quiz creating.</param>
        /// <param name="isActive">Indicate that new quiz will be active or not.</param>
        /// <returns>Id of created quiz.</returns>
        public int CreateQuiz(
            string question,
            QuestionType questionType,
            int translationUnitDefinitionId,
            ShowType questionShowType,
            ShowType exampleShowType,
            ShowType answerShowType,
            DateTimeOffset created,
            bool isActive)
        {
            var newQuiz = new EntityFramework.Quiz()
            {
                Question = question,
                QuestionType = (byte)questionType,
                TranslationUnitDefinitionId = translationUnitDefinitionId,
                QuestionShowType = (byte)questionShowType,
                ExampleShowType = (byte)exampleShowType,
                AnswerShowType = (byte)answerShowType,
                Created = created,
                IsActive = isActive,
            };
            var quizeEntity = this.dbContext.Quizzes.Add(newQuiz);
            this.dbContext.SaveChanges();
            return quizeEntity.Entity.Id;
        }

        /// <summary>
        /// Add new answer options to quiz.
        /// </summary>
        /// <param name="quizId">Id of related to answer quiz.</param>
        /// <param name="text">Text of answer.</param>
        /// <param name="isRightAnswer">Indicate that answer is right or not.</param>
        /// <returns>Id of added answer option.</returns>
        public int AddAnswerOptionToQuiz(
            int quizId,
            string text,
            bool isRightAnswer)
        {
            var newAnswerOption = new EntityFramework.QuizAnswerOption()
            {
                IsRightAnswer = isRightAnswer,
                Text = text,
                QuizId = quizId,
            };
            var answerOptionEntity = this.dbContext.QuizAnswerOptions.Add(newAnswerOption);
            this.dbContext.SaveChanges();
            return answerOptionEntity.Entity.Id;
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

// -----------------------------------------------------------------------
// <copyright file="QuizManager.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DjayEnglish.Server.Core.EntityFrameworkCore;
    using DjayEnglish.Server.ObjectModels;

    /// <summary>
    /// Manager which work with quiz.
    /// </summary>
    public class QuizManager
    {
        private readonly QuizManagerEvents quizManagerEvents;
        private readonly DbQuizPersistence dbQuizPersistence;
        private readonly DbTranslationUnitPersistence dbTranslationUnitPersistence;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuizManager"/> class.
        /// </summary>
        /// <param name="quizManagerEvents">Global class with quiz manager events.</param>
        /// <param name="dbQuizPersistence">Database context for quizzes representation.</param>
        /// <param name="dbTranslationUnitPersistence">Database context for translation units representation.</param>
        public QuizManager(
            QuizManagerEvents quizManagerEvents,
            DbQuizPersistence dbQuizPersistence,
            DbTranslationUnitPersistence dbTranslationUnitPersistence)
        {
            this.quizManagerEvents = quizManagerEvents ?? throw new ArgumentNullException(nameof(quizManagerEvents));
            this.dbQuizPersistence = dbQuizPersistence ?? throw new ArgumentNullException(nameof(dbQuizPersistence));
            this.dbTranslationUnitPersistence = dbTranslationUnitPersistence ?? throw new ArgumentNullException(nameof(dbTranslationUnitPersistence));
        }

        /// <summary>
        /// Get quizzes without audio files.
        /// </summary>
        /// <returns>Quiz collection which does not have audio.</returns>
        public IEnumerable<Quiz> GetQuizzesWithoutAudio()
        {
            var result = this.dbQuizPersistence.GetQuizzesWithoutAudio();
            return result;
        }

        /// <summary>
        /// Set quiz audio enable.
        /// </summary>
        /// /<param name="quizId">Id of quiz for which set enable.</param>
        /// /<param name="hasAudio">Indicate that quiz has audio.</param>
        public void SetQuizAudioEnable(int quizId, bool hasAudio)
        {
            this.dbQuizPersistence.SetQuizAudioEnable(quizId, hasAudio);
        }

        /// <summary>
        /// Generate new quiz for translation unit.
        /// </summary>
        /// <param name="trasnlationUnitId">Id of the translation unit.</param>
        /// <param name="answerOptionsCount">Count of available for user answer options in quiz.</param>
        /// <param name="createdAt">Time when new quiz created.</param>
        public void GenerateQuiz(
            int trasnlationUnitId,
            int answerOptionsCount,
            DateTimeOffset createdAt)
        {
            var translationUnit = this.dbTranslationUnitPersistence
                .GetTranslationUnit(trasnlationUnitId);
            if (translationUnit == null)
            {
                throw new InvalidOperationException(
                    $"Translation unit with id {trasnlationUnitId} does not existed.");
            }

            var definitionAnswerOptions = this.GetAntonymsDefinitions(translationUnit);

            var neededAnswerOptionsCount = (answerOptionsCount - 1) * translationUnit.Definitions.Count();
            if (definitionAnswerOptions.Count() < neededAnswerOptionsCount)
            {
                var remainder = neededAnswerOptionsCount - definitionAnswerOptions.Count();
                var wordLength = translationUnit.Spelling.Split(" ").First().Length;
                var patternLength = wordLength / 4;
                if (patternLength < 3)
                {
                    patternLength = 3;
                }

                var otherAnswerOptions = this.dbTranslationUnitPersistence.GetTranslationUnits(
                    translationUnit.Language,
                    translationUnit.PartOfSpeech,
                    definitionAnswerOptions.Select(_ => _.TranslationUnitId).ToArray(),
                    remainder,
                    translationUnit.Spelling.Substring(0, patternLength) + "%");
                var otherAnswerOptionsDefinitions = otherAnswerOptions
                    .SelectMany(_ => _.Definitions)
                    .ToList();

                definitionAnswerOptions = definitionAnswerOptions.Union(otherAnswerOptionsDefinitions).ToList();
            }

            var random = new Random();
            definitionAnswerOptions = definitionAnswerOptions
                .OrderBy(x => random.Next())
                .ToList();

            if (definitionAnswerOptions == null)
            {
                throw new InvalidOperationException(
                    $"Definitions couldn't be null for translation unit {trasnlationUnitId}");
            }

            foreach (var definition in translationUnit.Definitions)
            {
                var usage = this.dbTranslationUnitPersistence.GetTranslationUnitUsage(
                    definition.Id,
                    isActive: true);
                var showUsage = usage.Any() ? ShowType.Audio : ShowType.None;

                var question = $"What does {translationUnit.Spelling} mean.";

                var quizId = this.dbQuizPersistence.CreateQuiz(
                    question,
                    QuestionType.TranslationUnitDefinition,
                    definition.Id,
                    ShowType.Audio,
                    ShowType.Audio,
                    showUsage,
                    createdAt,
                    isActive: false);
                var quiz = this.dbQuizPersistence.GetQuiz(quizId);

                this.dbQuizPersistence.AddAnswerOptionToQuiz(
                    quizId,
                    definition.Definition,
                    isRightAnswer: true);

                var options = definitionAnswerOptions.GetRange(0, answerOptionsCount - 1);
                foreach (var answerOption in options)
                {
                    this.dbQuizPersistence.AddAnswerOptionToQuiz(
                        quizId,
                        answerOption.Definition,
                        isRightAnswer: false);
                }

                foreach (var translationUnitUsage in usage)
                {
                    this.dbQuizPersistence.AddUsageToQuiz(
                        quizId,
                        translationUnitUsage.Example,
                        translationUnitUsage.Id);
                }
            }
        }

        /// <summary>
        /// Start new quiz for user.
        /// </summary>
        /// <param name="chatId">Id of the chat.</param>
        /// <param name="startedAt">Time when new quiz started.</param>
        /// <returns>New quiz for user.</returns>
        public Quiz StartQuiz(long chatId, DateTimeOffset startedAt)
        {
            var user = this.dbQuizPersistence.GetUser(chatId);
            if (user == null)
            {
                user = this.dbQuizPersistence.RegisterNewChat(chatId);
            }

            var startedQuize = this.GetNextQuizForUser(user.Id);
            this.dbQuizPersistence.AddQuizToUser(user.Id, startedQuize.Id, startedAt);
            this.quizManagerEvents.NotifyQuizStarted(chatId, startedQuize);
            return startedQuize;
        }

        /// <summary>
        /// Register user answer.
        /// </summary>
        /// <param name="chatId">Chat Id.</param>
        /// <param name="userAnswerkey">User answer key.</param>
        /// <param name="answerDate">Date time when user answer on quiz.</param>
        public void RegisterUserAnswer(
            long chatId,
            string userAnswerkey,
            DateTimeOffset answerDate)
        {
            var user = this.dbQuizPersistence.GetUser(chatId);
            if (user == null)
            {
                throw new InvalidOperationException(
                    $"Chat {chatId} doesn't have user.");
            }

            var userQuize = this.dbQuizPersistence.GetActiveUserQuiz(user.Id);
            if (userQuize == null)
            {
                throw new InvalidOperationException(
                    $"Chat {chatId} doesn't have active quize. User answer could'nt be registered.");
            }

            var quiz = this.GetQuiz(userQuize.QuizId);
            if (quiz == null)
            {
                throw new InvalidOperationException(
                    $"User answer couldn't be registered in chat {chatId} because quize {userQuize.QuizId} is null.");
            }

            var answerOption = quiz.GetAnswerOption(userAnswerkey);
            if (answerOption == null)
            {
                return;
            }

            this.dbQuizPersistence.RegisterQuizAnswer(userQuize.Id, answerOption.Id);
            this.quizManagerEvents.NotifyUserAnswerResultRecived(chatId, answerOption.IsRightAnswer);

            if (answerOption.IsRightAnswer)
            {
                this.StartQuiz(chatId, answerDate);
            }
        }

        /// <summary>
        /// Get next quiz for user.
        /// </summary>
        /// <param name="userId">Id which identify user.</param>
        /// <returns>Quiz model for user.</returns>
        public Quiz GetNextQuizForUser(string userId)
        {
            var user = this.dbQuizPersistence.GetUser(userId);
            if (user == null)
            {
                throw new InvalidOperationException(
                    $"User {userId} doesn't exist.");
            }

            var newForChatQuiz = this.dbQuizPersistence.GetNewQuiz(user.Id);
            if (newForChatQuiz != null)
            {
                return newForChatQuiz;
            }

            var oldestQuizId = this.dbQuizPersistence.GetOldestActiveQuizId(user.Id);
            if (oldestQuizId != null)
            {
                var oldestQuize = this.GetQuiz(oldestQuizId.Value);
                if (oldestQuize == null)
                {
                    throw new InvalidOperationException(
                        $"Oldest quiz model with id {oldestQuizId.Value} couldn't be null for user {userId}");
                }
            }

            var randomQuize = this.dbQuizPersistence.GetRandomActiveQuizId();
            if (randomQuize == null)
            {
                throw new InvalidOperationException(
                        $"Random quiz couldn't be null for {userId}");
            }

            return randomQuize;
        }

        /// <summary>
        /// Get quiz by id.
        /// </summary>
        /// <param name="quizId">Id of the requested quiz.</param>
        /// <returns>Quiz model if it found.</returns>
        public Quiz? GetQuiz(int quizId)
        {
            return this.dbQuizPersistence.GetQuiz(quizId);
        }

        /// <summary>
        /// Get quizzes by requested parameters.
        /// </summary>
        /// <param name="fromDate">Date from which quiz was created.</param>
        /// <param name="toDate">Date before which quiz was created.</param>
        /// <param name="isActive">Indicate that quiz is active.</param>
        /// <returns>Collection of Quiz modeles from database.</returns>
        public IEnumerable<Quiz> GetQuizzes(
            DateTimeOffset fromDate,
            DateTimeOffset toDate,
            bool? isActive)
        {
            return this.dbQuizPersistence.GetQuizzes(
                fromDate,
                toDate,
                isActive);
        }

        private List<TranslationUnitDefinition> GetAntonymsDefinitions(
            TranslationUnit translationUnit)
        {
            var antonymsIds = translationUnit.Antonyms
                .Select(_ => _.AntonymTranslationUnitId)
                .Distinct()
                .ToArray();
            var antonyms = this.dbTranslationUnitPersistence
                .GetTranslationUnits(antonymsIds);
            var antonymSynonymsIds = antonyms
                .SelectMany(_ => _.Synonyms.Select(s => s.SynonymTranslationUnitId).ToArray())
                .Distinct()
                .ToArray();
            var antonymWithAntotymSynonymsIds = antonymsIds
                .Union(antonymSynonymsIds)
                .Distinct()
                .ToArray();
            var antonymAnswerOptions = this.dbTranslationUnitPersistence
                .GetTranslationUnits(antonymWithAntotymSynonymsIds);
            var definitionAnswerOptions = antonymAnswerOptions
                .SelectMany(_ => _.Definitions)
                .Distinct()
                .ToList();

            return definitionAnswerOptions;
        }
    }
}

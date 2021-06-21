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
        /// Create quiz using <see cref="QuizCandidate"/> class.
        /// </summary>
        /// <param name="quizCandidate">Class which contain all information about creating quiz.</param>
        public void CreateQuiz(QuizCandidate quizCandidate)
        {
            this.dbQuizPersistence.CreateQuiz(quizCandidate);
        }

        /// <summary>
        /// Generate new quiz candidate for translation unit definition.
        /// </summary>
        /// <param name="trasnlationUnitDefinitionId">Id of the translation unit definition for which quiz genereting.</param>
        /// <param name="answerOptionsCandidatesCount">Count of available for user answer options candidates in quiz.</param>
        /// <param name="maxAnswerOptionLength">Max length of answer option.</param>
        /// <param name="emptyAnswerOptionCount">Empty answer option count for manual editing.</param>
        /// <param name="emptyExampleOptionCount">Empty example count for manual editing.</param>
        /// <param name="createdAt">Time when new quiz created.</param>
        /// <returns>Generated Quiz Candidate model without saving in database.</returns>
        public QuizCandidate GenerateQuizCandidate(
            int trasnlationUnitDefinitionId,
            int answerOptionsCandidatesCount,
            int maxAnswerOptionLength,
            int emptyAnswerOptionCount,
            int emptyExampleOptionCount,
            DateTimeOffset createdAt)
        {
            var definition = this.dbTranslationUnitPersistence
                .GetTranslationUnitDefinition(trasnlationUnitDefinitionId);
            if (definition == null)
            {
                throw new InvalidOperationException(
                    $"Translation unit definition with id {trasnlationUnitDefinitionId} does not existed.");
            }

            var translationUnit = this.dbTranslationUnitPersistence
                .GetTranslationUnit(definition.TranslationUnitId);
            if (translationUnit == null)
            {
                throw new InvalidOperationException(
                    $"Translation unit with id {trasnlationUnitDefinitionId} does not existed.");
            }

            if (definition.Definition.Length >= maxAnswerOptionLength)
            {
                throw new InvalidOperationException(
                    $"Translation unit definition {trasnlationUnitDefinitionId} has too long definition {definition.Definition.Length}. Max length should be {maxAnswerOptionLength}");
            }

            var definitionAnswerOptions = this.GetAntonymsDefinitions(
                translationUnit,
                maxDefinitionsOnOneUnit: 2)
                .Where(_ => _.Definition.Length < maxAnswerOptionLength)
                .Take(answerOptionsCandidatesCount)
                .ToList();

            var answerOptions = new List<QuizAnswerOptionCandidate>();
            answerOptions.Add(new QuizAnswerOptionCandidate()
            {
                IsRightAnswer = true,
                IsAntonym = false,
                IsInclude = true,
                Text = definition.Definition,
                SourceTranslationUnit = translationUnit.Spelling,
            });
            foreach (var answerOption in definitionAnswerOptions)
            {
                answerOptions.Add(new QuizAnswerOptionCandidate()
                {
                    IsRightAnswer = false,
                    IsAntonym = true,
                    Text = answerOption.Definition,
                    SourceTranslationUnit = answerOption.TranslationUnit?.Spelling,
                });
            }

            if (definitionAnswerOptions.Count() < answerOptionsCandidatesCount)
            {
                var remainder = answerOptionsCandidatesCount - definitionAnswerOptions.Count();
                var wordLength = translationUnit.Spelling.Split(" ").First().Length;
                var patternLength = wordLength / 4;
                if (patternLength < 3)
                {
                    patternLength = 3;
                }

                var definitionAnswerOptionsIds = definitionAnswerOptions
                    .Select(_ => _.TranslationUnit?.Spelling ?? throw new ArgumentNullException($"Definition {_.Id} has to have spelling!"))
                    .ToList();
                definitionAnswerOptionsIds.Add(translationUnit.Spelling);
                var excludedIds = definitionAnswerOptionsIds.ToArray();
                var otherAnswerOptions = this.dbTranslationUnitPersistence.GetTranslationUnits(
                    translationUnit.Language,
                    translationUnit.PartOfSpeech,
                    excludedIds,
                    remainder,
                    maxAnswerOptionLength,
                    translationUnit.Spelling.Substring(0, patternLength));
                var otherAnswerOptionsDefinitions = otherAnswerOptions
                    .SelectMany(_ => _.Definitions)
                    .Where(_ => _.Definition.Length < maxAnswerOptionLength)
                    .Take(remainder)
                    .ToList();

                foreach (var answerOption in otherAnswerOptionsDefinitions)
                {
                    answerOptions.Add(new QuizAnswerOptionCandidate()
                    {
                        IsRightAnswer = false,
                        IsAntonym = false,
                        Text = answerOption.Definition,
                        SourceTranslationUnit = answerOption.TranslationUnit?.Spelling,
                    });
                }
            }

            var random = new Random();
            answerOptions = answerOptions
                .OrderBy(x => random.Next())
                .OrderByDescending(_ => _.IsRightAnswer)
                .Take(answerOptionsCandidatesCount)
                .ToList();
            for (int i = 0; i < emptyAnswerOptionCount; i++)
            {
                answerOptions.Add(new QuizAnswerOptionCandidate());
            }

            if (definitionAnswerOptions == null)
            {
                throw new InvalidOperationException(
                    $"Definitions couldn't be null for translation unit definition {trasnlationUnitDefinitionId}");
            }

            var usage = this.dbTranslationUnitPersistence.GetTranslationUnitUsage(
                definition.Id,
                isActive: true);
            var showUsage = usage.Any() ? ShowType.Audio : ShowType.None;

            var question = $"What does {translationUnit.Spelling} mean.";

            var usages = new List<QuizExample>();
            foreach (var translationUnitUsage in usage)
            {
                usages.Add(new QuizExample()
                {
                    Text = translationUnitUsage.Example,
                    TranslationUnitUsageId = translationUnitUsage.Id,
                });
            }

            for (int i = 0; i < emptyExampleOptionCount; i++)
            {
                usages.Add(new QuizExample());
            }

            var newQuize = new QuizCandidate()
            {
                Question = question,
                QuestionType = QuestionType.TranslationUnitDefinition,
                QuestionShowType = ShowType.Audio,
                AnswerShowType = ShowType.Audio,
                ExampleShowType = showUsage,
                IsActive = false,
                Created = createdAt,
                TranslationUnitDefinitionId = trasnlationUnitDefinitionId,
                TranslationUnitDefinition = definition,
                QuizExamples = usages,
                QuizAnswerOptions = answerOptions,
                CanEditRigthAnswer = false,
            };

            return newQuize;
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

        private IEnumerable<TranslationUnitDefinition> GetAntonymsDefinitions(
            TranslationUnit translationUnit,
            int maxDefinitionsOnOneUnit)
        {
            var antonymsIds = translationUnit.Antonyms
                .Select(_ => _.AntonymTranslationUnitId)
                .Distinct()
                .ToArray();
            var antonyms = this.dbTranslationUnitPersistence
                .GetTranslationUnits(antonymsIds);
            var antonymSynonymsIds = antonyms
                .SelectMany(_ => _.Synonyms.Select(s => s.SynonymTranslationUnitId).ToArray())
                .Where(_ => _ != translationUnit.Id)
                .Distinct()
                .ToArray();
            var antonymWithAntonymSynonymsIds = antonymsIds
                .Union(antonymSynonymsIds)
                .Distinct()
                .ToArray();
            var antonymAnswerOptions = this.dbTranslationUnitPersistence
                .GetTranslationUnits(antonymWithAntonymSynonymsIds);
            var definitionAnswerOptions = antonymAnswerOptions
                .SelectMany(_ => _.Definitions.Take(maxDefinitionsOnOneUnit))
                .Distinct();

            return definitionAnswerOptions;
        }
    }
}

// -----------------------------------------------------------------------
// <copyright file="Quiz.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Object model which contain information about quiz.
    /// </summary>
    public class Quiz
    {
        /// <summary>
        /// Gets or sets id of quiz.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets id of word definition which used in quiz.
        /// </summary>
        public int WordDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether quiz is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a date when quiz was created.
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Gets or sets a date when quiz was closed.
        /// </summary>
        public DateTimeOffset? Closed { get; set; }

        /// <summary>
        /// Gets or sets text of question.
        /// </summary>
        public string Question { get; set; } = null!;

        /// <summary>
        /// Gets or sets show type of question.
        /// </summary>
        public ShowType QuestionShowType { get; set; }

        /// <summary>
        /// Gets or sets show type of examples and usage.
        /// </summary>
        public ShowType ExampleShowType { get; set; }

        /// <summary>
        /// Gets or sets show type of answers.
        /// </summary>
        public ShowType AnswerShowType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether quiz has audio files.
        /// </summary>
        public bool HasAudioFiles { get; set; }

        /// <summary>
        /// Gets answer options.
        /// </summary>
        public string[] Examples => this.QuizExamples.Select(_ => _.WordUsage.Example).ToArray();

        /// <summary>
        /// Gets or sets answer options for quiz.
        /// </summary>
        public IEnumerable<QuizAnswerOption> QuizAnswerOptions { get; set; } = null!;

        /// <summary>
        /// Gets or sets using and examples for quiz.
        /// </summary>
        public IEnumerable<QuizExample> QuizExamples { get; set; } = null!;

        /// <summary>
        /// Get full quiz text.
        /// </summary>
        /// <returns>Full quiz text.</returns>
        public string GetQuizText()
        {
            var result = $"{this.Question}.";
            return result;
        }

        /// <summary>
        /// Get full answer options text.
        /// </summary>
        /// <returns>Full answer options text.</returns>
        public string GetAnswerOptionsText()
        {
            var result = string.Empty;
            var index = 1;
            foreach (var answerOption in this.QuizAnswerOptions)
            {
                result += $"{index}. {answerOption.Text}. \r\n";
                index++;
            }

            return result;
        }

        /// <summary>
        /// Get examples text.
        /// </summary>
        /// <returns>Examples text.</returns>
        public string GetExamplesText()
        {
            var result = string.Empty;
            if (this.Examples.Length > 0)
            {
                result += "Example: \r\n";
            }

            foreach (var example in this.Examples)
            {
                result += $"{example} \r\n";
            }

            return result;
        }

        /// <summary>
        /// Get showed to user quiz answer options.
        /// </summary>
        /// <returns>Collection of showed answer options.</returns>
        public string[] GetShowedQuizAnswerOptions()
        {
            return Enumerable.Range(1, this.QuizAnswerOptions.Count()).Select(_ => _.ToString()).ToArray();
        }

        /// <summary>
        /// Get answer option by user answer key.
        /// </summary>
        /// <param name="userAnswerKey">User answer key.</param>
        /// <returns>Quiz answer option if key is correct.</returns>
        public QuizAnswerOption? GetAnswerOption(string userAnswerKey)
        {
            if (!int.TryParse(userAnswerKey, out var answerKey))
            {
                return null;
            }

            if (answerKey < 1 && answerKey > this.QuizAnswerOptions.Count())
            {
                return null;
            }

            var answerOption = this.QuizAnswerOptions.ElementAt(answerKey - 1);
            if (answerOption == null)
            {
                throw new InvalidOperationException($"Answer option couldn't be null. Quiz id {this.Id}, UserAnswerKey {userAnswerKey}");
            }

            return answerOption;
        }
    }
}

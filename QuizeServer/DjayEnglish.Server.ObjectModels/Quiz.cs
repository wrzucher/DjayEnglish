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
        /// Gets answer options.
        /// </summary>
        public string[] Examples => this.QuizeExamples.Select(_ => _.WordUsage.Example).ToArray();

        /// <summary>
        /// Gets or sets answer options for quiz.
        /// </summary>
        public IEnumerable<QuizAnswerOption> QuizeAnswerOptions { get; set; } = null!;

        /// <summary>
        /// Gets or sets using and examples for quiz.
        /// </summary>
        public IEnumerable<QuizExample> QuizeExamples { get; set; } = null!;

        /// <summary>
        /// Get full quiz text.
        /// </summary>
        /// <returns>Full quiz text.</returns>
        public string GetQuizeText()
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
            foreach (var answerOption in this.QuizeAnswerOptions)
            {
                result += $"{index}: {answerOption.Text} \r\n";
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
        public string[] GetShowedQuizeAnswerOptions()
        {
            return Enumerable.Range(1, this.QuizeAnswerOptions.Count()).Select(_ => _.ToString()).ToArray();
        }

        /// <summary>
        /// Indicate that user answer is right.
        /// </summary>
        /// <param name="userAnswerKey">User answer key.</param>
        /// <returns>Result of user answer.</returns>
        public bool IsRightAnswer(string userAnswerKey)
        {
            if (!int.TryParse(userAnswerKey, out var answerKey))
            {
                return false;
            }

            if (answerKey < 1 && answerKey > this.QuizeAnswerOptions.Count())
            {
                return false;
            }

            var answerOption = this.QuizeAnswerOptions.ElementAt(answerKey - 1);
            if (answerOption == null)
            {
                throw new InvalidOperationException($"Answer option couldn't be null. Quizeid {this.Id}, UserAnswerKey {userAnswerKey}");
            }

            return answerOption.IsRightAnswer;
        }
    }
}

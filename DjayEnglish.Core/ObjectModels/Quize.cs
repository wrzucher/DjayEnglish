// -----------------------------------------------------------------------
// <copyright file="Quize.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Core.ObjectModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Object model which contain information about quize.
    /// </summary>
    public class Quize
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Quize"/> class.
        /// </summary>
        /// <param name="id">Id of quize.</param>
        /// <param name="question">Text of question.</param>
        /// <param name="examples">Examples which will be used in question.</param>
        /// <param name="answerOptions">Answer options.</param>
        public Quize(
            int id,
            string question,
            string[] examples,
            Dictionary<string, AnswerOption> answerOptions)
        {
            this.Id = id;
            this.Question = question;
            this.Examples = examples;
            this.AnswerOptions = answerOptions;
        }

        /// <summary>
        /// Gets id of quize.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets text of question.
        /// </summary>
        public string Question { get; }

        /// <summary>
        /// Gets answer options.
        /// </summary>
        public string[] Examples { get; }

        /// <summary>
        /// Gets answer options.
        /// </summary>
        public Dictionary<string, AnswerOption> AnswerOptions { get; }

        /// <summary>
        /// Get full quize text.
        /// </summary>
        /// <returns>Full quize text.</returns>
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
            foreach (var answerOption in this.AnswerOptions)
            {
                result += $"{answerOption.Key}: {answerOption.Value} \r\n";
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
        /// Get showed to user quize answer options.
        /// </summary>
        /// <returns>Collection of showed answer options.</returns>
        public string[] GetShowedQuizeAnswerOptions()
        {
            return this.AnswerOptions.Keys.ToArray();
        }

        /// <summary>
        /// Indicate that user answer is right.
        /// </summary>
        /// <param name="userAnswerKey">User answer key.</param>
        /// <returns>Result of user answer.</returns>
        public bool IsRightAnswer(string userAnswerKey)
        {
            var answerExist = this.AnswerOptions.TryGetValue(userAnswerKey, out var answerOption);
            if (!answerExist)
            {
                return false;
            }

            if (answerOption == null)
            {
                throw new InvalidOperationException($"Answer option couldn't be null. Quizeid {this.Id}, UserAnswerKey {userAnswerKey}");
            }

            return answerOption.IsRight;
        }
    }
}

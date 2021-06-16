// -----------------------------------------------------------------------
// <copyright file="QuizCandidate.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Administration.Models
{
    using System;
    using System.Collections.Generic;
    using DjayEnglish.Server.ObjectModels;

    /// <summary>
    /// Object model which contain information about quiz candidate.
    /// </summary>
    public class QuizCandidate
    {
        /// <summary>
        /// Gets or sets id of translation unit definition which used in quiz.
        /// </summary>
        public int? TranslationUnitDefinitionId { get; set; }

        /// <summary>
        /// Gets or sets id of translation unit definition which used in quiz.
        /// </summary>
        public TranslationUnitDefinition? TranslationUnitDefinition { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether quiz is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets a date when quiz was created.
        /// </summary>
        public DateTimeOffset Created { get; set; }

        /// <summary>
        /// Gets or sets text of question.
        /// </summary>
        public string Question { get; set; } = null!;

        /// <summary>
        /// Gets or sets type of question.
        /// </summary>
        public QuestionType QuestionType { get; set; }

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
        /// Gets or sets answer options for candidate quiz.
        /// </summary>
        public QuizAnswerOptionCandidate[] QuizAnswerOptions { get; set; } = null!;

        /// <summary>
        /// Gets or sets using and examples for candidates quiz.
        /// </summary>
        public QuizExampleCandidate[] QuizExamples { get; set; } = null!;
    }
}

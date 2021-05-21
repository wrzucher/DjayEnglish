﻿// -----------------------------------------------------------------------
// <copyright file="QuizManagerEvents.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.Core
{
    using System;
    using DjayEnglish.Server.ObjectModels;

    /// <summary>
    /// Global events generated by quiz managers.
    /// </summary>
    public class QuizManagerEvents
    {
        /// <summary>
        /// Notify all listeners subscribed that quiz started.
        /// </summary>
        public event EventHandler<OnQuizStartedEventArgs>? OnQuizStarted;

        /// <summary>
        /// Notify all listeners subscribed that user answer result recived.
        /// </summary>
        public event EventHandler<OnUserAnswerResultEventArgs>? OnUserAnswerResultRecived;

        /// <summary>
        /// Notify that quiz started.
        /// </summary>
        /// <param name="chatId">Id of the chat where quiz started.</param>
        /// <param name="quiz">Quiz model which was started.</param>
        public void NotifyQuizStarted(long chatId, Quiz quiz)
        {
            this.OnQuizStarted?.Invoke(this, new OnQuizStartedEventArgs(chatId, quiz));
        }

        /// <summary>
        /// Notify that user answer result was recived.
        /// </summary>
        /// <param name="chatId">Id of the chat where user answer on quiz.</param>
        /// <param name="isAnswerRight">Indicate that user answer was right.</param>
        public void NotifyUserAnswerResultRecived(long chatId, bool isAnswerRight)
        {
            this.OnUserAnswerResultRecived?.Invoke(this, new OnUserAnswerResultEventArgs(chatId, isAnswerRight));
        }
    }
}
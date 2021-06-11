// -----------------------------------------------------------------------
// <copyright file="User.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    /// <summary>
    /// Object model which contain information about user.
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Gets or sets user id.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets id og the chat in the Telegram chanel.
        /// </summary>
        public long? ChatId { get; set; }
    }
}

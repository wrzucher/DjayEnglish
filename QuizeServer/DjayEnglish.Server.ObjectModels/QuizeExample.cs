// -----------------------------------------------------------------------
// <copyright file="QuizeExample.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.ObjectModels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Object model which contain infromation about using and examples for quize.
    /// </summary>
    public partial class QuizeExample
    {
        /// <summary>
        /// Gets id of quize example.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets id of quize to which example related.
        /// </summary>
        public int QuizeId { get; }

        /// <summary>
        /// Gets id of word example.
        /// </summary>
        public int WordExampleId { get; }
    }
}

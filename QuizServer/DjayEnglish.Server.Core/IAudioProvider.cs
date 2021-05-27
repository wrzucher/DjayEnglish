// -----------------------------------------------------------------------
// <copyright file="IAudioProvider.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.Core
{
    using System.IO;
    using System.Threading.Tasks;
    using DjayEnglish.Server.ObjectModels;

    /// <summary>
    /// Interface which provide audio information.
    /// </summary>
    public interface IAudioProvider
    {
        /// <summary>
        /// Get audio for quize part.
        /// </summary>
        /// <param name="quizeId">Quize id for which should be getting audio.</param>
        /// <param name="quizePartType">Quize part type for which should be getting audio.</param>
        /// <returns>A <see cref="MemoryStream"/> which contain audio file.</returns>
        Task<MemoryStream> GetAudio(int quizeId, QuizePartType quizePartType);
    }
}

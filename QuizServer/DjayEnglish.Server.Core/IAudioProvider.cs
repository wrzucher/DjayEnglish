// -----------------------------------------------------------------------
// <copyright file="IAudioProvider.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.Server.Core
{
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface which provide audio information.
    /// </summary>
    public interface IAudioProvider
    {
        /// <summary>
        /// Get audio using test.
        /// </summary>
        /// <param name="text">Test which should be getting in audio format.</param>
        /// <returns>A <see cref="MemoryStream"/> which contain audio file.</returns>
        Task<MemoryStream> GetAudio(string text);
    }
}

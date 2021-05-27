// -----------------------------------------------------------------------
// <copyright file="LocalAudioProvider.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.App
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using DjayEnglish.Server.Core;
    using DjayEnglish.Server.ObjectModels;

    /// <summary>
    /// Audio provider which used remote service to provide audio information.
    /// </summary>
    public class LocalAudioProvider : IAudioProvider
    {
        /// <inheritdoc/>
        public async Task<MemoryStream> GetAudio(int quizeId, QuizePartType quizePartType)
        {
            using var fileStream = new FileStream($"media/{quizeId}/{quizePartType}.wav", FileMode.Open, FileAccess.Read);
            var result = new MemoryStream();
            await fileStream.CopyToAsync(result).ConfigureAwait(false);
            result.Position = 0;
            return result;
        }
    }
}

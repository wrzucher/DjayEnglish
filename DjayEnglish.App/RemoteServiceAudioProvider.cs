// -----------------------------------------------------------------------
// <copyright file="RemoteServiceAudioProvider.cs" company="DjayEnglish">
// Copyright (c) DjayEnglish. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DjayEnglish.App
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Audio provider which used remote service to provide audio information.
    /// </summary>
    public class RemoteServiceAudioProvider : IAudioProvider
    {
        /// <inheritdoc/>
        public async Task<MemoryStream> GetAudio(string text)
        {
            var uri = new Uri("http://localhost:5002/api/tts");
            var client = new HttpClient();
            StringContent httpContent = new StringContent(text, System.Text.Encoding.UTF8, "text/plain");
            var response = await client.PostAsync(uri, httpContent).ConfigureAwait(false);
            var result = new MemoryStream();
            await response.Content.CopyToAsync(result).ConfigureAwait(false);
            result.Position = 0;
            return result;
        }
    }
}

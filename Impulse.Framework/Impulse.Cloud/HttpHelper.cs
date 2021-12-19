// <copyright file="HttpHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Impulse.Cloud
{
    public class HttpHelper
    {
        public static async Task<string> Get(string uriString)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "RD web client");
                client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(uriString);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ArgumentException($"Http GET request failure: {response.ReasonPhrase}");
                }

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}

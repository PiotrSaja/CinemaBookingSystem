﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces;

namespace CinemaBookingSystem.Infrastructure.ExternalAPI.OMDB
{
    public partial class OmdbClient : IOmdbClient
    {
        private string _baseUrl = "http://www.omdbapi.com";
        private readonly HttpClient _httpClient;
        private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

        #region OmdbClient()
        public OmdbClient(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("OmdbClient");
            _baseUrl = _httpClient.BaseAddress.ToString();
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(() =>
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings();
                UpdateJsonSerializerSettings(settings);
                return settings;
            });
        }
        #endregion

        #region JsonSerializerSettings()
        protected Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings { get { return _settings.Value; } }
        #endregion

        #region UpdateJsonSerializerSettings()
        partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings);
        #endregion

        #region BaseUrl()
        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }
        #endregion

        #region GetMovie()
        public async Task<string> GetMovie(string searchFilter, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("?apikey=305be6dd");
            urlBuilder.Append("&t=").Append(searchFilter);
            var client = _httpClient;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return responseData;
                    }
                    else
                    {
                        return "Bad request";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region GetMovieById()
        public async Task<string> GetMovieById(string id, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("?apikey=305be6dd");
            urlBuilder.Append("&i=").Append(id);
            urlBuilder.Append("&plot=full");
            var client = _httpClient;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);
                    var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = response.Content == null ? null : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return responseData;
                    }
                    else
                    {
                        return "Bad request";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CinemaBookingSystem.Application.Common.Interfaces.TicketBookingSystem.Application.Common.Interfaces;

namespace CinemaBookingSystem.Infrastructure.ExternalAPI.TMDB
{
    public partial class TmdbClient : ITmdbClient
    {
        private string _baseUrl = "https://api.themoviedb.org/3";
        private readonly HttpClient _httpClient;
        private System.Lazy<Newtonsoft.Json.JsonSerializerSettings> _settings;

        public TmdbClient(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("TmdbClient");
            _baseUrl = _httpClient.BaseAddress.ToString();
            _settings = new System.Lazy<Newtonsoft.Json.JsonSerializerSettings>(() =>
            {
                var settings = new Newtonsoft.Json.JsonSerializerSettings();
                UpdateJsonSerializerSettings(settings);
                return settings;
            });


        }

        protected Newtonsoft.Json.JsonSerializerSettings JsonSerializerSettings
        {
            get { return _settings.Value; }
        }

        partial void UpdateJsonSerializerSettings(Newtonsoft.Json.JsonSerializerSettings settings);

        public string BaseUrl
        {
            get { return _baseUrl; }
            set { _baseUrl = value; }
        }

        public async Task<string> GetMovieByImdbId(string id, CancellationToken cancellationToken)
        {
            var urlBuilder = new StringBuilder();
            urlBuilder.Append(BaseUrl != null ? BaseUrl.TrimEnd('/') : "").Append("/find/");
            urlBuilder.Append(id);
            urlBuilder.Append("?api_key=7a5b6599a906e4a2d4ee54b1e9e4b35e");
            urlBuilder.Append("&language=en-US");
            urlBuilder.Append("&external_source=imdb_id");
            var client = _httpClient;
            try
            {
                using (var request = new HttpRequestMessage())
                {
                    request.Method = new HttpMethod("GET");
                    var url = urlBuilder.ToString();
                    request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);
                    var response = await client
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                        .ConfigureAwait(false);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var responseData = response.Content == null
                            ? null
                            : await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                        return responseData;
                    }
                    else
                    {
                        return "Bad request";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}

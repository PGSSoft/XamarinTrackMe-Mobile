using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TrackMe.Core.Localization.Resx;
using TrackMe.Core.Models;
using TrackMe.Core.Services.Interfaces;

namespace TrackMe.Core.Services
{
    public class SendPositionService : ISendPositionService
    {
        public async Task<WebResult> SendPosition(string token, Position position, string address, double? accuracy)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constants.BaseUrl);

                    var parameters = new Dictionary<string, string>
                    { 
                        ["token"] = token,
                        ["latitude"] = position.Latitude.ToString(CultureInfo.InvariantCulture),
                        ["longitude"] = position.Longitude.ToString(CultureInfo.InvariantCulture),
                        ["address"] = address,
                        ["accuracy"] = accuracy?.ToString(CultureInfo.InvariantCulture)
                    };

                    var response =
                        await client.PostAsync(Constants.SendPositionSubUrl, new FormUrlEncodedContent(parameters));

                      return response.IsSuccessStatusCode
                        ? WebResult.Success()
                        : WebResult.Error($"Error: {response.StatusCode}");
                }
            }
            catch (Exception){ return WebResult.Error(AppResources.ErrorSendingPosition); }
        }

        public async Task<WebResult<TokenPair>> GetToken(Position position, int duration, string address = "", double? accuracy = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Constants.BaseUrl);
                var parameters = new Dictionary<string, string>
                {
                    ["Latitude"] = position.Latitude.ToString(CultureInfo.InvariantCulture),
                    ["Longitude"] = position.Longitude.ToString(CultureInfo.InvariantCulture),
                    ["Duration"] = duration.ToString(CultureInfo.InvariantCulture),
                    ["Address"] = address,
                    ["Accuracy"] = accuracy.HasValue ? accuracy.Value.ToString(CultureInfo.InvariantCulture) : "0"
                };
                
                var response = await client.PostAsync(Constants.TokenSubUrl, new FormUrlEncodedContent(parameters));
                WebResult<TokenPair> tokenResult = null;
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    tokenResult = JsonConvert.DeserializeObject<WebResult<TokenPair>>(content);

                    if (!tokenResult.IsError)
                    {
                        Debug.WriteLine($"Got tokens: {tokenResult.Result.PublicToken} {tokenResult.Result.PrivateToken}");
                        return tokenResult;
                    }
                }

                return WebResult<TokenPair>.Error($"{AppResources.ErrorConnectionProblem}: {response.StatusCode} {tokenResult?.ErrorMessage}");         
            }
        }

        public async Task<WebResult> StopSession(string privateToken) 
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Constants.BaseUrl);
    
                    var response = await client.DeleteAsync($"{Constants.TokenSubUrl}/{privateToken}");
                    Debug.WriteLine($"Stop session result: {response.StatusCode} {response.RequestMessage.RequestUri}");
                    return response.IsSuccessStatusCode
                        ? WebResult.Success()
                        : WebResult.Error($"{AppResources.Error}: {response.StatusCode}");
                }
            }
            catch (Exception)
            {
                return WebResult.Error(AppResources.CannotStopTrackingNoInternet);
            }
        }
    }
}
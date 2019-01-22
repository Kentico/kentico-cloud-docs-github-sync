﻿using GithubService.Models.Github;
using GithubService.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GithubService.Services.Clients
{
    public class GithubClient : IGithubClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiEndpoint;
        private readonly string _accessToken;

        public GithubClient(string repositoryName, string repositoryOwner, string accessToken)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("KenticoCloudDocsGithubService", "1.0.0"));
            _apiEndpoint = $"https://api.github.com/repos/{repositoryOwner}/{repositoryName}";
            _accessToken = accessToken;
        }

        public async Task<IEnumerable<GithubTreeNode>> GetTreeNodesRecursivelyAsync()
        {
            var branchInfo = await GetContentAsync($"{_apiEndpoint}/branches/master?access_token={_accessToken}");
            var treeSha = branchInfo["commit"]["commit"]["tree"]["sha"].Value<string>();

            var treeInfo = await GetContentAsync($"{_apiEndpoint}/git/trees/{treeSha}?recursive=1&access_token={_accessToken}");
            return treeInfo["tree"].ToObject<List<GithubTreeNode>>();
        }

        public async Task<string> GetBlobContentAsync(string blobId)
        {
            var dynamicContent = await GetContentAsync($"{_apiEndpoint}/git/blobs/{blobId}?access_token={_accessToken}");
            var content = Convert.FromBase64String(dynamicContent["content"].Value<string>());

            return Encoding.UTF8.GetString(content);
        }

        private async Task<JObject> GetContentAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                throw new Exception(responseContent);

            return JObject.Parse(responseContent);
        }
    }
}
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace wikiracer
{
    public interface IWikiProxy
    {
        Task<IEnumerable<string>> GetArticleLinks(string title, string continueToken = null);
    }

    public class WikiProxy: IWikiProxy
    {
        private readonly ILogger<WikiProxy> _logger;

        public string WikipediaApiUrl { get; private set; }


        public WikiProxy(IOptions<Options> options, ILogger<WikiProxy> logger)
        {
            WikipediaApiUrl = options.Value.Wikipedia.ApiUrl;
            _logger = logger;
        }

        public async Task<IEnumerable<string>> GetArticleLinks(string title, string continueToken = null)
        {
            if(String.IsNullOrEmpty(title))
                throw new ArgumentNullException();

            using(var client = new System.Net.Http.HttpClient())
            {
                var url = WikipediaApiUrl + 
                    "?action=query&prop=links&format=json&titles=" + 
                    title + 
                    (String.IsNullOrEmpty(continueToken) ? String.Empty : ("&plcontinue=" + continueToken));
                

                _logger.LogDebug("[GetArticleLinks] request url: " + url);           

                using(var respMsg = await client.GetAsync(url))
                {
                    _logger.LogDebug("[GetArticleLinks] response status: " + respMsg.StatusCode.ToString());


                    var respContent = await respMsg.Content.ReadAsStringAsync();

                    if(respMsg.StatusCode != System.Net.HttpStatusCode.OK) {
                        _logger.LogError("[GetArticleLinks] response content: " + respContent);
                        return null;  //empty result
                    }

                    JToken token = JObject.Parse(respContent);

                    var plToken = token.SelectToken("continue.plcontinue");
                    
                    continueToken = plToken == null ? null : plToken.Value<string>() ;
                    
                    var results = token.SelectTokens("query.pages.*.links[*].title").Select(x => x.ToString()).ToList();
                    
                    if(!String.IsNullOrEmpty(continueToken))
                        results.AddRange( await GetArticleLinks(title, continueToken));
                   
                    return results;
                }
            }
        }

       
    }
}
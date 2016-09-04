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
        Task<IEnumerable<string>> GetArticleLinks(string title);
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

        public async Task<IEnumerable<string>> GetArticleLinks(string title)
        {
            if(String.IsNullOrEmpty(title))
                throw new ArgumentNullException();

            using(var client = new System.Net.Http.HttpClient())
            {
                var url = WikipediaApiUrl + 
                    "?action=query&prop=links&format=json&titles=" + 
                    title;

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

                    //parse resp 
                    //handle empty results
                    //handle invalid articles

                    return token.SelectTokens("query.pages[0].links[*].title").Select(x => x.ToString());
                }
            }
        }

       
    }
}
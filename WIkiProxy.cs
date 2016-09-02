namespace wikiracer
{
    public class WikiProxy
    {
        public void GetArticle(int articleId, bool getLinks = true)
        {

            using(var client = new System.Net.Http.HttpClient())
            {
                var url = _endpointUrl + 
                    "origin=" +
                    WebUtility.UrlEncode(origin) + 
                    "&destination=" +
                    WebUtility.UrlEncode(destination) +
                    "&key=" + 
                    _apiKey;

                _logger.LogDebug("[GetDirections] request url: " + url);           

                using(var respMsg = await client.GetAsync(url))
                {
                    _logger.LogDebug("[GetDirections] response status: " + respMsg.StatusCode.ToString());


                    var respContent = await respMsg.Content.ReadAsStringAsync();

                    if(respMsg.StatusCode != HttpStatusCode.OK) {
                        _logger.LogError("[GetDirections] response content: " + respContent);
                        return null;  //empty result
                    }

                    return JsonConvert.DeserializeObject<Directions>(respContent);
                }
            }


        }
    }
}
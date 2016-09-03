using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace wikiracer
{
    public interface IRace
    {
        Task<Models.Race> FindPath(string startUrl, string endUrl);
    }

    public class BFSTraversalRace: IRace
    {
        private IWikiProxy _proxy;
        private IPathConstructor _pathConstructor;

        private int _maxNumberOfArticlesTraversed;

        public BFSTraversalRace(IWikiProxy proxy, IPathConstructor pathConstructor, IOptions<Options> options)
        {
            _proxy = proxy;
            _pathConstructor = pathConstructor;
            _maxNumberOfArticlesTraversed = options.Value.MaxNumberOfArticles;
        } 

        //note: this does not fully validate URL
        private string ParseTitle(string url)
        {
            if(string.IsNullOrEmpty(url))
                throw new ArgumentNullException();
                
            var pos = url.LastIndexOf('/');

            if(pos < 0)
                throw new ArgumentException("Invalid url provided");

            return url.Substring(pos+1);
        }

        ///<summary>
        /// Finds path from start to end by building a graph using BFS traversal
        ///</summary>
        public async Task<Models.Race> FindPath(string startUrl, string endUrl)
        {
            var visited = new HashSet<string>();

            var startArticleTitle = ParseTitle(startUrl);
            var endArticleTitle = ParseTitle(endUrl);


            visited.Add(startArticleTitle);
            var queue = new Queue<Models.Article>();
            queue.Enqueue(new Models.Article()
            {
                Title = startArticleTitle,
                Parent = null
            });

            Models.Article currentArticle = null;
            int numberOfArticlesTraversed = 0;

            while(queue.Count > 0)
            {
                currentArticle = queue.Dequeue();

                visited.Add(currentArticle.Title);

                if(++numberOfArticlesTraversed >= _maxNumberOfArticlesTraversed)
                    throw new Exception("Maximm number of articles traversed reached");

                var links = await _proxy.GetArticleLinks(currentArticle.Title);

                foreach(var link in links)
                {
                    
                    if(!visited.Contains(link)) 
                    {
                        var linkedArticle = new Models.Article()
                        {
                            Title = link,
                            Parent = currentArticle
                        };

                        queue.Enqueue(linkedArticle);

                        if(link == endArticleTitle) 
                        {   
                            return new Models.Race()
                            {
                                Start = startUrl,
                                End = endUrl,
                                Path = _pathConstructor.ConstructPath(linkedArticle)
                            };
                        }
                    }
                }
            }

            throw new Exception("Path does not exist");
        }
    }
}
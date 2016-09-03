using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace wikiracer
{
    public class Race
    {
        private IWikiProxy _proxy;
        private IPathConstructor _pathConstructor;

        public Race(IWikiProxy proxy, IPathConstructor pathConstructor)
        {
            _proxy = proxy;
            _pathConstructor = pathConstructor;
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
        public async Task<Models.Race> FindPath(string start, string end)
        {
            var visited = new HashSet<string>();

            var startArticleTitle = ParseTitle(start);
            var endArticleTitle = ParseTitle(end);


            visited.Add(startArticleTitle);
            var queue = new Queue<Models.Article>();
            queue.Enqueue(new Models.Article()
            {
                Title = startArticleTitle,
                Parent = null
            });

            Models.Article currentArticle = null;
            while(queue.Count > 0) //max limit will go here
            {
                currentArticle = queue.Dequeue();

                visited.Add(currentArticle.Title);

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
                            //make sure we have complete path before we return
                            currentArticle = linkedArticle;
                            break;
                        }
                    }
                }
            }

            return new Models.Race()
            {
                Start = start,
                End = end,
                Path = _pathConstructor.ConstructPath(currentArticle)
            };
        }
    }
}
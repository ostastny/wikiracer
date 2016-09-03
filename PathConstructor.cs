using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace wikiracer
{
    public interface IPathConstructor
    {
        List<string> ConstructPath(Models.Article currentArticle);
    }

    public class PathConstructor: IPathConstructor
    {
        private string _ArticleUrl;

        public PathConstructor(IOptions<WikipediaOptions> options)
        {
            _ArticleUrl = options.Value.ArticleUrl;
        }

        public List<string> ConstructPath(Models.Article currentArticle)
        {
            var path = new List<string>();
            while(currentArticle != null)
            {
                path.Add(_ArticleUrl + currentArticle.Title);
                currentArticle = currentArticle.Parent;
            }

            //since we traverse from the end back to start we need to reverse the path 
            path.Reverse();

            return path;
        }
    }
}
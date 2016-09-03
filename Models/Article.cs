using System.Collections.Generic;

namespace wikiracer.Models
{
    public class Article
    {
        public int ArticleId { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public Article Parent { get; set; }
    }
}
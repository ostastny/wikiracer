namespace wikiracer
{
    public class Options
    {
        public class WikipediaOptions
        {

            public string ApiUrl { get; set; }   

            public string ArticleUrl { get; set; }
        }

        public WikipediaOptions Wikipedia { get; set; }

        public int MaxNumberOfArticles { get; set; }
    }
}
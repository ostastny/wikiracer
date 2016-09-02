#Wiki Racer

Description


# General Algorithm

1. Parse start & end URLs to get article titles
2. Build start & nodes by reaching out to WikiPedia API and getting the articles and links
3. Note pageid for the end article
4. Until we reach the end article
5.    Get articles for each link from previous set of nodes
6.    Build graph and mark already visited articles to avoid cycles

## Notes

* Will use Dictionary<int, Article> for easy lookup of visited articles
* Will use Queue<Article> to track articles that should be explored next

#Complexity

#Todo

* Swagger
* Logger
#Wiki Racer

Wikiracing is a game that people play on Wikipedia. Given a starting article and an ending article, 
the objective of a wikirace is to get from the starting article to the ending article by only clicking on 
links occurring in the main bodies of wikipedia articles (not including the side navigation bar or the 
category footer).
Write a wikiracing bot, which will take race speci cations in the form of a JSON object:
and which will return the results of the race in the form of a JSON object:
```
{
''start'': ''<starting article>'', 
''end'': ''<ending article>''
}
{
''start'': ''<starting article>'', 
''end'': ''<ending article>'', 
''path'': [
        ''<starting article>'',
        ''<article at step 1>'',
        ''<article at step 2>'',
        .
        .
        .
        ''<article at step n-1>'',
        ''<ending article>''
] }
```
Each article will be identi ed with a fully expanded URL. So, for example, the Wikipedia article about 
“World War II” will be represented by the URL https://en.wikipedia.org/wiki/World_War_II

# General Algorithm ("BFS" graph traversal)

1. Parse start & end URLs to get article titles
2. Validate start & nodes by reaching out to WikiPedia API 
4. Until we reach the end article or reach limit of articles to try
5.    Get linked articles for next article in the queue
6.    Build graph and mark already visited articles to avoid cycles

## Notes

* Uses Hash<string> for easy lookup of visited articles
* Uses Queue<Article> to track articles that should be explored next

#Complexity

#Todo

* Wiki query continuation ?
* Validate start & end articles exist
* Exception handling
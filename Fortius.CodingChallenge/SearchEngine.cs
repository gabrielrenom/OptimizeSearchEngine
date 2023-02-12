using System;
using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    /// <summary>
    /// Indexing is a way to optimize search by pre-computing the results of a search query and storing them in a data structure that allows for quick retrieval. This can significantly improve the performance of the search engine, especially for complex search options.
    /// </summary>
    /// Yes, you can add caching to improve the performance of the search engine. Caching can be used to store the results of expensive computations and retrieval operations, so that subsequent requests for the same information can be served much faster. You can use a cache store, such as a Dictionary or a Hashtable, to store the results of each search query, along with the corresponding search options. Before performing a new search, you can check if the results for the same search options are already available in the cache, and if so, return the cached results instead of performing a new search. This can significantly improve the speed of the search engine, especially if you expect a large number of repeated search requests.
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;
        private readonly Dictionary<SearchOptions, SearchResults> _cache;
        private readonly Dictionary<Tuple<Color, Size>, List<Shirt>> _index;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
            _cache = new Dictionary<SearchOptions, SearchResults>();
            _index = new Dictionary<Tuple<Color, Size>, List<Shirt>>();
            //_shirts.Sort((x, y) => x.Size.CompareTo(y.Size));
           
            // Build the index
            foreach (var shirt in _shirts)
            {
                var key = Tuple.Create(shirt.Color, shirt.Size);
                if (!_index.ContainsKey(key))
                {
                    _index[key] = new List<Shirt>();
                }
                _index[key].Add(shirt);
            }
        }

        public List<SearchResults> BatchSearch(List<SearchOptions> options)
        {
            return options.AsParallel().Select(Search).ToList();
        }

        public SearchResults Search(SearchOptions options)
        {
            // Check if the results for the same search options are already available in the cache
            if (_cache.ContainsKey(options))
            {
                return _cache[options];
            }

            List<Shirt> shirts = _shirts;

            // Filter by color
            if (options.Colors.Count > 0)
            {
                shirts = shirts.Where(s => options.Colors.Contains(s.Color)).ToList();
            }

            // Filter by size
            if (options.Sizes.Count > 0)
            {
                shirts = shirts.Where(s => options.Sizes.Contains(s.Size)).ToList();
            }

            // Use the index to optimize the search
            var colorCounts = new List<ColorCount>();
            if (options.Colors.Count > 0 && options.Sizes.Count > 0)
            {
                shirts = options.Colors.SelectMany(color => options.Sizes.Select(size => Tuple.Create(color, size)))
                                    .Where(key => _index.ContainsKey(key))
                                    .SelectMany(key => _index[key])
                                    .ToList();

                colorCounts = shirts.GroupBy(s => s.Color)
                                      .Select(g => new ColorCount
                                      {
                                          Color = g.Key,
                                          Count = g.Count()
                                      })
                                      .ToList();

            }


            var sizeCounts = shirts.GroupBy(s => s.Size)
                                   .Select(g => new SizeCount
                                   {
                                       Size = g.Key,
                                       Count = g.Count()
                                   })
                                   .ToList();

            var results = new SearchResults
            {
                Shirts = shirts,
                ColorCounts = colorCounts,
                SizeCounts = sizeCounts
            };

    
            // Adding not matched colors
            Color.All.Where(color => results.ColorCounts.FirstOrDefault(x => x.Color.Name == color.Name) == null).ToList()
                .ForEach(color => results.ColorCounts.Add(new ColorCount { Count = 0, Color = color }));

            // Adding not matched sizes
            Size.All.Where(size => results.SizeCounts.FirstOrDefault(x => x.Size.Name == size.Name) == null).ToList()
                .ForEach(size => results.SizeCounts.Add(new SizeCount { Count = 0, Size = size }));

            // Add the results to the cache
            _cache[options] = results;

            return results;
        }
    }

}
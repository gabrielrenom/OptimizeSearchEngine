using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngineBasic
    {
        private readonly List<Shirt> _shirts;

        public SearchEngineBasic(List<Shirt> shirts)
        {
            _shirts = shirts;

            // TODO: data preparation and initialisation of additional data structures to improve performance goes here.

        }


        public SearchResults Search(SearchOptions options)
        {
         
            var shirts = _shirts;

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

            var colorCounts = shirts.GroupBy(s => s.Color)
                                    .Select(g => new ColorCount
                                    {
                                        Color = g.Key,
                                        Count = g.Count()
                                    })
                                    .ToList();
         
            var sizeCounts = shirts.GroupBy(s => s.Size)
                                   .Select(g => new SizeCount
                                   {
                                       Size = g.Key,
                                       Count = g.Count()
                                   })
                                   .ToList();

            var results= new SearchResults
            {
                Shirts = shirts,
                ColorCounts = colorCounts,
                SizeCounts = sizeCounts
            };

            //Adding not matched colours
            Color.All.Where(color => results.ColorCounts.FirstOrDefault(x => x.Color.Name == color.Name) == null).ToList()
                .ForEach(color => results.ColorCounts.Add(new ColorCount { Count = 0, Color = color }));

            //Adding not matched sizes
            Size.All.Where(size => results.SizeCounts.FirstOrDefault(x => x.Size.Name == size.Name) == null).ToList()
                .ForEach(size => results.SizeCounts.Add(new SizeCount { Count = 0, Size = size }));

            return results;
            
        }
    }
}
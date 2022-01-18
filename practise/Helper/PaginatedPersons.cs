using practise.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace practise.Helper
{
    public class PaginatedPersons
    {
        public PaginatedPersons(List<Persons> items, int count, int pageNumber, int itemsPerPage)
        {
            PageInfo = new PageInfo
            {
                CurrentPage = pageNumber,
                ItemsPerPage = itemsPerPage,
                TotalPages = (int)Math.Ceiling(count / (double)itemsPerPage),
                TotalItems = count
            };
            

            this.Items = items;

        }

        public PageInfo PageInfo { get; set; }

        public List<Persons> Items { get; set; }

        public static PaginatedPersons ToPaginatedPost(
            IQueryable<Persons> posts, int pageNumber, int postsPerPage)
        {
            var count = posts.Count();
            var chunk = posts.Skip((pageNumber - 1) * postsPerPage).Take(postsPerPage);
            return new PaginatedPersons(chunk.ToList(), count, pageNumber, postsPerPage);
        }
    }
}

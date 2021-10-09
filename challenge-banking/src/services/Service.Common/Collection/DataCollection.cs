using Service.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Common.Collection
{
   public class DataCollection<T>
    {
        public bool HasItems
        {
            get
            {
                return Items != null && Items.Any();
            }
        }
        public PaginationHeader Paging { get; set; }
        public IEnumerable<LinkInfo> Links { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}

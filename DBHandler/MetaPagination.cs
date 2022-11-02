namespace GeoLocationAPI.DBHandler
{

    public class MetaPagination<T>
    {
        public int Draw { get; set; } = 1;
        public int Start { get; set; } = 10;
        public int Length { get; set; } = 10;
        public string Order { get; set; } = "asc";
        public string Search { get; set; } = string.Empty;

        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public IList<T> Data { get; set; }

        public long Total { get; set; }
        public long TotalNotFiltered { get; set; }

        public IEnumerable<T> Rows { get; set; }

    }

    /// <summary>
    /// Parameter value used in Bootstrap-Table
    /// </summary>
    public class MetaPagingParams
    {
        /// <summary>
        /// Nothing but a start position 'Start' or 'Offset'. Start from 0=1st page(1-10), 1-second page(11-20) etc.
        /// </summary>
        public int offset { get; set; } = 0;
        /// <summary>
        /// 'Length' or 'Limit'
        /// </summary>
        public int limit { get; set; } = 10;
        string _order = string.Empty;
        /// <summary>
        /// Order by 'asc' or 'desc'
        /// </summary>
        public string order
        {
            get { return _order; }
            set
            {
                _order = string.IsNullOrEmpty(_order) ? string.Empty : _order;
            }
        }
        public string searchcolumn
        {
            get; set;
        }

        string _search = string.Empty;
        public string search
        {
            get; set;
            //get { return _search; }
            //set
            //{
            //    _search = string.IsNullOrEmpty(_search) ? string.Empty : _search;
            //}
        }

        string _sort = string.Empty;
        /// <summary>
        /// Sort field
        /// </summary>
        public string sort
        {
            get { return _sort; }
            set
            {
                _sort = string.IsNullOrEmpty(_sort) ? string.Empty : _sort;
            }
        }

    }
}

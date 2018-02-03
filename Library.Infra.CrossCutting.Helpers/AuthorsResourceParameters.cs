namespace Library.Infra.CrossCutting.Helpers
{
    public class AuthorsResourceParameters
    {
        private const int MaxPageSize = 20;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }

        #region Filtering and Searching properties
        public string Genre { get; set; }

        public string SearchQuery { get; set; }

        #endregion

        #region Sorting properties

        public string OrderBy { get; set; } = "Name";

        #endregion

        #region Data Shapping
        public string Fields { get; set; }

        #endregion

    }
}

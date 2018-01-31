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

        #region Filtering properties
        public string Genre { get; set; }


        #endregion

    }
}

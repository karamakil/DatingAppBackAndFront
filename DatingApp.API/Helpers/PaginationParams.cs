namespace DatingApp.API.Helpers
{
    public class PaginationParams

    {
        private const int MaxPageSize = 50;
        private int _PageSize = 10;
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => this._PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

    }
}

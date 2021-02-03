namespace DatingApp.API.Helpers
{
    public class UserParams
    {
        #region Private Properties
        private const int MaxPageSize = 50;
        private int _PageSize = 10;
        #endregion

        #region Public Properties
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get => this._PageSize;
            set => _PageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        public string CurrentUserName { get; set; }
        public string Gender { get; set; }
        public int minAge { get; set; } = 18;
        public int maxAge { get; set; } = 152;
        public string OrderBy { get; set; } = "lastActive";

        #endregion

    }
}

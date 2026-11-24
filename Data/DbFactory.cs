namespace CinemaManagement.Data
{
    public static class DbFactory
    {
        public static CinemaDbContext Create() => new CinemaDbContext();
    }
}

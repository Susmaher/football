namespace backend.Services
{
    public interface IEntityWithId
    {
        public int Id { get; set; }
    }
    public interface IEntityWithName : IEntityWithId
    {
        public string Name { get; set; }
    }
    public interface IEntityWithNameAndBirthDate : IEntityWithName
    {
        public DateOnly Birth_date { get; set; }
    }
}

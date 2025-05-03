namespace SystemSazek.Core.Sazky
{
    // Interface definition
    public interface IMapperSupertype<R, T>
    {
        R Save(T obj);
        bool Update(T obj);
    }
}

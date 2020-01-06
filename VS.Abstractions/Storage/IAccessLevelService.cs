namespace VS.Abstractions.Storage {
    public interface IAccessLevelService<T>   {

        T GetLevel(AccessLevel AccessLevel);

    }
}

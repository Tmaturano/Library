namespace Library.API.Helpers
{
    public interface ITypeHelperService
    {
        bool TypeHasProperties<T>(string fields);
    }
}

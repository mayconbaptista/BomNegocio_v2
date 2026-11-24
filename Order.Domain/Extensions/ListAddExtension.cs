
namespace Order.Domain.Extensions
{
    public static class ListAddExtension
    {
        public static IList<string> AddIf(this IList<string> list, bool check, string item)
        {
            if(check)
            {
                list.Add(item);
            }

            return list;
        }
    }
}

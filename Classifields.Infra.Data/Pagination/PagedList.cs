namespace Classifields.Infra.Data.Pagination;

public class PagedList<TEntity> : List<TEntity> where TEntity : notnull
{
    public uint PageIndex { get; init; }
    public uint PageSize { get; init; }
    public uint TotalCount { get; init; }
    public uint TotalPages { get; init; }
    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < TotalPages;

    public PagedList(List<TEntity> itens, uint count, uint pageIndex, uint pageSize)
    {
        AddRange(itens);

        PageIndex = pageIndex;
        PageSize = pageSize;
        TotalCount = count;
        TotalPages = (uint)Math.Ceiling(count / (double)PageSize);
    }

    public static async Task<PagedList<TEntity>> ToPagedListAsync(IQueryable<TEntity> source, uint pageIndex, uint pageSize)
    {
        var result = await source.Skip((int)((pageIndex - 1) * pageSize)).Take((int)pageSize).ToListAsync();
        var count = (uint)await source.CountAsync();

        return new PagedList<TEntity>(result, count, pageIndex, pageSize);
    }
}

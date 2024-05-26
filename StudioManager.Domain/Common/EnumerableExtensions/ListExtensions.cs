namespace StudioManager.Domain.Common.EnumerableExtensions;

public static class ListExtensions
{
    public static IReadOnlyList<TMember> MakeReadOnly<TMember>(this List<TMember> list) => list;
}

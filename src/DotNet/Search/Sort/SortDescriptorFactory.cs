namespace Stize.DotNet.Search.Sort
{
    public class SortDescriptorFactory
    {
        public static SortDescriptor Create(string sortValue)
        {
            if (sortValue.StartsWith("-"))
            {
                var member = sortValue.Remove(0,1);
                return new SortDescriptor
                {
                    Member = member,
                    Direction = SortDirection.Descending
                };
            }

            return new SortDescriptor
            {
                Member = sortValue,
                Direction = SortDirection.Ascending
            };
        }
    }
}
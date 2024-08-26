namespace Demo.Core.Utils
{
    public class FhirBundleLinks
    {
        public bool IsOnLastPage { get; private set; }

        public bool IsOnFirstPage { get; private set; }

        public bool HasPreviousPage { get; private set; }

        public int? PreviousPage { get; private set; }

        public bool HasNextPage { get; private set; }

        public int? NextPage { get; private set; }

        public int TotalPages { get; private set; }

        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        public int TotalItems { get; private set; }

        private FhirBundleLinks()
        {
        }

        public static FhirBundleLinks Calculate(int currentPage, int pageSize, int totalItems)
        {
            int totalPages = CalculateTotalPages(pageSize, totalItems);

            // Adjust the current page when it's out of bounds
            if (currentPage > totalPages)
            {
                currentPage = totalPages;
            }

            // Adjust the current page when it's out of bounds
            if (currentPage < 1)
            {
                currentPage = 1;
            }

            var stepPager = new FhirBundleLinks
            {
                PageSize = pageSize,
                TotalItems = totalItems,
                CurrentPage = currentPage,
                TotalPages = totalPages
            };

            if (stepPager.CurrentPage == stepPager.TotalPages && stepPager.TotalPages > 1)
            {
                stepPager.IsOnLastPage = true;
            }

            if (stepPager.CurrentPage == 1 && stepPager.TotalPages > 1)
            {
                stepPager.IsOnFirstPage = true;
            }

            if (stepPager.CurrentPage < stepPager.TotalPages)
            {
                stepPager.HasNextPage = true;
                stepPager.NextPage = stepPager.CurrentPage + 1;
            }

            if (stepPager.CurrentPage > 1)
            {
                stepPager.HasPreviousPage = true;
                stepPager.PreviousPage = stepPager.CurrentPage - 1;
            }

            return stepPager;
        }

        private static int CalculateTotalPages(int pageSize, int totalResources)
        {
            return (totalResources + pageSize - 1) / pageSize;
        }
    }
}
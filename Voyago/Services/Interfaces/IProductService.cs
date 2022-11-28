using Models.ViewModels;

namespace Services.Interfaces
{
    public interface IProductService
    {
        IQueryable<ProductAllViewModel> GetAllProducts();

        IQueryable<ProductAllViewModel> GetListOfProducts(IEnumerable<int> productIds);

        IQueryable<ProductAllViewModel> GetSimilarProducts(int productId, int modelId, int count);

        Task<ProductDetailsViewModel?> GetProductDetailsById(int productId);

        IQueryable<SearchSuggestionViewModel> GetSearchSuggestions(string text);

        Task<IQueryable<string>> GetAvailableSizes(int productId);

        Task<IQueryable<string>> GetAvailableColors(int productId);

        Task<int?> GetProductIdByModelAndSize(int modelId, string size);

        Task<int?> GetProductIdByModelAndColor(int modelId, string color);

        Task<int?> GetProductIdByModelSizeAndColor(int modelId, string size, string color);

        Task<byte[]?> GetProductThumbnailById(int photoId);

        Task<byte[]?> GetProductLargePhotoById(int photoId);

        //IEnumerable<SortParmeterViewModel> GetAllSortParameters();

        IQueryable<string> GetAllCategoryNames();

        IQueryable<string> GetAllSubCategoryNames();

        IQueryable<string> GetAllModels();

        IQueryable<string> GetAllModelsInSubCategory(string subCategory);

        IQueryable<string> GetAllSizes();

        IQueryable<string> GetAllSizesInSubCategory(string subCategory);

        IQueryable<string> GetAllColors();

        IQueryable<string> GetAllColorsInSubCategory(string subCategory);

        IEnumerable<SubCategoryViewModel?> GetTopSellingSubCategories(int count);

        Task<string> GetParentCategory(string subCategory);
    }
}

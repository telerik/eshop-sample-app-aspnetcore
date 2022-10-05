using Microsoft.EntityFrameworkCore;
using Services.ServiceExtensions;
using Services.Interfaces;
using System.Reflection;
using Models.ViewModels;
using Data;

namespace Services
{
    public class ProductService : IProductService
    {
        private readonly AdventureWorksContext dbContext;

        public ProductService(AdventureWorksContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<ProductAllViewModel> GetAllProducts()
        {
            var products = dbContext.Products;

            return MapProductsToProductAllViewModel(products);
        }

        public IQueryable<ProductAllViewModel> GetListOfProducts(IEnumerable<int> productIds)
        {
            var products = dbContext.Products.Where(p => productIds.Contains(p.ProductId));

            return MapProductsToProductAllViewModel(products);
        }

        public IQueryable<ProductAllViewModel> GetSimilarProducts(int productId, int subCategoryId, int count)
        {
            var products = dbContext.Products.Where(p => p.ProductSubcategoryId == subCategoryId && p.ProductId != productId).Take(count);

            return MapProductsToProductAllViewModel(products).Shuffle().AsQueryable();
        }

        private IQueryable<ProductAllViewModel> MapProductsToProductAllViewModel(IQueryable<Product> products)
        {
            var viewModels = products.Select(p => new ProductAllViewModel
            {
                Id = p.ProductId,
                Name = p.Name,
                ModelId = p.ProductModelId,
                Model = p.ProductModel != null ?
                            p.ProductModel.Name :
                            null,
                Price = p.ListPrice,
                Color = p.Color,
                Size = p.Size,
                Weight = p.Weight ?? 0,
                AverageRating = p.ProductReviews.Any() ?
                            p.ProductReviews.Average(pr => pr.Rating) :
                            0,
                DiscountPct = p.SpecialOfferProducts.Any(sop => sop.ProductId == p.ProductId) ?
                            p.SpecialOfferProducts.Max(sop => sop.SpecialOffer.DiscountPct) :
                            0,
                SubCategoryId = p.ProductSubcategoryId ?? 0,
                SubCategory = p.ProductSubcategory != null ?
                            p.ProductSubcategory.Name :
                            "Other",
                PhotoId = p.ProductProductPhotos.Any(pp => pp.ProductId == p.ProductId) ?
                            p.ProductProductPhotos.First(pp => pp.ProductId == p.ProductId).ProductPhotoId :
                            0,
            });

            return viewModels;
        }

        public async Task<ProductDetailsViewModel?> GetProductDetailsById(int productId)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductId == productId))
            {
                return null;
            }

            var productFromDb = await dbContext.Products.FirstAsync(p => p.ProductId == productId);
            var productViewModel = new ProductDetailsViewModel
            {
                Id = productFromDb.ProductId,
                Name = productFromDb.Name,
                ProductNumber = productFromDb.ProductNumber,
                ModelId = productFromDb.ProductModelId,
                Model = productFromDb.ProductModel != null ?
                            productFromDb.ProductModel.Name :
                            null,
                ListPrice = productFromDb.ListPrice,
                DiscountPct = productFromDb.SpecialOfferProducts.Any(sop => sop.ProductId == productFromDb.ProductId) ?
                            productFromDb.SpecialOfferProducts.Max(sop => sop.SpecialOffer.DiscountPct) :
                            0,
                Color = productFromDb.Color,
                Size = productFromDb.Size,
                Weight = productFromDb.Weight ?? 0,
                CategoryId = productFromDb.ProductSubcategory != null ?
                            productFromDb.ProductSubcategory.ProductCategoryId :
                            0,
                Category = productFromDb.ProductSubcategory != null ?
                            productFromDb.ProductSubcategory.ProductCategory.Name :
                            "Other",
                SubCategoryId = productFromDb.ProductSubcategoryId,
                SubCategory = productFromDb.ProductSubcategory != null ?
                            productFromDb.ProductSubcategory.Name :
                            "Other",
                Description = productFromDb.ProductModel != null && productFromDb.ProductModel.ProductModelProductDescriptionCultures.Any(pmd => pmd.ProductModelId == productFromDb.ProductModelId) ?
                            productFromDb.ProductModel.ProductModelProductDescriptionCultures.First(pmd => pmd.ProductModelId == productFromDb.ProductModelId).ProductDescription.Description :
                            "...",
                InStock = productFromDb.ProductInventories.Any(pi => pi.ProductId == productFromDb.ProductId) ?
                            productFromDb.ProductInventories.First(pi => pi.ProductId == productFromDb.ProductId).Quantity :
                            0,
                AverageRating = productFromDb.ProductReviews.Any() ?
                            productFromDb.ProductReviews.Average(pr => pr.Rating) :
                            0,
                PhotoId = productFromDb.ProductProductPhotos.Any(pp => pp.ProductId == productFromDb.ProductId) ?
                            productFromDb.ProductProductPhotos.First(pp => pp.ProductId == productFromDb.ProductId).ProductPhotoId :
                            0,
            };

            return productViewModel;
        }

        public IQueryable<SearchSuggestionViewModel> GetSearchSuggestions(string text)
        {
            var suggestions = dbContext.Products.Where(p => p.Name.Contains(text)).Select(p => new SearchSuggestionViewModel
            {
                ProductName = p.Name,
                SubCategory = p.ProductSubcategory != null ?
                            p.ProductSubcategory.Name :
                            "Other"
            });

            return suggestions;
        }

        public async Task<IQueryable<string>> GetAvailableSizes(int productId)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductId == productId))
            {
                return Enumerable.Empty<string>().AsQueryable();
            }

            var color = await dbContext.Products.Where(p => p.ProductId == productId).Select(p => p.Color).FirstAsync();
            var modelId = await dbContext.Products.Where(p => p.ProductId == productId).Select(p => p.ProductModelId).FirstAsync();

            if (modelId == null)
            {
                return Enumerable.Empty<string>().AsQueryable();
            }

            var availableSizes = dbContext.Products
                .Where(p => p.ProductModelId == modelId && p.Color == color && p.Size != null)
                .Select(p => p.Size ?? "")
                .Distinct();

            return availableSizes;
        }

        public async Task<IQueryable<string>> GetAvailableColors(int productId)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductId == productId))
            {
                return Enumerable.Empty<string>().AsQueryable();
            }

            var size = await dbContext.Products.Where(p => p.ProductId == productId).Select(p => p.Size).FirstAsync();
            var modelId = await dbContext.Products.Where(p => p.ProductId == productId).Select(p => p.ProductModelId).FirstAsync();

            if (modelId == null)
            {
                return Enumerable.Empty<string>().AsQueryable();
            }

            var availableColors = dbContext.Products
                .Where(p => p.ProductModelId == modelId && p.Size == size && p.Color != null)
                .Select(p => p.Color ?? "")
                .Distinct();

            return availableColors;
        }

        public async Task<int?> GetProductIdByModelAndSize(int modelId, string size)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductModelId == modelId && p.Size == size))
            {
                return null;
            }

            var productId = await dbContext.Products
                .Where(p => p.ProductModelId == modelId && p.Size == size)
                .Select(p => p.ProductId)
                .FirstAsync();

            return productId;
        }

        public async Task<int?> GetProductIdByModelAndColor(int modelId, string color)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductModelId == modelId && p.Color == color))
            {
                return null;
            }

            var productId = await dbContext.Products
                .Where(p => p.ProductModelId == modelId && p.Color == color)
                .Select(p => p.ProductId)
                .FirstAsync();

            return productId;
        }

        public async Task<int?> GetProductIdByModelSizeAndColor(int modelId, string size, string color)
        {
            if (!await dbContext.Products.AnyAsync(p => p.ProductModelId == modelId && p.Size == size && p.Color == color))
            {
                return null;
            }

            var productId = await dbContext.Products
                .Where(p => p.ProductModelId == modelId && p.Size == size && p.Color == color)
                .Select(p => p.ProductId)
                .FirstAsync();

            return productId;
        }

        public IQueryable<string> GetAllCategoryNames()
        {
            var categories = dbContext.ProductCategories.Select(pc => pc.Name);

            return categories;
        }

        public IQueryable<string> GetAllSubCategoryNames()
        {
            var subCategories = dbContext.ProductSubcategories.Select(psc => psc.Name);

            return subCategories;
        }

        public IQueryable<string> GetAllModels()
        {
            var models = dbContext.ProductModels.Select(pm => pm.Name);

            return models;
        }

        public IQueryable<string> GetAllModelsInSubCategory(string subCategory)
        {
            var models = dbContext.ProductModels
                .Where(pm => pm.Products.Any(p => p.ProductSubcategory != null && p.ProductSubcategory.Name == subCategory))
                .Select(pm => pm.Name)
                .Distinct();

            return models;
        }

        public IQueryable<string> GetAllSizes()
        {
            var sizes = dbContext.Products.Where(p => p.Size != null).Select(p => p.Size ?? "").Distinct();

            return sizes;
        }

        public IQueryable<string> GetAllSizesInSubCategory(string subCategory)
        {
            var sizes = dbContext.Products
                .Where(p => p.ProductSubcategory != null && p.ProductSubcategory.Name == subCategory)
                .Where(p => p.Size != null)
                .Select(p => p.Size ?? "")
                .Distinct();

            return sizes;
        }

        public IQueryable<string> GetAllColors()
        {
            var colors = dbContext.Products.Where(p => p.Color != null).Select(p => p.Color ?? "").Distinct();

            return colors;
        }

        public IQueryable<string> GetAllColorsInSubCategory(string subCategory)
        {
            var colors = dbContext.Products
                .Where(p => p.ProductSubcategory != null && p.ProductSubcategory.Name == subCategory)
                .Where(p => p.Color != null)
                .Select(p => p.Color ?? "")
                .Distinct();

            return colors;
        }

        public IEnumerable<SortParmeterViewModel> GetAllSortParameters()
        {
            PropertyInfo[] properties = typeof(ProductAllViewModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var parameters = new List<SortParmeterViewModel>();
            foreach (var property in properties.Where(p => !p.Name.Contains("Id")))
            {
                var text = property.Name;
                for (int i = 1; i < text.Length; ++i)
                {
                    if (Char.IsUpper(text[i]))
                    {
                        text = text.Insert(i++, " ");
                    }
                }
                var asc = new SortParmeterViewModel() { Text = text + " - Ascending", Value = property.Name, Direction = "asc" };
                var desc = new SortParmeterViewModel() { Text = text + " - Descending", Value = property.Name, Direction = "desc" };
                parameters.Add(asc);
                parameters.Add(desc);
            }

            return parameters;
        }

        public async Task<byte[]?> GetProductThumbnailById(int photoId)
        {
            if (await dbContext.ProductPhotos.AnyAsync(p => p.ProductPhotoId == photoId))
            {
                return (await dbContext.ProductPhotos.FirstAsync(p => p.ProductPhotoId == photoId)).ThumbNailPhoto;
            }

            return null;
        }

        public async Task<byte[]?> GetProductLargePhotoById(int photoId)
        {
            if (await dbContext.ProductPhotos.AnyAsync(p => p.ProductPhotoId == photoId))
            {
                return (await dbContext.ProductPhotos.FirstAsync(p => p.ProductPhotoId == photoId)).LargePhoto;
            }

            return null;
        }

        public async Task<IEnumerable<SubCategoryViewModel>> GetTopSellingSubCategories(int count)
        {
            var productSalesByid = await dbContext.SalesOrderDetails
                                                  .GroupBy(g => g.ProductId)
                                                  .Select(g => new KeyValuePair<int, int>(g.Key, g.Sum(s => s.OrderQty)))
                                                  .ToListAsync();
            var subCategorySalesById = new Dictionary<int, int>();

            foreach (var kvp in productSalesByid)
            {
                if (!await dbContext.Products.AnyAsync(p => p.ProductId == kvp.Key))
                {
                    continue;
                }
                var product = await dbContext.Products.FirstAsync(p => p.ProductId == kvp.Key);
                if (product.ProductSubcategoryId == null)
                {
                    continue;
                }

                var subCategory = (int)product.ProductSubcategoryId;
                if (!subCategorySalesById.ContainsKey(subCategory))
                {
                    subCategorySalesById.Add(subCategory, 0);
                }
                subCategorySalesById[subCategory] += kvp.Value;
            }

            var topSubCateoryIds = subCategorySalesById.OrderByDescending(x => x.Value).Take(count).Select(x => x.Key);

            var viewModels = await dbContext.ProductSubcategories.Where(psc => topSubCateoryIds.Contains(psc.ProductSubcategoryId)).Select(psc => new SubCategoryViewModel
            {
                SubCategoryId = psc.ProductSubcategoryId,
                Name = psc.Name,
                CategoryId = psc.ProductCategoryId,
                TotalSales = subCategorySalesById[psc.ProductSubcategoryId],
                ImageName = GetCategoryImageName(psc.ProductSubcategoryId)
            }).ToListAsync();

            return viewModels;
        }

        private static string GetCategoryImageName(int productSubcategoryId)
        {
            switch (productSubcategoryId)
            {
                case 1:
                    return "mountain_bikes.png";
                case 2:
                    return "road_bikes.png";
                case 3:
                    return "touring_bikes.png";
                case 4:
                    return "handlebars.png";
                case 7:
                    return "chains.png";
                case 9:
                    return "derailleurs.png";
                case 10:
                    return "forks.png";
                case 12:
                    return "mountain_frames.png";
                case 13:
                    return "pedals.png";
                case 15:
                    return "saddles.png";
                case 16:
                    return "touring_frames.png";
                case 17:
                    return "wheels.png";
                case 21:
                    return "jerseys.png";
                case 22:
                    return "shorts.png";
                case 28:
                    return "bottles_cages.png";
                case 34:
                    return "locks.png";
                case 36:
                    return "pumps.png";
                case 37:
                    return "tires_tubes1.png";
                default:
                    return "generic_bike1.png";
            }
        }

        public async Task<string> GetParentCategory(string subCategory)
        {
            if (await dbContext.ProductSubcategories.AnyAsync(psc => psc.Name == subCategory))
            {
                return (await dbContext.ProductSubcategories.FirstAsync(psc => psc.Name == subCategory)).ProductCategory.Name;
            }

            return "Other";
        }
    }
}
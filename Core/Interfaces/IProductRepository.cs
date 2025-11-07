using System;
using Core.Entities;

namespace Core.Interfaces;

public interface IProductRepository
{
    public Task<IReadOnlyList<Product>> GetProductsAsync(string? brand,string? type,string? sort);
    public Task<Product?> GetProductByIdAsync(int id);
    void AddProduct(Product product);
    void UpdateProduct(Product product);
    void DeleteProduct(Product product);
    bool ProductExists(int id);
    public Task<IReadOnlyList<string>> GetBrandsAsync();
    public Task<IReadOnlyList<string>> GetTypesAsync();
    Task<bool> SaveChangesAsync();


}

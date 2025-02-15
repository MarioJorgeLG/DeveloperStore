using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DeveloperStore.Domain.Entities;

public class Sale
{
    public Guid Id { get; private set; }
    public string SaleNumber { get; private set; }
    public DateTime SaleDate { get; private set; }
    public Guid CustomerId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public Guid BranchId { get; private set; }
    public bool IsCancelled { get; private set; }
    public List<SaleItem> Items { get; set; } = new List<SaleItem>();

    public Sale(string saleNumber, Guid customerId, Guid branchId)
    {
        Id = Guid.NewGuid();
        SaleNumber = saleNumber;
        SaleDate = DateTime.UtcNow;
        CustomerId = customerId;
        BranchId = branchId;
        TotalAmount = 0;
        IsCancelled = false;
    }

    public void AddItem(Product product, int quantity, decimal unitPrice)
    {
        if (quantity > 20)
            throw new InvalidOperationException(
                "Não é possível vender mais de 20 itens do mesmo produto."
            );

        decimal discount = 0;
        if (quantity >= 4 && quantity < 10)
            discount = 0.10m;
        if (quantity >= 10 && quantity <= 20)
            discount = 0.20m;

        var totalItemPrice = quantity * unitPrice * (1 - discount);

        // Criando corretamente um SaleItem com os parâmetros corretos
        var saleItem = new SaleItem(
            this.Id,
            product.Id,
            unitPrice,
            quantity,
            totalItemPrice,
            discount
        );

        Items.Add(saleItem);
        TotalAmount += totalItemPrice;
    }

    public void Cancel()
    {
        IsCancelled = true;
    }
}

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public decimal Price { get; private set; }

    public Product(string name, decimal price)
    {
        Id = Guid.NewGuid();
        Name = name;
        Price = price;
    }
}

public class Customer
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public Customer(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }
}

public interface ISaleRepository
{
    Task<Sale> GetByIdAsync(Guid id);
    Task<IEnumerable<Sale>> GetAllAsync();
    Task AddAsync(Sale sale);
    Task UpdateAsync(Sale sale);
    Task DeleteAsync(Guid id);
}

public interface IProductRepository
{
    Task<Product> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
}

public interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(Guid id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Guid id);
}

public class SaleService
{
    private readonly ISaleRepository _saleRepository;

    public SaleService(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    public async Task<Sale> GetSaleByIdAsync(Guid id)
    {
        return await _saleRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Sale>> GetAllSalesAsync()
    {
        return await _saleRepository.GetAllAsync();
    }

    public async Task AddSaleAsync(Sale sale)
    {
        await _saleRepository.AddAsync(sale);
    }

    public async Task UpdateSaleAsync(Sale sale)
    {
        await _saleRepository.UpdateAsync(sale);
    }

    public async Task DeleteSaleAsync(Guid id)
    {
        await _saleRepository.DeleteAsync(id);
    }
}

public class ProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> GetProductByIdAsync(Guid id)
    {
        return await _productRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _productRepository.GetAllAsync();
    }

    public async Task AddProductAsync(Product product)
    {
        await _productRepository.AddAsync(product);
    }

    public async Task UpdateProductAsync(Product product)
    {
        await _productRepository.UpdateAsync(product);
    }

    public async Task DeleteProductAsync(Guid id)
    {
        await _productRepository.DeleteAsync(id);
    }
}

public class CustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> GetCustomerByIdAsync(Guid id)
    {
        return await _customerRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        await _customerRepository.AddAsync(customer);
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        await _customerRepository.UpdateAsync(customer);
    }

    public async Task DeleteCustomerAsync(Guid id)
    {
        await _customerRepository.DeleteAsync(id);
    }
}

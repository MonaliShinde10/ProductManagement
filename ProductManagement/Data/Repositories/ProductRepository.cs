﻿using Microsoft.EntityFrameworkCore;
using ProductManagement.Data;
using ProductManagement.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;

public class ProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<ProductModel> GetAllProducts()
    {
        return _context.Products.ToList();
    }

    public List<ProductModel> AllProducts()
    {
        return _context.Products.ToList();
    }
    public ProductModel GetProductById(Guid id)
    {
        return _context.Products.FirstOrDefault(p => p.Id == id);
    }

    public void AddProduct(ProductModel product)
    {
        product.Id = Guid.NewGuid();
        _context.Products.Add(product);
        _context.SaveChanges();
    }

    public void UpdateProduct(ProductModel product)
    {
        var existingProduct = _context.Products.FirstOrDefault(p => p.Id == product.Id);
        if (existingProduct != null)
        {
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            // Update other properties as needed
            _context.SaveChanges();
        }
    }

    public void DeleteProduct(Guid id)
    {
        var productToRemove = _context.Products.FirstOrDefault(p => p.Id == id);
        if (productToRemove != null)
        {
            _context.Products.Remove(productToRemove);
            _context.SaveChanges();
        }
    }
}

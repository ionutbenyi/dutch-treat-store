using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("All products loaded!");
                return _context.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            } catch (Exception ex)
            {
                _logger.LogError($"Failed to load all products: {ex}");
                return null;
            }
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            try { 
            return _context.Products
                .Where(p => p.Category == category)
                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to load all products by category: {ex}");
                return null;
            }
        }

        public bool SaveAll()
        {
            try { 
                return _context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to save all products: {ex}");
                return false;
            }
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            try
            {
                if (includeItems)
                {
                    return _context.Orders
                        .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                        .ToList();
                }
                else
                {
                    return _context.Orders
                        .ToList();
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to load all orders: {ex}");
                return null;
            }
        }

        public Order GetOrderById(int id)
        {
            try
            {
                _logger.LogInformation("Order loaded!");
                return _context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .Where(o => o.Id == id)
                    .FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to load order: {ex}");
                return null;
            }
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }
    }
}

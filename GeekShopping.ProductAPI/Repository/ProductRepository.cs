﻿using AutoMapper;
using GeekShopping.ProductAPI.Data.ValueObjects;
using GeekShopping.ProductAPI.Model;
using GeekShopping.ProductAPI.Model.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GeekShopping.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MySqlContext _context;
        private IMapper _mapper;

        public ProductRepository(MySqlContext context, IMapper maper)
        {
            _context = context;
            _mapper = maper;                
        }

        public async Task<IEnumerable<ProductVO>> FindAll()
        {
            List<Product> products = await _context.Products.ToListAsync();

            return _mapper.Map<List<ProductVO>>(products);
        }

        public async Task<ProductVO> FindById(long id)
        {
            Product product = await _context.Products
                .Where(p => p.Id == id).FirstOrDefaultAsync() ?? new Product();

            return _mapper.Map<ProductVO>(product);
        }

        public async Task<ProductVO> Create(ProductVO productVO)
        {
            Product product = _mapper.Map<Product>(productVO);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductVO>(product);
        }
        public async Task<ProductVO> Update(ProductVO productVO)
        {
            Product product = _mapper.Map<Product>(productVO);
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProductVO>(product);
        }

        public async Task<bool> Delete(long id)
        {
            try
            {
                Product product = await _context.Products
                .Where(p => p.Id == id).FirstOrDefaultAsync() ?? new Product();

                if(product.Id <= 0)
                {
                    return false;
                }

                _context.Remove(product);
                await _context.SaveChangesAsync();  

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}

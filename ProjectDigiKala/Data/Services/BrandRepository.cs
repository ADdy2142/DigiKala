using Microsoft.EntityFrameworkCore;
using ProjectDigiKala.Data.Repositories;
using ProjectDigiKala.Models;
using ProjectDigiKala.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDigiKala.Data.Services
{
    public class BrandRepository : IBrandRepository
    {
        private ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context) => _context = context;

        public void Add(Brand brand) => _context.Brands.Add(brand);

        public async Task AddAsync(Brand brand) => await _context.Brands.AddAsync(brand);

        public void Delete(Brand brand) => _context.Brands.Remove(brand);

        public async Task DeleteAsync(Brand brand)
        {
            //var mainBrand = await GetBrandByIdAsync(brand.Id);
            //Delete(mainBrand);

            await Task.Run(() =>
            {
                Delete(brand);
            });
        }

        public void DeleteById(int id)
        {
            var brand = GetBrandById(id);
            Delete(brand);
        }

        public async Task DeleteByIdAsync(int id)
        {
            var brand = await GetBrandByIdAsync(id);
            await DeleteAsync(brand);
        }

        public Brand GetBrandById(int id) => _context.Brands.SingleOrDefault(b => b.Id == id);

        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            //return await _context.Brands.FindAsync(id);

            return await Task.Run(() =>
            {
                return GetBrandById(id);
            });
        }

        /*
         * .Include(p => p.Products).Include(o => o.Creator).Include(o => o.LastModifier)
         */

        public IEnumerable<Brand> GetBrands(int? id, byte? state, string title = "", string slug = "")
        {
            List<Brand> list;
            if (id != null)
                list = _context.Brands.Where(b => b.Id == id).Include(b => b.Creator).Include(b => b.LastModifier).ToList();
            else
                list = _context.Brands.Where(b => b.Title.Contains(title) || b.Slug.Contains(slug)).Include(b => b.Creator).Include(b => b.LastModifier).ToList();

            if (state != null)
                list = list.Where(b => b.State == (State)state).ToList();

            return list;
        }

        public async Task<IEnumerable<Brand>> GetBrandsAsync(int? id, byte? state, string title = "", string slug = "")
        {
            //return await Task.Run(() =>
            //{
            //    return GetBrands(id, title, slug, state);
            //});

            List<Brand> list;
            if (id != null)
                list = await _context.Brands.Where(b => b.Id == id).Include(b => b.Creator).Include(b => b.LastModifier).ToAsyncEnumerable().ToList();
            else
                list = await _context.Brands.Where(b => b.Title.Contains(title) || b.Slug.Contains(slug)).Include(b => b.Creator).Include(b => b.LastModifier).ToAsyncEnumerable().ToList();

            if (state != null)
                list = await list.Where(b => b.State == (State)state).ToAsyncEnumerable().ToList();

            return list;
        }

        public void Save() => _context.SaveChanges();

        public async Task SaveAsync() => await _context.SaveChangesAsync();

        public void Update(Brand brand)
        {
            var mainBrand = GetBrandById(brand.Id);
            mainBrand.LastModifier = brand.LastModifier;
            mainBrand.LastModifyDate = brand.LastModifyDate;
            //mainBrand.Products = brand.Products;
            mainBrand.Slug = brand.Slug;
            mainBrand.Title = brand.Title;
            mainBrand.State = brand.State;
            mainBrand.Photo = brand.Photo;
        }

        public async Task UpdateAsync(Brand brand)
        {
            await Task.Run(() =>
            {
                Update(brand);
            });
        }
    }
}

using Infrastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.Dto;
using AutoMapper;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo,
            IGenericRepository<ProductBrand> productBrandRepo, IGenericRepository<ProductType> productTypeRepo,
            IMapper mapper)
        {
            _productRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductWithTypesAndBrandsSpecification();

            var products = await _productRepo.ListAsync(spec);

            //return products.Select(product => new ProductToReturnDto
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    PictureUrl = product.PictureUrl,
            //    ProductBrand = product.ProductBrand.Name,
            //    ProductType = product.ProductType.Name,
            //}).ToList();
            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);
            var product = await _productRepo.GetEntityWithSpec(spec);
            //return new ProductToReturnDto
            //{
            //    Id = product.Id,
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    PictureUrl = product.PictureUrl,
            //    ProductBrand = product.ProductBrand.Name,
            //    ProductType = product.ProductType.Name,
            //};

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("types")]
        public async Task<ActionResult<List<Product>>> GetProductTypes()
        {
            var productTypes = await _productTypeRepo.GetAllAsync();

            return Ok(productTypes);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<List<Product>>> GetProductBrands()
        {
            var productBrands = await _productBrandRepo.GetAllAsync();

            return Ok(productBrands);
        }
    }
}

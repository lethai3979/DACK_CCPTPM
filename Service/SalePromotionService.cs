using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class SalePromotionService
    {
        private readonly SalePromotionRepository _salepromotionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        public SalePromotionService(SalePromotionRepository repository, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _salepromotionRepository = repository;
            _httpContextAccessor = contextAccessor;
            _mapper = mapper;
        }
        private int DeterminePromotionType()
        {
            var user = _httpContextAccessor.HttpContext?.User!;
            if (user.IsInRole("Admin"))
            {
                return 1;
            }
            return 2;
        }
        public async Task<OperationResult> GetAllAsync()
        {
            var promolist = await _salepromotionRepository.GetAllAsync();
            if (!promolist.IsNullOrEmpty())
            {
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: promolist);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<OperationResult> GetAllByPromotionTypeAsync(int typeId)
        {
            var promolist = await _salepromotionRepository.GetAllByPromotionTypeAsync(typeId);
            if (!promolist.IsNullOrEmpty())
            {
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: promolist);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<OperationResult> AddAsync(SalePromotionDTO salePromotionDto)
        {
            salePromotionDto.CreateById = _httpContextAccessor.HttpContext?.User?
                                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
            salePromotionDto.CreateOn = DateTime.Now;
            salePromotionDto.PromotionId = DeterminePromotionType();
            try
            {
                var promotion = _mapper.Map<Promotion>(salePromotionDto);
                await _salepromotionRepository.AddAsync(promotion);
                return new OperationResult(true, "Promotion added succesfully", StatusCodes.Status200OK);
            }

            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OperationResult> DeletedByIdAsync(int id)
        {
            try
            {
                var carType = await _salepromotionRepository.GetByIdAsync(id);
                if (carType == null)
                {
                    return new OperationResult(false, "Sale Promotion not found", StatusCodes.Status404NotFound);
                }
                await _salepromotionRepository.DeleteAsync(carType);
                return new OperationResult(true, "Sale Promotion deleted succesfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<OperationResult> GetByIdAsync(int id)
        {
            try
            {
                var promotion = await _salepromotionRepository.GetByIdAsync(id);
                if (promotion != null)
                {
                    var promotionDto = _mapper.Map<SalePromotionDTO>(promotion);
                    return new OperationResult(true, "Promotion found", StatusCodes.Status200OK, promotionDto);
                }
                return new OperationResult(false, "Promotion not found", StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"An error occurred: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }

/*        public async Task<OperationResult> UpdateAsync(int id, SalePromotionDTO promotionDTO)
        {
            try
            {

            }
            catch
            {

            }
        } */
    }
}

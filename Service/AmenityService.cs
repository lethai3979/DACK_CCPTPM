using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class AmenityService
    {
        public readonly AmenityRepository _amenityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        private readonly IMapper _mapper;
        public AmenityService(AmenityRepository amenityRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _amenityRepository = amenityRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var amenityList = await _amenityRepository.GetAllAsync();
            if (amenityList.Count != 0)
            {
                var amenityListVM = _mapper.Map<List<AmenityVM>>(amenityList);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: amenityListVM);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            try
            {
                var amenity = await _amenityRepository.GetByIdAsync(id);
                if (amenity != null)
                {
                    var amenityVM = _mapper.Map<AmenityVM>(amenity);
                    return new OperationResult(true, "Amenity found", StatusCodes.Status200OK, amenityVM);
                }
                return new OperationResult(false, "Amenity not found", StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"An error occurred: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<OperationResult> AddAsync(AmenityDTO amenityDTO)
        {

            try
            {
                var amenity = _mapper.Map<Amenity>(amenityDTO);
                amenity.CreatedById = _userId;
                amenity.CreatedOn = DateTime.Now;
                amenity.IsDeleted = false;
                await _amenityRepository.AddAsync(amenity);
                return new OperationResult(true, "Amenity add succesfully", StatusCodes.Status200OK);
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
                var amenity = await _amenityRepository.GetByIdAsync(id);
                if (amenity == null)
                {
                    return new OperationResult(false, "Amenity not found", StatusCodes.Status404NotFound);
                }
                amenity.ModifiedById = _userId;
                amenity.ModifiedOn = DateTime.Now;
                amenity.IsDeleted = !amenity.IsDeleted;
                await _amenityRepository.UpdateAsync(amenity);
                return new OperationResult(true, "Amenity deleted succesfully", StatusCodes.Status200OK);
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
        public async Task<OperationResult> UpdateAsync(int id, AmenityDTO amenityDTO)
        {
            try
            {
                var existingAmenity = await _amenityRepository.GetByIdAsync(id);
                if (existingAmenity == null)
                {
                    return new OperationResult(true, "Amenity not found", StatusCodes.Status404NotFound);
                }
                var amenity = _mapper.Map<Amenity>(amenityDTO);
                amenity.CreatedOn = existingAmenity.CreatedOn;
                amenity.CreatedById = existingAmenity.CreatedById;
                amenity.ModifiedById = existingAmenity.ModifiedById;
                amenity.ModifiedOn = existingAmenity.ModifiedOn;
                var isValueChange = EditHelper<Amenity>.HasChanges(amenity, existingAmenity);
                EditHelper<Amenity>.SetModifiedIfNecessary(amenity, isValueChange, existingAmenity, _userId);
                await _amenityRepository.UpdateAsync(amenity);
                return new OperationResult(true, "Amenity update succesfully", StatusCodes.Status200OK);
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
    }
}

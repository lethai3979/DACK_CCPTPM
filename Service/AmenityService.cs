using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
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
        private readonly IMapper _mapper;
        public AmenityService(AmenityRepository amenityRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _amenityRepository = amenityRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var amenityList = await _amenityRepository.GetAllAsync();
            if (amenityList.Count != 0)
            {
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: amenityList);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }


        public async Task<OperationResult> AddAsync(AmenityDTO amenityDTO)
        {
            amenityDTO.CreatedById = _httpContextAccessor.HttpContext?.User?
                                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";//Get user Id
            amenityDTO.CreatedOn = DateTime.Now;
            try
            {
                var amenity = _mapper.Map<Amenity>(amenityDTO);
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
                await _amenityRepository.DeleteAsync(amenity);
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
        //public async Task<OperationResult> UpdateAsync(Amenity entity, Amenity newEntity)
        //{
        //    try
        //    {
        //        await _amenityRepository.UpdateAsync(newEntit);
        //        return new OperationResult(true, "Amenity update succesfully", StatusCodes.Status200OK);
        //    }

        //    catch (DbUpdateException dbEx)
        //    {
        //        var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
        //        return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
        //    }
        //    catch (Exception ex)
        //    {
        //        var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
        //        return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
        //    }
        //}
    }
}

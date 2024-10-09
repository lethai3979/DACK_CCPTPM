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
        public AmenityService(AmenityRepository amenityRepository, IHttpContextAccessor httpContextAccessor)
        {
            _amenityRepository = amenityRepository;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<List<Amenity>> GetAllAsync()
        {
            var amenityList = await _amenityRepository.GetAllAsync();
            if (amenityList.Count == 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return amenityList;
        }

        public async Task<Amenity> GetByIdAsync(int id)
        {

            var amenity = await _amenityRepository.GetByIdAsync(id);
            return amenity;
        }

        public async Task AddAsync(Amenity amenity)
        {
            try
            {
                amenity.CreatedById = _userId;
                amenity.CreatedOn = DateTime.Now;
                amenity.IsDeleted = false;
                await _amenityRepository.AddAsync(amenity);           
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeletedByIdAsync(int id)
        {
            try
            {
                var amenity = await _amenityRepository.GetByIdAsync(id);
                amenity.ModifiedById = _userId;
                amenity.ModifiedOn = DateTime.Now;
                amenity.IsDeleted = !amenity.IsDeleted;
                await _amenityRepository.UpdateAsync(amenity);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException ioEx)
            {
                throw new InvalidOperationException(ioEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateAsync(int id, Amenity amenity)
        {
            try
            {
                var existingAmenity = await _amenityRepository.GetByIdAsync(id);
                amenity.CreatedOn = existingAmenity.CreatedOn;
                amenity.CreatedById = existingAmenity.CreatedById;
                amenity.ModifiedById = existingAmenity.ModifiedById;
                amenity.ModifiedOn = existingAmenity.ModifiedOn;
                var isValueChange = EditHelper<Amenity>.HasChanges(amenity, existingAmenity);
                EditHelper<Amenity>.SetModifiedIfNecessary(amenity, isValueChange, existingAmenity, _userId);
                await _amenityRepository.UpdateAsync(amenity);

            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException ioEx)
            {
                throw new InvalidOperationException(ioEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

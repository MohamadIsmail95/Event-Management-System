using AutoMapper;
using AutoMapper.Internal.Mappers;
using AutoMapper.QueryableExtensions;
using Azure;
using ClinicSystem.Dtos;
using Domain.Dtos;
using Domain.Interfaces;
using Domain.Shared.Abstractions;
using Infrastructure.Data.Repositories;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Infrastructure.Data.Services
{
    public  class CurdService<TEntity, TGetOutputDto, TKey, TCreateInput, TUpdateInput> :
        ICrudService<TEntity, TGetOutputDto, TKey, TCreateInput, TUpdateInput> where TEntity : class, IEntity
    {

        protected IAsyncRepository<TEntity, TKey> Repository;
        protected IMapper _mapper;
        public CurdService(IAsyncRepository<TEntity, TKey> repository, IMapper mapper)
        {
            _mapper = mapper;
            Repository = repository;
        }


        public async Task<ApiResponse<TGetOutputDto>> CreateAsync(TCreateInput input)
        {
            try
            {
                var entity = await MapToEntityAsync(input);
                await Repository.AddAsync(entity);
                var response = await MapToGetOutputDtoAsync(entity);
                return new ApiResponse<TGetOutputDto>(response);
            }
            catch(Exception ex)
            {
                return new ApiResponse<TGetOutputDto>(ex.Message);

            }

        }

        public async  Task<ApiResponse<TGetOutputDto>> DeleteByIdAsync(TKey id)
        {
            TEntity model= await GetEntityByIdAsync(id);

            if(model!=null)
            {
                if (model.GetType().GetProperty("IsDeleted") != null)
                {
                    var property = model.GetType().GetProperty("IsDeleted");
                    var propertyValue = (bool?)property.GetValue(model);
                    if (!propertyValue.Value)
                    {
                        // set the value
                        property.SetValue(model, true);
                    }

                    var UpdateData = await Repository.UpdateAsync(model);
                    var response = await MapToGetOutputDtoAsync(UpdateData);
                    return new ApiResponse<TGetOutputDto>(response);
                }
                else
                {
                    await Repository.DeleteAsync(model);
                    var response = await MapToGetOutputDtoAsync(model);
                    return new ApiResponse<TGetOutputDto>(response);

                }
            }

            return new ApiResponse<TGetOutputDto>("This GUID is invalid");

        }

        public async Task<ApiResponse<TGetOutputDto>> UpdateAsync(TKey id, TUpdateInput input)
        {
            try
            {
                var entity = await GetEntityByIdAsync(id);
                if(entity!=null)
                {
                    //TODO: Check if input has id different than given id and normalize if it's default value, throw ex otherwise
                    await MapToEntityAsync(input, entity);
                    await Repository.UpdateAsync(entity);
                    var response = await MapToGetOutputDtoAsync(entity);
                    return new ApiResponse<TGetOutputDto>(response);
                }

                return new ApiResponse<TGetOutputDto>("This GUID is invalid");

            }
            catch (Exception ex)
            {
                return new ApiResponse<TGetOutputDto>(ex.Message);

            }

        }

        public async Task<ApiResponse<TGetOutputDto>> GetEntityDtoByIdAsync(TKey id)
        {
            var entity= await Repository.GetByIdAsync(id);
            if(entity!=null)
            {
                var response = await MapToGetOutputDtoAsync(entity);
                return new ApiResponse<TGetOutputDto>(response);
            }

            return new ApiResponse<TGetOutputDto>("This GUID is invalid");

        }

        public async Task<ApiResponse<List<TGetOutputDto>>> GetListAsync(ApiRequestFilter input)
        {
            try
            {
                var data = Repository.GetListAsQuerableAsync(input.searchQuery);                          
                 //Data filter
                if(input.filterLists.Count!=0)
                {
                    data = Repository.GetFilterListAsQuerableAsync(data, input);
                }
                //mapping wit DTO querable
                var dataDto = data.ProjectTo<TGetOutputDto>(_mapper.ConfigurationProvider);
                //Sort and paginition
                if (!string.IsNullOrEmpty(input.sortingField))
                {

                    if (input.sortingDir == "asc")
                        dataDto = dataDto.OrderBy2(input.sortingField);

                    else if (input.sortingDir == "desc")
                        dataDto = dataDto.OrderByDescending2(input.sortingField);

                    var result = dataDto.Skip((input.pageNumber-1) * input.pageSize)
                         .Take(input.pageSize).ToList();

                    return new ApiResponse<List<TGetOutputDto>>(result, data.Count());
                }

                else
                {

                    var result = dataDto.Skip((input.pageNumber-1) * input.pageSize)
                         .Take(input.pageSize).ToList();

                    return new ApiResponse<List<TGetOutputDto>>(result, data.Count());
                }
            }
            catch(Exception ex)
            {
                return new ApiResponse<List<TGetOutputDto>>(ex.Message);

            }

        }

        protected virtual async Task<TEntity> GetEntityByIdAsync(TKey id)
        {
            var entity = await Repository.GetByIdAsync(id);
            return entity;
        }

        protected virtual TEntity MapToEntity(TCreateInput createInput)
        {
           
                var entity = _mapper.Map<TCreateInput, TEntity>(createInput);
                return entity;             
        }

        protected  Task<TEntity> MapToEntityAsync(TCreateInput createInput)
        {
            return Task.FromResult(MapToEntity(createInput));
        }

        protected virtual TGetOutputDto MapToGetOutputDto(TEntity entity)
        {
            return _mapper.Map<TEntity, TGetOutputDto>(entity);
        }

        protected  Task<TGetOutputDto> MapToGetOutputDtoAsync(TEntity entity)
        {
            return Task.FromResult(MapToGetOutputDto(entity));
        }

        protected virtual Task MapToEntityAsync(TUpdateInput updateInput, TEntity entity)
        {
            MapToEntity(updateInput, entity);
            return Task.CompletedTask;
        }

        protected virtual void MapToEntity(TUpdateInput updateInput, TEntity entity)
        {
            _mapper.Map(updateInput, entity);
        }
    }
}

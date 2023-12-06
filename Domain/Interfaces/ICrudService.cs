using ClinicSystem.Dtos;
using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICrudService<TEntity, TGetOutputDto, TKey, TCreateInput, TUpdateInput>
    {

        Task<ApiResponse<TGetOutputDto>> CreateAsync(TCreateInput input);
        Task<ApiResponse<TGetOutputDto>> UpdateAsync(TKey id, TUpdateInput input);
        Task<ApiResponse<TGetOutputDto>> DeleteByIdAsync(TKey id);
        Task<ApiResponse<TGetOutputDto>> GetEntityDtoByIdAsync(TKey id);
        Task<ApiResponse<List<TGetOutputDto>>> GetListAsync(ApiRequestFilter input);

    }
}

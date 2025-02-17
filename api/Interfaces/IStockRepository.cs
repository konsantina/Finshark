using Microsoft.EntityFrameworkCore;
using api.Models;
using System.Data;
using api.Dtos.Stock;
using api.Helpers;



namespace api.Interfaces
{
    public interface IStockRepository
    {
          Task<List<Stock>> GetALLAsync(QueryObject query);

          Task<Stock?> GetByIdAsync(int id);
          Task<Stock>  CreateAsync(Stock stockModel);
          Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto);
          Task<Stock?> DeleteAsync (int id);
          Task<bool> StockExist(int id);
          
        
    }
}
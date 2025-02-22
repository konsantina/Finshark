
using api.Data;
using api.Dtos.Comment;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using api.Dtos.Stock;
using Microsoft.Extensions.FileProviders;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Repository;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
         private readonly IStockRepository _stockRepository;
        public StockController(ApplicationDbContext context, IStockRepository stockRepository)
        {
            _stockRepository = stockRepository;
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query )
     {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stocks = await _stockRepository.GetALLAsync(query);

            var stockDto = stocks.Select(s => s.ToStockDto());

        return Ok(stocks); 

     }

      [HttpGet("{id:int}")]
     public async Task<IActionResult> GetById([FromRoute] int id)
     {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await _stockRepository.GetByIdAsync(id);

        if (stock == null)
        {
            return NotFound();
        }
             return Ok(stock.ToStockDto());

     }
       
       [HttpPost]

       public async Task<IActionResult> Create ([FromBody] CreateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //var stock = new Stock()
            //{
            //    CompanyName = stockDto.CompanyName,
            //    LastDiv = stockDto.LastDiv,
            //    MarketCap = stockDto.MarketCap,
            //    Purchase = stockDto.Purchase,
            //    Symbol = stockDto.Symbol,
            //    Industry = stockDto.Industry,
            //};
            //_context.Stocks.Add(stock);
            //_context.SaveChanges();
            var stockModel = stockDto.ToStockFromCreateDTO();
              await _stockRepository.CreateAsync(stockModel); 
              return CreatedAtAction(nameof(GetById), new {id = stockModel.Id}, stockModel.ToStockDto());

       }

       [HttpPut]
       [Route("{id:int}")]

       public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto) 
       {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _stockRepository.UpdateAsync(id, updateDto);

          if(stockModel == null)
          {    
            return NotFound();
          }
          //Ενημέρωση των πεδίων
          //Αν το Stock υπάρχει, αντιγράφουμε τις τιμές από το updateDto στο stockModel.
          //Αυτό σημαίνει ότι τα νέα δεδομένα που έστειλε ο χρήστης θα αντικαταστήσουν τα υπάρχοντα.
          
          // stockModel.Symbol = updateDto.Symbol;
          // stockModel.CompanyName = updateDto.CompanyName;
          // stockModel.Purchase = updateDto.Purchase;
          // stockModel.LastDiv = updateDto.LastDiv;
          // stockModel.Industry =updateDto.Industry;
          // stockModel.MarketCap = updateDto.MarketCap;

           await  _context.SaveChangesAsync();
           return Ok(stockModel.ToStockDto());

    }

    [HttpDelete]
    [Route("{id}")]

    public async Task<IActionResult> Delete([FromRoute] int id)
    {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await _stockRepository.DeleteAsync(id);

       if(stockModel == null)
        {
            return NotFound();
        }
   
        return NoContent();


    }
    
}
}
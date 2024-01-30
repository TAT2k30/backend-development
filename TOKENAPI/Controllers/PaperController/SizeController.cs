using BackEndDevelopment.Models.PaperProperties;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TOKENAPI.Models;
using TOKENAPI.Services;

namespace BackEndDevelopment.Controllers.PaperController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SizeController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public SizeController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseiveAPI<IEnumerable<PaperSize>>>> Get()
        {
            var sizes = await _dbContext.PaperSizes.ToListAsync();
            return Ok(new ResponseiveAPI<IEnumerable<PaperSize>>(sizes, "Get all paper sizes successfully", 200));
        }

        [HttpPost]
        public async Task<ActionResult> Create(PaperSize paperSize)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Thêm một kích thước giấy mới vào cơ sở dữ liệu
                    _dbContext.PaperSizes.Add(paperSize);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ResponseiveAPI<string>("Paper size created successfully", "Create paper size", 200));
                }

                return BadRequest(new ResponseiveAPI<string>("Invalid model state", "Create paper size", 400));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseiveAPI<string>($"Internal server error: {ex.Message}", "Create paper size", 500));
            }
        }

        [HttpPost("createrange")]
        public async Task<ActionResult> CreateRange(List<PaperSize> paperSizes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Thêm một dãy kích thước giấy mới vào cơ sở dữ liệu
                    _dbContext.PaperSizes.AddRange(paperSizes);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ResponseiveAPI<string>("Paper sizes created successfully", "Create paper sizes", 200));
                }

                return BadRequest(new ResponseiveAPI<string>("Invalid model state", "Create paper sizes", 400));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseiveAPI<string>($"Internal server error: {ex.Message}", "Create paper sizes", 500));
            }
        }
    }
}

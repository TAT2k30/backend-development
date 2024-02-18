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
        private readonly ILogger<SizeController> _logger;
        private readonly DatabaseContext _dbContext;
        public SizeController(DatabaseContext dbContext, ILogger<SizeController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseiveAPI<IEnumerable<PaperSize>>>> Get()
        {
            var sizes = await _dbContext.PaperSizes.ToListAsync();
            return Ok(new ResponseiveAPI<IEnumerable<PaperSize>>(sizes, "Get all paper sizes successfully", 200));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm]PaperSize paperSize)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var submitSize = new PaperSize()
                    {
                        Name = paperSize.Name,
                        Dimensions = paperSize.Dimensions,
                        Description = paperSize.Description,
                        Status = false,
                        CreatedAt = DateTime.Now,
                    };

                    _dbContext.PaperSizes.Add(submitSize);
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
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var paperSizeDel = await _dbContext.PaperSizes.FirstOrDefaultAsync(x => x.Id == id);
                if (paperSizeDel != null)
                {
                    _dbContext.PaperSizes.Remove(paperSizeDel);
                    await _dbContext.SaveChangesAsync();
                    return Ok(new ResponseiveAPI<string>("Paper size deleted successfully", "Delete paper size", 200));
                }
                return BadRequest(new ResponseiveAPI<string>("Invalid model state", "Delete paper size", 400));
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseiveAPI<string>($"Internal server error: {ex.Message}", "Delete paper sizes", 500));
            }
        }
       
    }
}

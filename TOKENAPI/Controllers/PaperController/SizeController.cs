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
        public async Task<ActionResult<ResponsiveAPI<IEnumerable<PaperSize>>>> Get()
        {
            var sizes = await _dbContext.PaperSizes.ToListAsync();
            return Ok(new ResponsiveAPI<IEnumerable<PaperSize>>(sizes, "Get all paper sizes successfully", 200));
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
                        Acreage = paperSize.Acreage,
                        Description = paperSize.Description,
                        Status = false,
                        CreatedAt = DateTime.Now,
                    };

                    _dbContext.PaperSizes.Add(submitSize);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ResponsiveAPI<string>("Paper size created successfully", "Create paper size", 200));
                }

                return BadRequest(new ResponsiveAPI<string>("Invalid model state", "Create paper size", 400));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponsiveAPI<string>($"Internal server error: {ex.Message}", "Create paper size", 500));
            }
        }

        [HttpPost("createrange")]
        public async Task<ActionResult> CreateRange(List<PaperSize> paperSizes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var size in paperSizes)
                    {
                        var submitSize = new PaperSize()
                        {
                            Name = size.Name,
                            Acreage = size.Acreage,
                            Description = size.Description,
                            Status = false,
                            CreatedAt = DateTime.Now,
                        };
                        await _dbContext.PaperSizes.AddAsync(submitSize);
                    }
                    
                    
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ResponsiveAPI<string>("Range of sizes created successfully", "Create paper sizes", 200));
                }

                return BadRequest(new ResponsiveAPI<string>("Invalid model state", "Create paper sizes", 400));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponsiveAPI<string>($"Internal server error: {ex.Message}", "Create paper sizes", 500));
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
                    return Ok(new ResponsiveAPI<string>("Paper size deleted successfully", "Delete paper size", 200));
                }
                return BadRequest(new ResponsiveAPI<string>("Invalid model state", "Delete paper size", 400));
            }
            catch (Exception ex)
            {

                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponsiveAPI<string>($"Internal server error: {ex.Message}", "Delete paper sizes", 500));
            }
        }
        [HttpPut]
        public async Task<ActionResult<ResponsiveAPI<PaperSize>>> UpdateSize (int id)
        {
            var sizeUpdate = await _dbContext.PaperSizes.FindAsync(id);
            if (sizeUpdate == null)
            {
                return BadRequest(new ResponsiveAPI<string>("Size not found", $"Your size's id:{id} not found", 404));    
            }
            try
            {
              sizeUpdate.Status = !sizeUpdate.Status;
                await _dbContext.SaveChangesAsync();
                return Ok(new ResponsiveAPI<PaperSize>(sizeUpdate, "Size updated successfully", 200));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                                  new ResponsiveAPI<string>($"Internal server error: {ex.Message}", "Update paper size", 500));
            }
        }
       
    }
}

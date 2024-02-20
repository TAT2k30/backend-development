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
    public class TypeController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public TypeController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<ResponsiveAPI<IEnumerable<PaperType>>>> Get()
        {
            var types = await _dbContext.PaperTypes.ToListAsync();
            return Ok(new ResponsiveAPI<IEnumerable<PaperType>>(types, "Get all paper types successfully", 200));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] PaperType paperType)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _dbContext.PaperTypes.Add(paperType);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ResponsiveAPI<string>("Paper type created successfully", "Create paper type", 200));
                }

                return BadRequest(new ResponsiveAPI<string>("Invalid model type", "Create paper type", 400));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponsiveAPI<string>($"Internal server error: {ex.Message}", "Create paper type", 500));
            }
        }

        [HttpPost("createrange")]
        public async Task<ActionResult> CreateRange(List<PaperType> paperTypes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dbContext.PaperTypes.AddRange(paperTypes);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ResponsiveAPI<string>("Paper type created successfully", "Create paper type", 200));
                }

                return BadRequest(new ResponsiveAPI<string>("Invalid model state", "Create paper type", 400));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponsiveAPI<string>($"Internal server error: {ex.Message}", "Create paper type", 500));
            }
        }
        [HttpPost("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var paperTypeDel = await _dbContext.PaperTypes.FirstOrDefaultAsync(x => x.Id == id);
                if (paperTypeDel != null)
                {
                    _dbContext.PaperTypes.Remove(paperTypeDel);
                    await _dbContext.SaveChangesAsync();
                    return Ok(new ResponsiveAPI<string>("Paper type deleted successfully", "Delete paper type", 200));
                }
                return BadRequest(new ResponsiveAPI<string>("Invalid model state", "Delete paper type", 400));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponsiveAPI<string>($"Internal server error: {ex.Message}", "Delete paper type", 500));
            }
        }
    }
}

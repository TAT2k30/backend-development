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
    public class FrameController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public FrameController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<ResponsiveAPI<IEnumerable<PaperFrame>>>> Get()
        {
            var frames = await _dbContext.PaperFrames.ToListAsync();
            return Ok(new ResponsiveAPI<IEnumerable<PaperFrame>>(frames, "Get all paper frames successfully", 200));
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] PaperFrame paperFrame)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _dbContext.PaperFrames.Add(paperFrame);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ResponsiveAPI<string>("Paper frame created successfully", "Create paper frame", 200));
                }

                return BadRequest(new ResponsiveAPI<string>("Invalid model state", "Create paper frame", 400));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponsiveAPI<string>($"Internal server error: {ex.Message}", "Create paper frame", 500));
            }
        }

        [HttpPost("createrange")]
        public async Task<ActionResult> CreateRange(List<PaperFrame> paperFrames)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _dbContext.PaperFrames.AddRange(paperFrames);
                    await _dbContext.SaveChangesAsync();

                    return Ok(new ResponsiveAPI<string>("Paper frame created successfully", "Create paper frame", 200));
                }

                return BadRequest(new ResponsiveAPI<string>("Invalid model state", "Create paper frames", 400));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponsiveAPI<string>($"Internal server error: {ex.Message}", "Create paper frame", 500));
            }
        }
        [HttpPost("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var paperFrameDel = await _dbContext.PaperFrames.FirstOrDefaultAsync(x => x.Id == id);
                if (paperFrameDel != null)
                {
                    _dbContext.PaperFrames.Remove(paperFrameDel);
                    await _dbContext.SaveChangesAsync();
                    return Ok(new ResponsiveAPI<string>("Paper frame deleted successfully", "Delete paper frame", 200));
                }
                return BadRequest(new ResponsiveAPI<string>("Invalid model state", "Delete paper frame", 400));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponsiveAPI<string>($"Internal server error: {ex.Message}", "Delete paper frame", 500));
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BhelInspectionApi.Data;
using BhelInspectionApi.Models.Entities;
using BhelInspectionApi.Models.DTOs;
using BhelInspectionApi.Services;
using AutoMapper;

namespace BhelInspectionApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InspectionFormsController : ControllerBase
    {
        private readonly InspectionDbContext _context;
        private readonly IMapper _mapper;
        private readonly IInspectionAnalysisService _analysisService;
        private readonly ILogger<InspectionFormsController> _logger;

        public InspectionFormsController(
            InspectionDbContext context,
            IMapper mapper,
            IInspectionAnalysisService analysisService,
            ILogger<InspectionFormsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _analysisService = analysisService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InspectionFormDto>>> GetInspectionForms(
            [FromQuery] string? jobNumber = null,
            [FromQuery] string? customer = null,
            [FromQuery] InspectionStatus? status = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.InspectionForms
                .Include(i => i.BladeMeasurements)
                .Include(i => i.InspectionAnalysis)
                .AsQueryable();

            if (!string.IsNullOrEmpty(jobNumber))
                query = query.Where(i => i.JobNumber.Contains(jobNumber));

            if (!string.IsNullOrEmpty(customer))
                query = query.Where(i => i.Customer.Contains(customer));

            if (status.HasValue)
                query = query.Where(i => i.Status == status.Value);

            var totalCount = await query.CountAsync();
            var inspectionForms = await query
                .OrderByDescending(i => i.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = _mapper.Map<List<InspectionFormDto>>(inspectionForms);

            Response.Headers.Add("X-Total-Count", totalCount.ToString());
            Response.Headers.Add("X-Page", page.ToString());
            Response.Headers.Add("X-Page-Size", pageSize.ToString());

            return Ok(dtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InspectionFormDto>> GetInspectionForm(int id)
        {
            var inspectionForm = await _context.InspectionForms
                .Include(i => i.BladeMeasurements)
                    .ThenInclude(b => b.MeasurementDeviations)
                .Include(i => i.InspectionAnalysis)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (inspectionForm == null)
                return NotFound();

            var dto = _mapper.Map<InspectionFormDto>(inspectionForm);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<InspectionFormDto>> CreateInspectionForm(CreateInspectionFormDto createDto)
        {
            var inspectionForm = _mapper.Map<InspectionForm>(createDto);
            
            _context.InspectionForms.Add(inspectionForm);
            await _context.SaveChangesAsync();

            // Perform analysis if measurements are provided
            if (createDto.BladeMeasurements.Any())
            {
                try
                {
                    await _analysisService.AnalyzeInspectionAsync(inspectionForm.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to perform analysis for inspection form {Id}", inspectionForm.Id);
                }
            }

            // Reload with analysis
            var createdForm = await _context.InspectionForms
                .Include(i => i.BladeMeasurements)
                .Include(i => i.InspectionAnalysis)
                .FirstOrDefaultAsync(i => i.Id == inspectionForm.Id);

            var dto = _mapper.Map<InspectionFormDto>(createdForm);
            return CreatedAtAction(nameof(GetInspectionForm), new { id = dto.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInspectionForm(int id, UpdateInspectionFormDto updateDto)
        {
            var inspectionForm = await _context.InspectionForms.FindAsync(id);
            if (inspectionForm == null)
                return NotFound();

            _mapper.Map(updateDto, inspectionForm);
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InspectionFormExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInspectionForm(int id)
        {
            var inspectionForm = await _context.InspectionForms.FindAsync(id);
            if (inspectionForm == null)
                return NotFound();

            _context.InspectionForms.Remove(inspectionForm);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/analyze")]
        public async Task<ActionResult<InspectionAnalysisDto>> AnalyzeInspection(int id)
        {
            try
            {
                var analysis = await _analysisService.AnalyzeInspectionAsync(id);
                return Ok(analysis);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error analyzing inspection form {Id}", id);
                return StatusCode(500, "An error occurred while analyzing the inspection.");
            }
        }

        [HttpPost("{id}/submit")]
        public async Task<IActionResult> SubmitInspection(int id)
        {
            var inspectionForm = await _context.InspectionForms.FindAsync(id);
            if (inspectionForm == null)
                return NotFound();

            if (inspectionForm.Status != InspectionStatus.Draft)
                return BadRequest("Only draft inspections can be submitted.");

            inspectionForm.Status = InspectionStatus.Submitted;
            await _context.SaveChangesAsync();

            // Perform analysis on submission
            try
            {
                await _analysisService.AnalyzeInspectionAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to perform analysis for submitted inspection {Id}", id);
            }

            return NoContent();
        }

        private bool InspectionFormExists(int id)
        {
            return _context.InspectionForms.Any(e => e.Id == id);
        }
    }
}
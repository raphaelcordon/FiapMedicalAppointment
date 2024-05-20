using Api.Common;
using Domain.Dtos;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class MedicalSpecialtyController : ControllerBase
{
    private readonly IMedicalSpecialtyService _medicalSpecialtyService;

    public MedicalSpecialtyController(IMedicalSpecialtyService medicalSpecialtyService)
    {
        _medicalSpecialtyService = medicalSpecialtyService;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MedicalSpecialtiesDtos.MedicalSpecialtyDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSpecialties()
    {
        var result = await _medicalSpecialtyService.GetAllSpecialties();
        return Ok(result);
    }

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(MedicalSpecialtiesDtos.MedicalSpecialtyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpecialtyById(Guid id)
    {
        try
        {
            var result = await _medicalSpecialtyService.GetSpecialtyById(id);
            return Ok(result);
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpPost]
    [ProducesResponseType(typeof(MedicalSpecialtiesDtos.MedicalSpecialtyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSpecialty([FromBody] MedicalSpecialtiesDtos.CreateMedicalSpecialtyDto createDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _medicalSpecialtyService.AddSpecialty(createDto);
        return Ok(result);
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(MedicalSpecialtiesDtos.MedicalSpecialtyDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateSpecialty(Guid id, [FromBody] MedicalSpecialtiesDtos.UpdateMedicalSpecialtyDto updateDto)
    {
        try
        {
            var result = await _medicalSpecialtyService.UpdateSpecialty(id, updateDto);
            return Ok(result);
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Roles = "Admin,Doctor")]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteSpecialty(Guid id)
    {
        try
        {
            await _medicalSpecialtyService.DeleteSpecialty(id);
            return NoContent();
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Roles = "Doctor")]
    [HttpPost("users/{userId}/specialties/{specialtyId}")]
    public async Task<IActionResult> AddSpecialtyToDoctor(Guid userId, Guid specialtyId)
    {
        try
        {
            await _medicalSpecialtyService.AddSpecialtyToDoctor(userId, specialtyId);
            return Ok("Specialty added to doctor successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Roles = "Doctor")]
    [HttpDelete("users/{userId}/specialties/{specialtyId}")]
    public async Task<IActionResult> RemoveSpecialtyFromDoctor(Guid userId, Guid specialtyId)
    {
        try
        {
            await _medicalSpecialtyService.RemoveSpecialtyFromDoctor(userId, specialtyId);
            return Ok("Specialty removed from doctor successfully.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

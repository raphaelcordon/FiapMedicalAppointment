using Api.Common;
using Api.Dtos;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class DoctorController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("Doctors")]
    [ProducesResponseType(typeof(IEnumerable<DoctorResponseDto>), StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = _doctorService.GetAll();
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("Doctors/{id}")]
    [ProducesResponseType(typeof(DoctorResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(Guid id)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(await _doctorService.GetById(id));
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPost("Doctors")]
    [ProducesResponseType(typeof(DoctorResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Insert(DoctorRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _doctorService.Insert(dto);
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut("Doctors/{id}")]
    [ProducesResponseType(typeof(DoctorResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Edit(Guid id, DoctorRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _doctorService.Edit(id, dto);
        return Ok(result);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("Doctors/{id}")]
    [ProducesResponseType(typeof(DoctorResponseDto), StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _doctorService.DeleteByIdAsync(id);
            return NoContent();
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
}
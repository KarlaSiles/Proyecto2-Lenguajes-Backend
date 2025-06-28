using Mercatika.Business;
using Mercatika.Domain;
using Microsoft.AspNetCore.Mvc;

namespace Mercatika.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly CompanyBusiness _companyBusiness;

        public CompanyController(CompanyBusiness companyBusiness)
        {
            _companyBusiness = companyBusiness;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var company = _companyBusiness.GetCompany();
                if (company == null)
                {
                    return NotFound();
                }
                return Ok(company);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Company company)
        {
            try
            {
                if (company == null || id != company.Idsetup)
                {
                    return BadRequest("Company ID mismatch");
                }

                bool success = _companyBusiness.UpdateCompany(company);
                if (!success)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Company company)
        {
            try
            {
                if (company == null)
                {
                    return BadRequest("Company object is null");
                }

                // Asignamos un ID por defecto si es necesario
                if (company.Idsetup == 0)
                {
                    company.Idsetup = 1; // O el ID que corresponda
                }

                bool success = _companyBusiness.UpdateCompany(company);
                if (!success)
                {
                    return StatusCode(500, "Error creating company");
                }

                return CreatedAtAction(nameof(Get), new { id = company.Idsetup }, company);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
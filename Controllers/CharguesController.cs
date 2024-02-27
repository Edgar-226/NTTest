using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NTTest.Context;
using NTTest.Models;

namespace NTTest.Controllers
{
    #region InitialConfiguration
    [Route("api/[controller]")]
    [ApiController]
    public class CharguesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CharguesController(ApplicationDbContext context)
        {
            _db = context;
        }

        //// GET: api/Chargues
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Company>>> GetCompany()
        //{
        //    if (_db.Company == null)
        //    {
        //        return NotFound();
        //    }
        //    return await _db.Company.ToListAsync();
        //}

        //// GET: api/Chargues/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Company>> GetCompany(string id)
        //{
        //    if (_db.Company == null)
        //    {
        //        return NotFound();
        //    }
        //    var company = await _db.Company.FindAsync(id);

        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    return company;
        //}

        //// PUT: api/Chargues/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCompany(string id, Company company)
        //{
        //    if (id != company.company_id)
        //    {
        //        return BadRequest();
        //    }

        //    _db.Entry(company).State = EntityState.Modified;

        //    try
        //    {
        //        await _db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CompanyExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Chargues
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Company>> PostCompany(Company company)
        //{
        //    if (_db.Company == null)
        //    {
        //        return Problem("Entity set 'ApplicationDbContext.Company'  is null.");
        //    }
        //    _db.Company.Add(company);
        //    try
        //    {
        //        await _db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (CompanyExists(company.company_id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtAction("GetCompany", new { id = company.company_id }, company);
        //}

        //// DELETE: api/Chargues/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCompany(string id)
        //{
        //    if (_db.Company == null)
        //    {
        //        return NotFound();
        //    }
        //    var company = await _db.Company.FindAsync(id);
        //    if (company == null)
        //    {
        //        return NotFound();
        //    }

        //    _db.Company.Remove(company);
        //    await _db.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CompanyExists(string id)
        //{
        //    return (_db.Company?.Any(e => e.company_id == id)).GetValueOrDefault();
        //}

        #endregion

        #region Read Info and chargue



        [HttpGet("LoadInfo")]
        public async Task<Response> ReadAndLoadInfo()
        {
            var response = new Response
            {
                status = "",
                message = "",
                
            };
            if (_db.Company == null || _db.Charges == null)
            {
                response.status = "error";
                response.message = "db not found";
                return response;
            }

            var pathCsv = "D:\\Repos\\NTTest\\CSV\\data_prueba_tecnica.csv";

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };
            using (var reader = new StreamReader(pathCsv))
            using (var csv = new CsvReader(reader, config))
            {
                var records = csv.GetRecords<Csv>();

                var companiesIds = records.Select(s => s.company_id).Distinct().ToList();
            }
            return response;
        }

        #endregion
    }
}
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
using System.ComponentModel.Design;
using System.Collections;

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
            var pathCsv = "D:\\Repos\\NTTest\\CSV\\data_prueba_tecnica.csv";

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                PrepareHeaderForMatch = args => args.Header.ToLower(),
            };
            using (var reader = new StreamReader(pathCsv))
            using (var csv = new CsvReader(reader, config))
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



                var records = csv.GetRecords<Csv>();

                var recordsMapped = records.Select(s => new Csv()
                {
                    id = s.id,
                    name = s.name,
                    company_id = s.company_id,
                    amount = s.amount,
                    status = s.status,
                    created_at = s.created_at,
                    paid_at = s.paid_at
                }).ToList();
                #region Create Companies
                //Obtener Id y Nombre de Empresas
                var companiesIds = recordsMapped.Select(s => s.company_id).Distinct().ToList();
                var companiesNames = recordsMapped.Select(s => s.name).Distinct().ToList();

                //Validacion de nombre y ids de empresas
                foreach (var companyName in companiesNames)
                {
                    var record = recordsMapped.FirstOrDefault(s => s.name == companyName);
                    if (record != null && !companiesIds.Contains(record.company_id))
                    {
                        companiesIds.Add(record.company_id);
                    }
                }

                var companies = new List<Company>();

                //Add de empresas en la db
                foreach (var companyId in companiesIds)
                {
                    if (!string.IsNullOrEmpty(companyId))
                    {
                        var record = recordsMapped.FirstOrDefault(s => s.company_id == companyId);
                        if (record != null)
                        {
                            var companyExist = await _db.Company.FirstOrDefaultAsync(s => (!string.IsNullOrEmpty(companyId) && s.company_id.Trim() == companyId.Trim()) || (!string.IsNullOrEmpty(companyId) && s.company_name.Trim() == record.name.Trim()));
                            if (companyExist == null)
                            {
                                var company = new Company()
                                {
                                    company_id = companyId,
                                    company_name = record.name

                                };
                                _db.Company.Add(company);
                                await _db.SaveChangesAsync();
                            }
                        }
                    }
                }

                #endregion

                #region Create Chargues
                //var companiesDb = 

                //Validar Ids repetidos o nullos o vacions
                var charguesIds = recordsMapped.Select(s => s.id).Where(s => !string.IsNullOrEmpty(s)).Distinct().ToList();
               

                //Validacion empresaId
                var companyList = await _db.Company.Where(s => companiesIds.Contains(s.company_id)).ToListAsync();

                string GetCompanyId(string companyId, string companyName)
                {
                    if (!string.IsNullOrEmpty(companyId))
                    {
                        var companyById = companyList.FirstOrDefault(s => s.company_id.Trim() == companyId.Trim());
                        if (companyById != null)
                        {
                            return companyById.company_id;
                        }
                    }

                    if (!string.IsNullOrEmpty(companyName))
                    {
                        var companyByName = companyList.FirstOrDefault(s => s.company_name.Trim() == companyName.Trim());
                        if (companyByName != null)
                        {
                            return companyByName.company_id;
                        }
                    }

                    return companyId;
                }

                //Validacion amount
                decimal GetAmount(string amount)
                {
                    decimal result = 0;
                    decimal.TryParse(amount, out result);
                    return result;
                }
                DateTime GetDateCreate(string dateString)
                {
                    var date = DateTime.Now;

                    string format = "yyyy-MM-dd";
                    DateTime result;

                    if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out result))
                    {
                        date = result;
                    }
                    return date;
                }

                DateTime? GetDatePaid(string dateString)
                {

                    DateTime? date = null;

                    string format = "yyyy-MM-dd";
                    DateTime result;

                    if (DateTime.TryParseExact(dateString, format, null, System.Globalization.DateTimeStyles.None, out result))
                    {
                        date = result;
                    }
                    return date;

                };
                var charguesList = recordsMapped.Where(s => charguesIds.Contains(s.id)).Select(s => new Charges()
                {
                    id = s.id,
                    company_id = GetCompanyId(s.company_id, s.name),
                    amount = GetAmount(s.amount),
                    status = s.status,
                    created_at = GetDateCreate(s.created_at),
                    paid_at = GetDatePaid(s.paid_at)
                
                }).ToList();

                //Carga a la db

                try
                {

                    var CharguesIdsOnDb = await _db.Charges.Where(s => charguesIds.Contains(s.id)).Select(s => s.id).ToListAsync();

                    var CharguesToAdd = charguesList.Where(s => !CharguesIdsOnDb.Contains(s.id)).ToList();

                    if (CharguesToAdd.Any())
                    {
                        await _db.Charges.AddRangeAsync(CharguesToAdd);
                        await _db.SaveChangesAsync();
                    }

                }
                catch(Exception e)
                {

                }
                #endregion
                //No descomentar xd
                //response.Data = new
                //{
                //    companyList,
                //    charguesList,
                //};
                return response;
            }
        }

        #endregion
    }
}
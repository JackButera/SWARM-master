using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWARM.EF.Data;
using SWARM.EF.Models;
using SWARM.Server.Controllers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWARM.Server.Controllers.Applications
{
    [Route("api/[controller]")]
    [ApiController]
    public class GradeTypeWeightController : BaseController, iBaseController<GradeTypeWeight>
    {
        public GradeTypeWeightController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)

        {

        }
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                GradeTypeWeight itm = await _context.GradeTypeWeights.Where(x => int.Parse(x.GradeTypeCode) == KeyValue).FirstOrDefaultAsync();
                _context.GradeTypeWeights.Remove(itm);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return Ok();
            }
            catch (Exception e)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get()
        {
            List<GradeTypeWeight> lst = await _context.GradeTypeWeights.OrderBy(x => x.GradeTypeCode).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            GradeTypeWeight itm = await _context.GradeTypeWeights.Where(x => int.Parse(x.GradeTypeCode) == KeyValue).FirstOrDefaultAsync();
            return Ok(itm);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GradeTypeWeight _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exists = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == _Item.GradeTypeCode).FirstOrDefaultAsync();
                if (exists != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade Type Weight already exists.");

                }
                exists = new GradeTypeWeight();
                exists.SchoolId = _Item.SchoolId;
                exists.SectionId = _Item.SectionId;
                exists.GradeTypeCode = _Item.GradeTypeCode;
                exists.NumberPerSection = _Item.NumberPerSection;
                exists.PercentOfFinalGrade = _Item.PercentOfFinalGrade;
                exists.DropLowest = _Item.DropLowest;
                _context.GradeTypeWeights.Add(exists);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.GradeTypeCode);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        [HttpPut]
        public async Task<IActionResult> Put([FromBody] GradeTypeWeight _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exists = await _context.GradeTypeWeights.Where(x => x.GradeTypeCode == _Item.GradeTypeCode).FirstOrDefaultAsync();
                if (exists != null)
                {

                    await this.Post(_Item);
                    return Ok();
                }
                exists = new GradeTypeWeight();
                exists.SchoolId = _Item.SchoolId;
                exists.SectionId = _Item.SectionId;
                exists.GradeTypeCode = _Item.GradeTypeCode;
                exists.NumberPerSection = _Item.NumberPerSection;
                exists.PercentOfFinalGrade = _Item.PercentOfFinalGrade;
                exists.DropLowest = _Item.DropLowest;
                _context.GradeTypeWeights.Update(exists);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.GradeTypeCode);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
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
    public class GradeController : BaseController, iBaseController<Grade>
    {
        public GradeController(SWARMOracleContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)

        {

        }
        [HttpDelete]
        [Route("Delete/{KeyValue}")]
        public async Task<IActionResult> Delete(int KeyValue)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                Grade itm = await _context.Grades.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
                _context.Grades.Remove(itm);
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
            List<Grade> lst = await _context.Grades.OrderBy(x => x.SectionId).ToListAsync();
            return Ok(lst);
        }


        [HttpGet]
        [Route("Get/{KeyValue}")]
        public async Task<IActionResult> Get(int KeyValue)
        {
            Grade itm = await _context.Grades.Where(x => x.SectionId == KeyValue).FirstOrDefaultAsync();
            return Ok(itm);

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Grade _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exists = await _context.Grades.Where(x => x.SectionId == _Item.SectionId).FirstOrDefaultAsync();
                if (exists != null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Grade already exists.");

                }
                exists = new Grade();
                exists.SectionId = _Item.SectionId;
                exists.StudentId = _Item.StudentId;
                exists.SchoolId = _Item.SchoolId;
                exists.GradeTypeCode = _Item.GradeTypeCode;
                exists.GradeTypeWeight = _Item.GradeTypeWeight;
                exists.NumericGrade = _Item.NumericGrade;
                _context.Grades.Add(exists);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SectionId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }



        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Grade _Item)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var exists = await _context.Grades.Where(x => x.SectionId == _Item.SectionId).FirstOrDefaultAsync();
                if (exists != null)
                {

                    await this.Post(_Item);
                    return Ok();
                }
                exists = new Grade();
                exists.SectionId = _Item.SectionId;
                exists.StudentId = _Item.StudentId;
                exists.SchoolId = _Item.SchoolId;
                exists.GradeTypeCode = _Item.GradeTypeCode;
                exists.GradeTypeWeight = _Item.GradeTypeWeight;
                exists.NumericGrade = _Item.NumericGrade;
                _context.Grades.Update(exists);
                await _context.SaveChangesAsync();
                trans.Commit();
                return Ok(_Item.SectionId);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }
    }
}
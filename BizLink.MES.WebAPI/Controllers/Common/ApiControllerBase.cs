using BizLink.MES.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizLink.MES.WebAPI.Controllers.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase<TEntityDto, TCreateDto, TUpdateDto, TService> : ControllerBase
            where TService : IGenericService<TEntityDto, TCreateDto, TUpdateDto>
    {
        protected readonly TService _service;

        public ApiControllerBase(TService service)
        {
            _service = service;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<TEntityDto>>> GetAll()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<TEntityDto>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public virtual async Task<ActionResult<TEntityDto>> Create([FromBody] TCreateDto createDto)
        {
            var newItem = await _service.CreateAsync(createDto);
            // 假设 GetById 方法存在，并返回新创建的资源
            return CreatedAtAction(nameof(GetById), new
            {
                id = (newItem as dynamic).Id
            }, newItem);
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update([FromBody] TUpdateDto updateDto)
        {
            var success = await _service.UpdateAsync(updateDto);
            if (!success)
            {
                return NotFound(); // 或者其他错误码
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}

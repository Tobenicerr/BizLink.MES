using Microsoft.AspNetCore.Mvc;

namespace BizLink.MES.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        [HttpPost("Upload")]
        // 限制上传文件大小，例如 10MB
        [RequestSizeLimit(10 * 1024 * 1024)]
        public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string docType)
        {
            if (file == null || file.Length == 0)
                return BadRequest("请选择文件");

            // 验证扩展名
            var ext = Path.GetExtension(file.FileName).ToLower();
            if (ext != ".pdf")
                return BadRequest("只允许上传 PDF 文件");

            // 1. 确定保存路径 (建议不要直接存数据库，存磁盘或OSS，数据库存路径)
            var folderName = Path.Combine("Resources", "PDFs", docType ?? "General");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(pathToSave))
                Directory.CreateDirectory(pathToSave);

            // 2. 生成唯一文件名防止覆盖
            var fileName = $"{Guid.NewGuid()}{ext}";
            var fullPath = Path.Combine(pathToSave, fileName);

            // 3. 保存文件
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 4. 返回相对路径或完整URL给客户端
            var dbPath = Path.Combine(folderName, fileName).Replace("\\", "/");
            return Ok(new
            {
                FilePath = dbPath,
                OriginalName = file.FileName
            });
        }
    }
}

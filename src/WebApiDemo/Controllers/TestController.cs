using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Utils;
using WebApiDemo.Services;

namespace WebApiDemo.Controllers
{
    /// <summary>
    /// 测试接口
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly TestService _testService;

        public TestController(TestService testService)
        {
            _testService = testService;
        }

        /// <summary>
        /// 测试GET接口
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        [Tags("测试GET接口")]
        public async Task<string> TestGet()
        {
            var value = await _testService.GetValue();

            return $"成功，data={value}";
        }

        /// <summary>
        /// 测试POST接口
        /// </summary>
        [HttpPost]
        [Route("[action]")]
        [Tags("测试POST接口")]
        public async Task<string> TestPost([FromBody] TestPostData data)
        {
            var value = await _testService.GetValue();

            return $"成功，data={value}";
        }
    }

    #region 实体类
    public class TestPostData
    {
        /// <summary>
        /// 数据
        /// </summary>
        public string Data { get; set; }
    }

    public class TestPostDataValidator : AbstractValidator<TestPostData>
    {
        public TestPostDataValidator()
        {
            RuleFor(m => m.Data).NotEmpty().WithMessage("不能为空");
        }
    }
    #endregion

}

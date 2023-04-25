using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Utils;
using WebApiDemo.Services;

namespace WebApiDemo.Controllers
{
    /// <summary>
    /// ���Խӿ�
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
        /// ����GET�ӿ�
        /// </summary>
        [HttpGet]
        [Route("[action]")]
        [Tags("����GET�ӿ�")]
        public async Task<string> TestGet()
        {
            var value = await _testService.GetValue();

            return $"�ɹ���data={value}";
        }

        /// <summary>
        /// ����POST�ӿ�
        /// </summary>
        [HttpPost]
        [Route("[action]")]
        [Tags("����POST�ӿ�")]
        public async Task<string> TestPost([FromBody] TestPostData data)
        {
            var value = await _testService.GetValue();

            return $"�ɹ���data={value}";
        }
    }

    #region ʵ����
    public class TestPostData
    {
        /// <summary>
        /// ����
        /// </summary>
        public string Data { get; set; }
    }

    public class TestPostDataValidator : AbstractValidator<TestPostData>
    {
        public TestPostDataValidator()
        {
            RuleFor(m => m.Data).NotEmpty().WithMessage("����Ϊ��");
        }
    }
    #endregion

}

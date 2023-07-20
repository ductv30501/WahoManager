using AutoMapper;
using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ViewModels.EmployeeViewModels;

namespace WahoAPI.Controllers.EmployeeController
{
    [Route("waho/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private IEmployeeRepositories respository = new EmployeeRepositories();
        [HttpGet("login")]
        [AllowAnonymous]

        public ActionResult<EmployeeVM> SearchProduct(string username, string password)
        {
            var employee = respository.GetEmployeeByUsernamePassword(username, password);
            if (employee == null)
            {
                return NotFound();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtKey"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JwtIssuer"],
                Subject = new ClaimsIdentity(new Claim[]
                {
                        new Claim(ClaimTypes.NameIdentifier, employee.UserName),
                        new Claim(ClaimTypes.Name, employee.EmployeeName),
                        new Claim(ClaimTypes.Email, employee.Email),
                        new Claim(ClaimTypes.Role, employee.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            employee.Token = tokenHandler.WriteToken(token);
            
            return Ok(employee);
        }
        [HttpGet("countPagingEmployee")]
        [Authorize]

        public ActionResult<int> countPagingEmployee(string? textSearch, string status, string title, int wahoId)
        {
            var total = respository.countPagingEmployee(textSearch, status, title, wahoId);
            if (total == null)
            {
                return NotFound();
            }
            return Ok(total);
        }
        [HttpGet("getEmployeePaging")]
        [Authorize]

        public ActionResult<List<Employee>> getEmployeePaging(int pageIndex, int pageSize, string? textSearch, string title, string status, int wahoId)
        {
            var employees = respository.getEmployeePaging(pageIndex, pageSize, textSearch, title, status, wahoId);
            if (employees == null || employees.Count == 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }
        [HttpPost]
        [AllowAnonymous]

        public IActionResult PostEmployee(PostEmployeeVM pe)
        {
            string message = respository.SaveEmployee(pe);
            return Ok(message);
        }
        [HttpGet("username")]
        [AllowAnonymous]

        public ActionResult<Employee> findEmployeeByUsername(string username, int wahoId)
        {
            Employee employee = respository.findEmployeeByUsername(username, wahoId);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpGet("usernameAllWaho")]
        [AllowAnonymous]

        public ActionResult<Employee> findEmployeeByUsernameAll(string username)
        {
            Employee employee = respository.findEmployeeByUsernameAll(username);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
        [HttpPut]
        [AllowAnonymous]

        public IActionResult PutEmployee(PostEmployeeVM pe)
        {
            string message = respository.updateEmployee(pe);
            return Ok(message);
        }
        [HttpGet("EmployeesInWaho")]
        [AllowAnonymous]

        public ActionResult<List<Employee>> getEmployeeInWaho(int wahoId)
        {
            List<Employee> employees = respository.GetEmployeesInWaho(wahoId);
            if (employees.Count == 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }
        [HttpGet("EmployeesInWahoByRole")]
        [AllowAnonymous]

        public ActionResult<List<Employee>> EmployeesInWahoByRole(int role, int wahoId)
        {
            List<Employee> employees = respository.GetEmployeesInWahoByRole(role, wahoId);
            if (employees.Count == 0)
            {
                return NotFound();
            }
            return Ok(employees);
        }
        [HttpGet("GetAll")]
        [AllowAnonymous]

        public ActionResult<List<Employee>> GetAll(int wahoId)
        {
            List<Employee> employees = respository.GetAllEmployeesInWaho(wahoId);
            return Ok(employees);
        }
        [HttpGet("GetByEmail")]
        [AllowAnonymous]

        public ActionResult<Employee> GetByEmail(string email)
        {
            Employee employee = respository.GetEmployeesByEmail(email);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }
    }
}

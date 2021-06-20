using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SenwesAssignment_API.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SenwesAssignment_API.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly LoadData _loadData;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
            _loadData = new LoadData();
        }

        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>Returns a list of all employees</returns>

        [HttpGet]
        public IActionResult Get()
        {
            var employeeData = _loadData.LoadEmployeeData();
            return Ok(employeeData);
        }


        [Route("Get/{empId}")]
        [HttpGet("{empId}")]
        public IActionResult GetByEmployeeId(int empId)
        {
            try
            {
                var employee = _loadData.LoadEmployeeData().Where(x => x.EmpID == empId).FirstOrDefault();
                return Ok(employee);
            }
            catch (Exception err)
            {
                return BadRequest($"There was a probrem retriving client with ID : {empId}\r error : {err}");
            }
        }

        [Route("Get/LatestEmplyees")]
        [HttpGet]
        protected IActionResult GetLastFiveYears()
        {
            var startFilterDate = DateTime.Today.AddYears(-5);
            EmployeeListDTO returnObj = new EmployeeListDTO() { };
            try
            {
                var employees = _loadData.LoadEmployeeData().OrderBy(a => a.DateOfJoining).Where(a => Convert.ToDateTime(a.DateOfJoining) >= startFilterDate || a.YearsInCompany <= 5).Select(a => new Employee
                {
                    Age = a.Age,
                    City = a.City,
                    DateOfBirth = a.DateOfBirth,
                    DateOfJoining = a.DateOfJoining,
                    EMail = a.EMail,
                    EmpID = a.EmpID,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastIncrease = a.LastIncrease,
                    LastName = a.LastName,
                    PhoneNo = a.PhoneNo,
                    Salary = a.Salary,
                    SSN = a.SSN,
                    State = a.State,
                    UserName = a.UserName,
                    YearsInCompany = a.YearsInCompany,
                    Zip = a.Zip

                }).ToList();

                if (employees.Count < 0)
                {
                    employees.ForEach(a =>
                    {
                        returnObj.EmployeeDTO.Add(a);
                    });
                }

                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving lastest employers of the last 5 years.\r the error : {ex}");
            }
        }

        [Route("Get/OlderThan30")]
        [HttpGet]
        protected IActionResult OlderThan30()
        {
            EmployeeListDTO returnObj = new EmployeeListDTO() { };
            try
            {
                var employees = _loadData.LoadEmployeeData().OrderBy(a => a.DateOfBirth).Where(a => GetEmployeeAge(a.DateOfBirth) > 30).Select(a => new Employee
                {
                    Age = a.Age,
                    City = a.City,
                    DateOfBirth = a.DateOfBirth,
                    DateOfJoining = a.DateOfJoining,
                    EMail = a.EMail,
                    EmpID = a.EmpID,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastIncrease = a.LastIncrease,
                    LastName = a.LastName,
                    PhoneNo = a.PhoneNo,
                    Salary = a.Salary,
                    SSN = a.SSN,
                    State = a.State,
                    UserName = a.UserName,
                    YearsInCompany = a.YearsInCompany,
                    Zip = a.Zip

                }).ToList();

                if (employees.Count < 0)
                {
                    employees.ForEach(a =>
                    {
                        returnObj.EmployeeDTO.Add(a);
                    });
                }

                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving all employers over the age of 30.\r the error : {ex}");
            }
        }
        protected static int GetEmployeeAge(string date)
        {
            int now = int.Parse(DateTime.Now.ToString("yyyyMMdd"));
            int dob = int.Parse(date);
            int age = (now - dob) / 10000;

            return age;
        }
        //remember to add route and params
        [HttpGet("{gender}")]
        protected IActionResult HighestPaidWorkers(string gender)
        {

            EmployeeListDTO returnObj = new EmployeeListDTO() { };
            IEnumerable<Employee> empMale;
            IEnumerable<Employee> empFemale;
            try
            {
                IEnumerable<Employee> emp = _loadData.LoadEmployeeData().OrderByDescending(a => a.Salary);
                if (gender != null)
                {
                    var forReturn = emp.Where(a => a.Gender == gender).Take(10).Select(a => new Employee
                    {
                        Age = a.Age,
                        City = a.City,
                        DateOfBirth = a.DateOfBirth,
                        DateOfJoining = a.DateOfJoining,
                        EMail = a.EMail,
                        EmpID = a.EmpID,
                        FirstName = a.FirstName,
                        Gender = a.Gender,
                        LastIncrease = a.LastIncrease,
                        LastName = a.LastName,
                        PhoneNo = a.PhoneNo,
                        Salary = a.Salary,
                        SSN = a.SSN,
                        State = a.State,
                        UserName = a.UserName,
                        YearsInCompany = a.YearsInCompany,
                        Zip = a.Zip

                    }).ToList();

                    forReturn.ForEach(a =>
                    {
                        returnObj.EmployeeDTO.Add(a);
                    });

                    return Ok(returnObj);
                }
                empMale = emp.Where(a => a.Gender == "M").Select(a => new Employee
                {
                    Age = a.Age,
                    City = a.City,
                    DateOfBirth = a.DateOfBirth,
                    DateOfJoining = a.DateOfJoining,
                    EMail = a.EMail,
                    EmpID = a.EmpID,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastIncrease = a.LastIncrease,
                    LastName = a.LastName,
                    PhoneNo = a.PhoneNo,
                    Salary = a.Salary,
                    SSN = a.SSN,
                    State = a.State,
                    UserName = a.UserName,
                    YearsInCompany = a.YearsInCompany,
                    Zip = a.Zip

                }).Take(10);
                empFemale = emp.Where(a => a.Gender == "F").Select(a => new Employee
                {
                    Age = a.Age,
                    City = a.City,
                    DateOfBirth = a.DateOfBirth,
                    DateOfJoining = a.DateOfJoining,
                    EMail = a.EMail,
                    EmpID = a.EmpID,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastIncrease = a.LastIncrease,
                    LastName = a.LastName,
                    PhoneNo = a.PhoneNo,
                    Salary = a.Salary,
                    SSN = a.SSN,
                    State = a.State,
                    UserName = a.UserName,
                    YearsInCompany = a.YearsInCompany,
                    Zip = a.Zip

                }).Take(10);

                List<Employee> employees = CombineData(empFemale.ToList(), empMale.ToList());
                if (employees.Count < 0)
                {
                    employees.ForEach(a =>
                    {
                        returnObj.EmployeeDTO.Add(a);
                    });
                }

                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving Highest Paying Genders.\r the error : {ex}");
            }
        }

        protected static List<Employee> CombineData(List<Employee> female, List<Employee> male)
        {
            List<Employee> returnData = new List<Employee>();
            female.ForEach(a =>
            {
                returnData.Add(a);
            });
            male.ForEach(a =>
            {
                returnData.Add(a);
            });

            return returnData;
        }


        [Route("Get/SearchTenant")]
        [HttpGet("{name}/{surname}/{city}")]
        protected IActionResult SearhByNameAndCity(string name, string surname, string city)
        {
            EmployeeListDTO returnObj = new EmployeeListDTO() { };
            try
            {
                var employees = _loadData.LoadEmployeeData().Where(a => (
                (a.FirstName != null && a.FirstName == name) || (a.LastName != null && a.LastName == surname) && a.City == city)
                ).Select(a => new Employee
                {
                    Age = a.Age,
                    City = a.City,
                    DateOfBirth = a.DateOfBirth,
                    DateOfJoining = a.DateOfJoining,
                    EMail = a.EMail,
                    EmpID = a.EmpID,
                    FirstName = a.FirstName,
                    Gender = a.Gender,
                    LastIncrease = a.LastIncrease,
                    LastName = a.LastName,
                    PhoneNo = a.PhoneNo,
                    Salary = a.Salary,
                    SSN = a.SSN,
                    State = a.State,
                    UserName = a.UserName,
                    YearsInCompany = a.YearsInCompany,
                    Zip = a.Zip

                }).ToList();

                if (employees.Count < 0)
                {
                    employees.ForEach(a =>
                    {
                        returnObj.EmployeeDTO.Add(a);
                    });
                }

                return Ok(returnObj);
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving Data for search by name or surname and city.\r the error : {ex}");
            }
        }


        [Route("Get/Tresure")]
        [HttpGet]
        protected IActionResult TresureSalary()
        {
            TresureSalaryDTO returnObj = new TresureSalaryDTO() { };
            try
            {
                var employees = _loadData.LoadEmployeeData().Where(a => a.FirstName == "Treasure").Select(a => new TresureSalaryDTO
                {
                    Firstame = a.FirstName,
                    LastName = a.LastName,
                    Salary = a.Salary
                }).ToList();

                return Ok(employees.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving The salary of all Thresures.\r the error : {ex}");
            }
        }

        [AllowAnonymous]
        [Route("Get/CityList")]
        [HttpGet]
        public IActionResult UnAuthCitiesList()
        {
            var startFilterDate = DateTime.Today.AddYears(-5);
            List<string> returnObj = new List<string>() { };
            try
            {
                var employees = _loadData.LoadEmployeeData().OrderBy(a => a.DateOfJoining).ToList();

                for (var i = 0; i <= employees.Count(); i++)
                {
                    returnObj.Add(employees[i].City);
                }
                return Ok(employees.Distinct());
            }
            catch (Exception ex)
            {
                return BadRequest($"There was a problem retriving The list of cities.\r the error : {ex}");
            }
        }
    }
}

﻿using AutoMapper;
using BusinessObjects.WahoModels;
using DataAccess.AutoMapperConfig;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.EmployeeViewModels;

namespace DataAccess
{
    public class EmployeeDAO
    {
        private static readonly IMapper _mapper = employeeMapper.Configure();
        private static readonly IMapper _mapperEToEVM = employeeMapper.ConfigureEToEVM();
        public static EmployeeVM GetEmployeeByUserAndPass(string userName, string password)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    EmployeeVM employeevm = (from e in context.Employees
                                             where e.UserName == userName && e.Password == password
                                             select new EmployeeVM
                                             {
                                                 UserName = e.UserName,
                                                 Password = e.Password,
                                                 Role = e.Role,
                                                 Email = e.Email,
                                                 Active = e.Active,
                                                 EmployeeName = e.EmployeeName,
                                                 WahoId = e.WahoId
                                             }).FirstOrDefault();
                    return employeevm;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static PostEmployeeVM GetPostEmployeeByUserAndPass(string userName, string password)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    Employee employee = context.Employees.FirstOrDefault(e => e.UserName == userName && e.Password == password);
                    PostEmployeeVM postEmployeeVM = _mapperEToEVM.Map<PostEmployeeVM>(employee);
                    return postEmployeeVM;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static int countPagingEmployee(string textSearch, string status, string title, int wahoId)
        {
            int TotalCount;
            try
            {
                using (var context = new WahoS8Context())
                {
                    var query = context.Employees.Where(e => e.WahoId == wahoId).AsQueryable();
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(e => e.EmployeeName.ToLower().Contains(textSearch) || e.Email.ToLower().Contains(textSearch)
                                || e.Dob.ToString().ToLower().Contains(textSearch) || e.Title.ToLower().Contains(textSearch)
                                || e.Phone.ToLower().Contains(textSearch) || e.Address.ToLower().Contains(textSearch)
                                || e.HireDate.ToString().ToLower().Contains(textSearch) || e.Role.ToString().Contains(textSearch));
                    }
                    if (status != "all")
                    {
                        query = query.Where(c => c.Active.ToString().Contains(status));
                    }
                    if (title != "all")
                    {
                        query = query.Where(e => e.Role == int.Parse(title));
                    }
                    TotalCount = query.Count();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return TotalCount;
        }
        public static List<Employee> getEmployeePaging(int pageIndex, int pageSize, string textSearch, string title, string status, int wahoId)
        {
            List<Employee> employees = new List<Employee>();
            try
            {
                using (var context = new WahoS8Context())
                {
                    var query = context.Employees.AsQueryable().Where(s => s.Active == true || s.Active == false). Where(e => e.WahoId == wahoId);
                    if (!string.IsNullOrEmpty(textSearch))
                    {
                        query = query.Where(e => e.EmployeeName.ToLower().Contains(textSearch) || e.Email.ToLower().Contains(textSearch)
                                            || e.Dob.ToString().ToLower().Contains(textSearch) || e.Title.ToLower().Contains(textSearch)
                                            || e.Phone.ToLower().Contains(textSearch)
                                            || e.Address.ToLower().Contains(textSearch) || e.HireDate.ToString().ToLower().Contains(textSearch)
                                            || e.Role.ToString().Contains(textSearch));
                    }
                    if (status != "all")
                    {
                        query = query.Where(c => c.Active.ToString().Contains(status));
                    }
                    if (title != "all")
                    {
                        query = query.Where(e => e.Role == int.Parse(title));
                    }
                    employees = query.OrderBy(s => s.UserName)
                                 .Skip((pageIndex - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return employees;
        }
        public static string SaveEmployee(PostEmployeeVM employeeVM)
        {
            Employee employee = _mapper.Map<Employee>(employeeVM);
            try
            {
                using (var context = new WahoS8Context())
                {
                    context.Employees.Add(employee);
                    context.SaveChanges();
                    return $"Thêm thành công nhân viên mới: {employee.EmployeeName} tài khoản: {employee.UserName}";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static Employee FindEmployeeByUsername(string username, int wahoId)
        {
            Employee employee = new Employee();
            try
            {
                using (var context = new WahoS8Context())
                {
                    employee = context.Employees.Include(e => e.Waho).FirstOrDefault(e => e.UserName == username && e.WahoId == wahoId);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return employee;
        }
        public static Employee FindEmployeeByUsernameAll(string username)
        {
            Employee employee = new Employee();
            try
            {
                using (var context = new WahoS8Context())
                {
                    employee = context.Employees.FirstOrDefault(e => e.UserName == username);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return employee;
        }
        public static string updateEmployee(PostEmployeeVM employeeVM)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    Employee employee = new Employee();
                    employee = _mapper.Map<Employee>(employeeVM);
                    context.Entry<Employee>(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                    return $"dã sửa thông tin của {employee.EmployeeName} thành công";
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<Employee> GetEmployeesInWaho(int wahoId)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    List<Employee> employees = context.Employees.Where(e => e.WahoId == wahoId && e.Role != 2).ToList();
                    return employees;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<Employee> GetAllEmployeesInWaho(int wahoId)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    List<Employee> employees = context.Employees.Where(e => e.WahoId == wahoId).ToList();
                    return employees;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static List<Employee> GetEmployeesInWahoByRole(int role, int wahoId)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    List<Employee> employees = context.Employees.Where(e => e.WahoId == wahoId && e.Role != role).ToList();
                    return employees;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public static Employee GetEmployeesByEmail(string email)
        {
            try
            {
                using (var context = new WahoS8Context())
                {
                    return context.Employees.FirstOrDefault(e => e.Email == email);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}

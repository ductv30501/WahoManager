﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects.WahoModels;
using Waho.DataService;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using ViewModels.EmployeeViewModels;

namespace WahoClient.Pages.Admin.Employees
{
    public class EditModel : PageModel
    {
        private readonly HttpClient client = null;
        private string employeeAPIUrl = "";
        private readonly Author _author;
        public EditModel( Author author)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            employeeAPIUrl = "https://localhost:7019/waho/Employee";
            _author = author;
        }
        public string message { get; set; }
        public string successMessage { get; set; }
        [BindProperty]
        public Employee Employee { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            //author
            if (!_author.IsAuthor(1))
            {
                return RedirectToPage("/accessDenied", new { message = "Trình quản lý của Admin" });
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var req = HttpContext.Request;
            //get data form form submit 
            string raw_userName = req.Form["userName"];
            string raw_EmployeeName = req.Form["employeeName"];
            string raw_title = req.Form["title"];
            string raw_dob = req.Form["dob"];
            string raw_hireDate = req.Form["hireDate"];
            string raw_phone = req.Form["phone"];
            string raw_addrress = req.Form["addrress"];
            string raw_email = req.Form["email"];
            string raw_note = req.Form["note"];
            string raw_role = req.Form["role"];

            // find employee by username

            HttpResponseMessage responseEmployee = await client.GetAsync($"{employeeAPIUrl}/username?username={raw_userName}");
            string strDataEmployee = await responseEmployee.Content.ReadAsStringAsync();
            Employee _employee = JsonConvert.DeserializeObject<Employee>(strDataEmployee);
            PostEmployeeVM _Employee = new PostEmployeeVM();
            _Employee.UserName = raw_userName;
            _Employee.EmployeeName = raw_EmployeeName;
            _Employee.Title = raw_title;
            _Employee.Password = _employee.Password;
            _Employee.WahoId = _employee.WahoId;
            if (string.IsNullOrEmpty(raw_dob))
            {
                message = "Bạn điền ngày sinh";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            _Employee.Dob = DateTime.Parse(raw_dob);
            if (!string.IsNullOrEmpty(raw_hireDate))
            {
                _Employee.HireDate = DateTime.Parse(raw_hireDate);
            }

            if (string.IsNullOrEmpty(raw_email))
            {
                message = "Bạn điền email của nhân viên";
                TempData["message"] = message;
                return RedirectToPage("./Index");
            }
            if (string.IsNullOrEmpty(req.Form["activeUpdate"]))
            {
                _Employee.Active = false;
            }
            else
            {
                _Employee.Active = true;
            }
            _Employee.Email = raw_email;
            _Employee.Note = raw_note;
            _Employee.Phone = raw_phone;
            _Employee.Address = raw_addrress;
            _Employee.Role = Int32.Parse(raw_role);

            //update to data
            var json = JsonConvert.SerializeObject(_Employee);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PutAsync(employeeAPIUrl, content);
            string messageResponse = await response.Content.ReadAsStringAsync();

            TempData["successMessage"] = messageResponse;
            return RedirectToPage("./Index");
        }

    }
}

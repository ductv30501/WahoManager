﻿using BusinessObjects.WahoModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Drawing.Printing;
using System.Net.Http.Headers;
using ViewModels.DashBoardViewModels;
using ViewModels.EmployeeViewModels;
using Waho.DataService;

namespace WahoClient.Pages.Admin
{
    [Authorize(Roles = "1")]

    public class IndexModel : PageModel
    {
        private readonly Author _author;
        private readonly HttpClient client = null;
        private string adminAPIUrl = "";
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IndexModel(ILogger<IndexModel> logger, Author author, IHttpContextAccessor httpContextAccessor)
        {
            _author = author;
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            adminAPIUrl = "https://localhost:7019/waho/Admin";
            _httpContextAccessor = httpContextAccessor;
        }
        public int numberBill { get; set; } = 0;
        public double totalMoney { get; set; } = 0;
        public int numberReturn { get; set; } = 0;
        public decimal totalMoneyReturn { get; set; } = 0;
        private DateTime now = DateTime.Today;
        private List<BillDetail> billDetailInday { get; set; } = new List<BillDetail>();
        private List<ReturnOrder> returnOrders { get; set; } = new List<ReturnOrder>();
        private List<BillDetail> billDetailYesterday { get; set; } = new List<BillDetail>();
        private List<BillDetail> billDetailMonth { get; set; } = new List<BillDetail>();
        public double percentYes { get; set; }
        private double totalMoneyYes { get; set; }
        public double percentMonthYes { get; set; }
        private double totalMoneyMonthYes { get; set; }
        public List<string> ProductNames { get; set; } = new List<string>();
        public List<double> Quantities { get; set; } = new List<double>();
        public List<double> QuantitiesDay { get; set; } = new List<double>();
        public List<int> Days { get; set; } = new List<int>();

        private List<KeyValuePair<string, double>> temp = new List<KeyValuePair<string, double>>();
        private List<KeyValuePair<int, double>> tempDay = new List<KeyValuePair<int, double>>();
        [BindProperty(SupportsGet = true)]
        public int selectFilter { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public DateTime dateQuery { get; set; } = DateTime.Now;
        [BindProperty(SupportsGet = true)]
        public DateTime dateQueryDay { get; set; } = DateTime.Now;
        private double total(List<BillDetail> list)
        {
            double totalM = 0;
            foreach (var b in list)
            {
                totalM += b.Quantity * b.Product.UnitPrice * (1 - b.Discount);
            }
            return totalM;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            //author
            if (User.Identity?.IsAuthenticated == false)
            {
                return RedirectToPage("/accessDenied", new { message = "do bạn chưa đăng nhập" });
            }
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Request.Cookies["AccessToken"]);
            // get data from session
            var employeeJson = _httpContextAccessor.HttpContext.Session.GetString("Employee");
            EmployeeVM employeeVM = JsonConvert.DeserializeObject<EmployeeVM>(employeeJson);
            //hóa đơn trong ngày
            HttpResponseMessage responseNB = await client.GetAsync($"{adminAPIUrl}/TotalBillInDay?wahoID={employeeVM.WahoId}");
            if ((int)responseNB.StatusCode == 401) return RedirectToPage("/accessDenied");

            string strDataNB = await responseNB.Content.ReadAsStringAsync();
            if (responseNB.IsSuccessStatusCode)
            {
                numberBill = int.Parse(strDataNB);
            }
            //numberBill = _context.Bills.Where(b => b.Date == now).Count();
            //get billdetail in day
            HttpResponseMessage rBINDay = await client.GetAsync($"{adminAPIUrl}/BillDetails?date={now.ToString()}&wahoID={employeeVM.WahoId}");
            if ((int)rBINDay.StatusCode == 401) return RedirectToPage("/accessDenied");

            string DataRBDINDay = await rBINDay.Content.ReadAsStringAsync();
            if (rBINDay.IsSuccessStatusCode)
            {
                billDetailInday = JsonConvert.DeserializeObject<List<BillDetail>>(DataRBDINDay);
            }
            //billDetailInday = _dataService.GetBillDetails(now);

            totalMoney = total(billDetailInday);
            //hoàn đơn trong ngày
            HttpResponseMessage responseNO = await client.GetAsync($"{adminAPIUrl}/TotalReturnInDay?wahoID={employeeVM.WahoId}");
            if ((int)responseNO.StatusCode == 401) return RedirectToPage("/accessDenied");

            string strDataNO = await responseNO.Content.ReadAsStringAsync();
            if (responseNO.IsSuccessStatusCode)
            {
                numberReturn = int.Parse(strDataNO);

            }
            //numberReturn = _context.ReturnOrders.Where(b => b.Date == now).Count();

            //hoàn đơn
            HttpResponseMessage rROD = await client.GetAsync($"{adminAPIUrl}/ReturnOrdersInDay?wahoID={employeeVM.WahoId}");
            if ((int)rROD.StatusCode == 401) return RedirectToPage("/accessDenied");

            string DataROD = await rROD.Content.ReadAsStringAsync();
            if (rROD.IsSuccessStatusCode)
            {
                returnOrders = JsonConvert.DeserializeObject<List<ReturnOrder>>(DataROD);
            }
            //returnOrders = _dataService.GetReturOrderByDay(now);

            foreach (var b in returnOrders)
            {
                totalMoneyReturn += b.PaidCustomer;
            }
            //get bill detail yesterday
            HttpResponseMessage rBINYesDay = await client.GetAsync($"{adminAPIUrl}/BillDetails?date={now.AddDays(-1)}&wahoID={employeeVM.WahoId}");
            if ((int)rBINYesDay.StatusCode == 401) return RedirectToPage("/accessDenied");

            string DataRBDINYesDay = await rBINYesDay.Content.ReadAsStringAsync();
            if (rBINYesDay.IsSuccessStatusCode)
            {
                billDetailYesterday = JsonConvert.DeserializeObject<List<BillDetail>>(DataRBDINYesDay);
            }
            //billDetailYesterday = _dataService.GetBillDetails(now.AddDays(-1));

            totalMoneyYes = total(billDetailYesterday);
            if (totalMoneyYes != 0)
            {
                percentYes = ((totalMoney - totalMoneyYes) / totalMoneyYes) * 100;
            }
            else
            {
                percentYes = numberBill * 100;
            }
            HttpResponseMessage rBINMonth = await client.GetAsync($"{adminAPIUrl}/BillDetails?date={now.AddMonths(-1)}&wahoID={employeeVM.WahoId}");
            if ((int)rBINMonth.StatusCode == 401) return RedirectToPage("/accessDenied");

            string DataRBDINMonth = await rBINMonth.Content.ReadAsStringAsync();
            if (rBINMonth.IsSuccessStatusCode)
            {
                billDetailMonth = JsonConvert.DeserializeObject<List<BillDetail>>(DataRBDINMonth);
            }
            //billDetailMonth = _dataService.GetBillDetails(now.AddMonths(-1));

            totalMoneyMonthYes = total(billDetailMonth);
            if (totalMoneyMonthYes != 0)
            {
                percentMonthYes = ((totalMoney - totalMoneyMonthYes) / totalMoneyMonthYes) * 100;
            }
            else
            {
                percentMonthYes = numberBill * 100;
            }

            //dash board danh số sản phẩm theo tháng
            if (selectFilter == 1)
            {
                int monthQueryT = dateQuery.Month;
                int yearQueryT = dateQuery.Year;
                //theo doanh số bill
                HttpResponseMessage rTotalBINMonth = await client.GetAsync($"{adminAPIUrl}/totalBillMMVMs?month={monthQueryT}&year={yearQueryT}&wahoID={employeeVM.WahoId}");
                if ((int)rTotalBINMonth.StatusCode == 401) return RedirectToPage("/accessDenied");

                string dataRTotalBDINMonth = await rTotalBINMonth.Content.ReadAsStringAsync();
                var results = new List<TotalMMVM>();
                if (rTotalBINMonth.IsSuccessStatusCode)
                {
                    results = JsonConvert.DeserializeObject<List<TotalMMVM>>(dataRTotalBDINMonth);
                }

                // order

                HttpResponseMessage rTotalODINMonth = await client.GetAsync($"{adminAPIUrl}/totalOrdersMMVMs?month={monthQueryT}&year={yearQueryT}&wahoID={employeeVM.WahoId}");
                if ((int)rTotalODINMonth.StatusCode == 401) return RedirectToPage("/accessDenied");

                string dataRTotalODINMonth = await rTotalODINMonth.Content.ReadAsStringAsync();
                var resultsOrder = new List<TotalMMVM>();
                if (rTotalODINMonth.IsSuccessStatusCode)
                {
                    resultsOrder = JsonConvert.DeserializeObject<List<TotalMMVM>>(dataRTotalODINMonth);
                }

                // Create a dictionary to store the product totals
                var tempDictionary = new Dictionary<string, double>();

                // Add the product totals from resultsOrder to tempDictionary
                foreach (var ro in resultsOrder)
                {
                    if (!tempDictionary.ContainsKey(ro.ProductName))
                    {
                        tempDictionary.Add(ro.ProductName, ro.TotalQuantity);
                    }
                }

                // Combine the product totals from results and resultsOrder
                foreach (var rs in results)
                {
                    if (tempDictionary.ContainsKey(rs.ProductName))
                    {
                        tempDictionary[rs.ProductName] += rs.TotalQuantity;
                    }
                    else
                    {
                        tempDictionary.Add(rs.ProductName, rs.TotalQuantity);
                    }
                }

                // Convert tempDictionary back to a list
                var temp = tempDictionary.ToList();

                //sort
                temp = temp.OrderByDescending(x => x.Value).ToList();
                for (int i = 0; i < temp.Count && i < 10; i++)
                {
                    ProductNames.Add(temp[i].Key);
                    Quantities.Add(temp[i].Value);
                }

            }
            else
            {
                int monthQueryN = dateQuery.Month;
                int yearQueryN = dateQuery.Year;
                //theo số lượng
                HttpResponseMessage RNUMBIll = await client.GetAsync($"{adminAPIUrl}/totalNumberBillMs?month={monthQueryN}&year={yearQueryN}&wahoID={employeeVM.WahoId}");
                if ((int)RNUMBIll.StatusCode == 401) return RedirectToPage("/accessDenied");

                string dataRNumbill = await RNUMBIll.Content.ReadAsStringAsync();
                var results = new List<TotalMMVM>();
                if (RNUMBIll.IsSuccessStatusCode)
                {
                    results = JsonConvert.DeserializeObject<List<TotalMMVM>>(dataRNumbill);
                }

                // order
                HttpResponseMessage RNUMOrder = await client.GetAsync($"{adminAPIUrl}/totalNummberOrdersMMVMs?month={monthQueryN}&year={yearQueryN}&wahoID={employeeVM.WahoId}");
                if ((int)RNUMOrder.StatusCode == 401) return RedirectToPage("/accessDenied");

                string dataRNumOrder = await RNUMOrder.Content.ReadAsStringAsync();
                var resultsOrder = new List<TotalMMVM>();
                if (RNUMOrder.IsSuccessStatusCode)
                {
                    resultsOrder = JsonConvert.DeserializeObject<List<TotalMMVM>>(dataRNumOrder);
                }

                // Create a dictionary to store the product totals
                var tempDictionary = new Dictionary<string, double>();

                // Add the product totals from resultsOrder to tempDictionary
                foreach (var ro in resultsOrder)
                {
                    if (!tempDictionary.ContainsKey(ro.ProductName))
                    {
                        tempDictionary.Add(ro.ProductName, ro.TotalQuantity);
                    }
                }

                // Combine the product totals from results and resultsOrder
                foreach (var rs in results)
                {
                    if (tempDictionary.ContainsKey(rs.ProductName))
                    {
                        tempDictionary[rs.ProductName] += rs.TotalQuantity;
                    }
                    else
                    {
                        tempDictionary.Add(rs.ProductName, rs.TotalQuantity);
                    }
                }

                // Convert tempDictionary back to a list
                var temp = tempDictionary.ToList();

                //sort
                temp = temp.OrderByDescending(x => x.Value).ToList();
                for (int i = 0; i < temp.Count && i < 10; i++)
                {
                    ProductNames.Add(temp[i].Key);
                    Quantities.Add(temp[i].Value);
                }
            }
            // doanh số theo ngày trong tháng trong tháng
            //theo doanh số bill
            int monthQuery = dateQueryDay.Month;
            int yearQuery = dateQueryDay.Year;
            HttpResponseMessage RNUMBIllDayINM = await client.GetAsync($"{adminAPIUrl}/totalNumberBillDayInMs?month={monthQuery}&year={yearQuery}&wahoID={employeeVM.WahoId}");
            if ((int)RNUMBIllDayINM.StatusCode == 401) return RedirectToPage("/accessDenied");

            string DataRNumbillDayINM = await RNUMBIllDayINM.Content.ReadAsStringAsync();
            var resultsDay = new List<DayInMonth>();
            if (RNUMBIllDayINM.IsSuccessStatusCode)
            {
                resultsDay = JsonConvert.DeserializeObject<List<DayInMonth>>(DataRNumbillDayINM);
            }

            // order

            HttpResponseMessage RNUMOrderDayINM = await client.GetAsync($"{adminAPIUrl}/totalNumberOrdersDayInMs?month={monthQuery}&year={yearQuery}&wahoID={employeeVM.WahoId}");
            if ((int)RNUMOrderDayINM.StatusCode == 401) return RedirectToPage("/accessDenied");

            string DataRNumOrderDayINM = await RNUMOrderDayINM.Content.ReadAsStringAsync();
            var resultsOrderDay = new List<DayInMonth>();
            if (RNUMOrderDayINM.IsSuccessStatusCode)
            {
                resultsOrderDay = JsonConvert.DeserializeObject<List<DayInMonth>>(DataRNumOrderDayINM);
            }

            // Create a dictionary to store the daily totals
            var tempDayDictionary = new Dictionary<int, double>();

            // Add the daily totals from resultsOrderDay to tempDayDictionary
            foreach (var ro in resultsOrderDay)
            {
                if (!tempDayDictionary.ContainsKey(ro.Day))
                {
                    tempDayDictionary.Add(ro.Day, ro.TotalQuantity);
                }
            }

            // Combine the daily totals from resultsDay and resultsOrderDay
            foreach (var rs in resultsDay)
            {
                if (tempDayDictionary.ContainsKey(rs.Day))
                {
                    tempDayDictionary[rs.Day] += rs.TotalQuantity;
                }
                else
                {
                    tempDayDictionary.Add(rs.Day, rs.TotalQuantity);
                }
            }

            // Convert tempDayDictionary back to a list
            var tempDay = tempDayDictionary.ToList();


            for (int i = 0; i < tempDay.Count; i++)
            {
                Days.Add(tempDay[i].Key);
                QuantitiesDay.Add(tempDay[i].Value);
            }

            return Page();
        }
    }
}
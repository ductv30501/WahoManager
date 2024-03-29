﻿using BusinessObjects.WahoModels;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.CustomerViewModels;

namespace Repositories.CustomerRepository
{
    public class CustomerRepositories : ICustomerRepository
    {
        public Customer getCustomerByBillIdBillType(int billlId, int billType, int wahoId) => CustomersDAO.getCustomerByBillIdBillType(billlId,billType,wahoId);

        public List<Customer> GetCustomersSearch(string textSearch, int wahoId) => CustomersDAO.GetCustomersSearch(textSearch, wahoId);

        public int SaveCustomer(CustomerVM customerVM) => CustomersDAO.SaveCustomer(customerVM);
    }
}

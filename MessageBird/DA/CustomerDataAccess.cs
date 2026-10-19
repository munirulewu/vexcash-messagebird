/*
 * File name            : CustomerDataAccess.cs
 * Author               : Munirul Islam
 * Date                 : 18 October 2026
 * Version              : 1.0
 *
 * Description          : Implementation of  CustomerDataAccess
 *
 * Modification history :
 * Name                         Date                            Desc
 * 
 * 
 * Copyright (c) 2026: THIS IS WRITTEN BY MUNIRUL ISLAM  FOR VEXCASH TEST PROJECT
 */

using MessageBird.DatabaseContext;
using MessageBird.Interfaces;
using MessageBird.Models;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace MessageBird.DA
{
    public class CustomerDataAccess: ICustomerInfo
    {
        private readonly AppDbContext _db;
        public CustomerDataAccess(AppDbContext db)
        {
            _db = db;
        }

        public async Task PrintCustomerListAsync()
        {
            List<CustomerDTO> customers = await GetCustomersAsync();

            if (customers.Count == 0)
            {
                Console.WriteLine("No customers found.");
                return;
            }

            Console.WriteLine("\n========== CUSTOMER LIST ==========");

            Console.WriteLine(
                $"{"Credit Number",-25} {"Customer Name",-30}");

            Console.WriteLine(new string('-', 55));

            foreach (var customer in customers)
            {
                Console.WriteLine(
                    $"{customer.CustomerCreditNumber,-25} " +
                    $"{customer.CustomerName,-30}");
            }

            Console.WriteLine(new string('-', 55));
            Console.WriteLine($"Total Customers: {customers.Count}");
        }
        public async Task<List<CustomerDTO>> GetCustomersAsync()
        {
            try
            {
                Log.Information("Starting customer lookup.");

                var customers = await _db.Customer_info
                    .AsNoTracking()
                    .Select(x => new CustomerDTO
                    {
                        CustomerCreditNumber = x.CreditNumber,
                        CustomerName = x.CustName
                    })
                    .ToListAsync();

                if (customers.Count == 0)
                {
                    Log.Warning("No customers were found.");
                    return new List<CustomerDTO>();
                }

                Log.Information(
                    "Customer lookup completed successfully. Found {CustomerCount} customers.",
                    customers.Count);

                return customers;
            }
            catch (OperationCanceledException)
            {
                Log.Warning("Customer lookup was canceled.");
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(
                    ex,
                    "An error occurred while retrieving customer information.");

                throw;
            }
        }
    }
}    
    

﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace VideoRental
{
    class Program
    {
        static List<Movie> ownedMovieList = new List<Movie>();
        static List<Customer> registeredCustomerList = new List<Customer>();

        static void Main(string[] args)
        {
            SetDefaultVideoList();
            SetCustomerList();

            ShowConsoleMenuView();

            while(RunMainMenu());
        }

        private static void SetDefaultVideoList()
        {
            ownedMovieList.Add(new Movie("일반 1", MOVETYPE.REGULAR));
            ownedMovieList.Add(new Movie("일반 2", MOVETYPE.REGULAR));
            ownedMovieList.Add(new Movie("신작 1", MOVETYPE.NEW_RELEASE));
            ownedMovieList.Add(new Movie("신작 2", MOVETYPE.NEW_RELEASE));
            ownedMovieList.Add(new Movie("어린이 1", MOVETYPE.CHILDRENS));
            ownedMovieList.Add(new Movie("어린이 2", MOVETYPE.CHILDRENS));
        }

        private static void SetCustomerList()
        {
            registeredCustomerList.Add(new Customer("고객"));
        }

        private static void ShowConsoleMenuView()
        {
            StringBuilder menu = new StringBuilder();
            menu.AppendLine("Main Menu");
            menu.AppendLine("0 : Print All Video Title");
            menu.AppendLine("1 : Rental");
            menu.AppendLine("2 : Return");
            menu.AppendLine("3 : Save to File");
            menu.AppendLine("4 : Receipt");
            menu.AppendLine("5 : Exit");
            menu.AppendLine();
            menu.AppendLine("Select Menu :");
            menu.AppendLine("------------------");

            Console.WriteLine(menu);
        }

        private static bool RunMainMenu()
        {
            int menu = 0;
            while (menu != 5)
            {
                var menuInput = Console.ReadLine();
                if (int.TryParse(menuInput, out menu))
                {
                    switch (menu)
                    {
                        case 0:
                            PrintAllVideoTitle();
                            break;
                        case 1:
                            Rental();
                            break;
                        case 2:
                            Return();
                            break;
                        case 3:
                            SaveToFile();
                            break;
                        case 4:
                            Receipt();
                            break;
                        case 5:
                            return false;
                    }
                }
            }
            
            return true;
        }

        private static void PrintAllVideoTitle()
        {
            StringBuilder videoList = new StringBuilder();
            videoList.AppendLine("Code".PadRight(10) + '\t' + "Title");

            foreach(var movie in ownedMovieList)
            {
                videoList.AppendLine( movie.getPriceCode().ToString().PadRight(10) + '\t' + movie.getTitle());
            }
            Console.WriteLine(videoList.ToString());

            ShowConsoleMenuView();
            return;
        }

        private static void Rental()
        {
            /*---Rental Menu-----
            Input customer ID :
            Input Video Title :
            Input Period : 
            
            Continue? (Y/N) // Y입력 시 Rental 메뉴 반복 N 입력 시 Main Menu로 이동
            ----------------------*/
            ConsoleKey continueRental = ConsoleKey.Y;

            while(continueRental == ConsoleKey.Y)
            {
                StringBuilder menu = new StringBuilder();
                menu.AppendLine("---Rental Menu-----");
                Console.WriteLine(menu);
                string customerid = "";
                Movie rentalMovie = ownedMovieList.FirstOrDefault();
                bool validCustomer = false;
                bool validTitle= false;
                while (!validCustomer)
                {
                    menu = new StringBuilder();
                    menu.AppendLine("Input customer ID :");
                    Console.WriteLine(menu);
                    customerid = Console.ReadLine();
                    foreach (var customer in registeredCustomerList)
                    {
                        if (customerid.Trim() == customer.getName())
                        {
                            validCustomer = true;
                        }
                    }
                }

                while (!validTitle)
                {
                    menu = new StringBuilder();
                    menu.AppendLine("Input Video Title :");
                    Console.WriteLine(menu);
                    var title = Console.ReadLine();
                    foreach (var movie in ownedMovieList)
                    {
                        if ((title == movie.getTitle()))
                        {
                            rentalMovie = movie;
                            validTitle = true;
                        }
                    }
                }

                int period = 0;
                bool validPeriod = false;
                
                while(!validPeriod)
                {
                    menu = new StringBuilder();
                    menu.AppendLine("Input Period :");
                    Console.WriteLine(menu);
                    var inputperiod = Console.ReadLine();
                    if (int.TryParse(inputperiod, out period))
                    {
                        validPeriod = true;
                    }
                }
                
                var rentalcustomer = registeredCustomerList.Where(cus => cus.getName() == customerid).FirstOrDefault();
                rentalcustomer.addRental(new Rental(rentalMovie, period));

                Console.WriteLine("Continue? (Y/N)");
                continueRental = Console.ReadKey().Key;

            }

            ShowConsoleMenuView();
            return;
        }

        private static void Return()
        {
            /*---Return Menu-----
            Input customer ID :
            Input Video Title :
            
            Continue? (Y/N) // Y입력 시 Rental 메뉴 반복 N 입력 시 Main Menu로 이동
            -----------------------*/
            ConsoleKey continueReturn = ConsoleKey.Y;

            while(continueReturn == ConsoleKey.Y)
            {
                StringBuilder menu = new StringBuilder();
                menu.AppendLine("---Return Menu-----");
                Console.WriteLine(menu);
                string customerid = "";
                Movie returnMovie = ownedMovieList.FirstOrDefault();
                bool validCustomer = false;
                bool validTitle = false;
                while (!validCustomer)
                {
                    menu = new StringBuilder();
                    menu.AppendLine("Input customer ID :");
                    Console.WriteLine(menu);
                    customerid = Console.ReadLine();
                    foreach (var customer in registeredCustomerList)
                    {
                        if (customerid.Trim() == customer.getName())
                        {
                            validCustomer = true;
                        }
                    }
                }

                while (!validTitle)
                {
                    menu = new StringBuilder();
                    menu.AppendLine("Input Video Title :");
                    Console.WriteLine(menu);
                    var title = Console.ReadLine();
                    var customer = registeredCustomerList.Where(cus => cus.getName() == customerid).FirstOrDefault();
                    validTitle = customer.addReturn(returnMovie.getTitle());
                }

                Console.WriteLine("Continue? (Y/N)");
                continueReturn = Console.ReadKey().Key;

            }

            ShowConsoleMenuView();
        }

        private static void SaveToFile()
        {
            StringBuilder customersRental = new StringBuilder();

            registeredCustomerList.ForEach(customer =>
            {
                if(customer.getRentalExist())
                {
                    customersRental.AppendLine(customer.statement());
                }
            });

            string dateTime = DateTime.Now.ToString("yyyy-MM-dd_hh;mm;ss");
            FileStream filestream = new FileStream($"savetofile_{dateTime}.txt", FileMode.Create);
            var streamwriter = new StreamWriter(filestream);
            streamwriter.Write(customersRental.ToString());
            streamwriter.AutoFlush = true;
            streamwriter.Close();

            ShowConsoleMenuView();
            return;
        }
        
        private static void Receipt()
        {
            StringBuilder menu = new StringBuilder();
            menu.AppendLine("---Receipt Menu-----");
            Console.WriteLine(menu);
            string customerid;
            bool validCustomer = false;
            while (!validCustomer)
            {
                menu = new StringBuilder();
                menu.AppendLine("Input customer ID :");
                Console.WriteLine(menu);
                customerid = Console.ReadLine();
                foreach (var customer in registeredCustomerList)
                {
                    if (customerid.Trim() == customer.getName())
                    {
                        string receipt = customer.statement();
                        Console.WriteLine(receipt);
                        validCustomer = true;
                    }
                }
            }

            ShowConsoleMenuView();
            return;
        }
    }
}

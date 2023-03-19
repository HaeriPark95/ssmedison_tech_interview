using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoRental
{
  class Customer
  {
    public Customer(string name)
    {
        customerName = name;
    }

    public void addRental(Rental arg) { customerRental.Add(arg); }
    public string getName() { return customerName; }
    public bool getRentalExist() { return customerRental.Any(); }

    public bool addReturn(string arg)
    {
        var rentalMovie = customerRental.Where(rental => rental.getMovie().getTitle() == arg).FirstOrDefault();
        if(rentalMovie != null)
        {
            customerRental.Remove(rentalMovie);
            return true;
        }
        else
        {
            return false;
        }

    }

    //영화별 가격, 합산가격, 획득한 포인트
    public string statement()
    {
        double totalAmount = 0.0;
        int frequentRenterPoints = 0;
        StringBuilder result = new StringBuilder();
        StringBuilder newResult = new StringBuilder();

        result.AppendLine("Rental Record for" + getName());

        newResult.AppendLine("-----------------------------------");
        newResult.AppendLine("New Rental Record for "+getName());
        newResult.AppendLine("\t" + "장르".PadRight(3) + "\t" + "제목".PadRight(8) + "\t" + "대여기간".PadRight(8) + "\t" + "가격".PadRight(3));

        IEnumerator<Rental> enumerator = customerRental.GetEnumerator();

        for (; enumerator.MoveNext();)
        {
            double thisAmount = 0.0;
            Rental each = enumerator.Current;

            switch (each.getMovie().getPriceCode())
            {
                case MOVETYPE.REGULAR:
                    thisAmount += 2.0;
                    if (each.getDaysRented() > 2)
                        thisAmount += (each.getDaysRented() - 2) * 1.5;
                    break;
                case MOVETYPE.NEW_RELEASE:
                    thisAmount += each.getDaysRented() * 3;
                    break;

                case MOVETYPE.CHILDRENS:
                    thisAmount += 1.5;
                    if (each.getDaysRented() > 3)
                        thisAmount += (each.getDaysRented() - 3) * 1.5;
                    break;
                case MOVETYPE.EXAMPLE_GENRE:
                    thisAmount += each.getDaysRented() * 3;
                    break;
            }

            // Add frequent renter points
            frequentRenterPoints++;

            // Add bonus for a two day new release rental
            if ((each.getMovie().getPriceCode() == MOVETYPE.NEW_RELEASE)
                    && each.getDaysRented() > 1) frequentRenterPoints++;

            // Show figures for this rental
            result.AppendLine("\t" + each.getMovie().getTitle() + "\t" + thisAmount.ToString());
            newResult.AppendLine("\t" + each.getMovie().getPriceCode().ToString().PadRight(3) + "\t" + each.getMovie().getTitle().PadRight(8) + "\t" +
                                 each.getDaysRented().ToString().PadRight(8) + "\t" + thisAmount.ToString().ToString().PadRight(3));
            totalAmount += thisAmount;
        }

        result.AppendLine("Amount owed is " + totalAmount);
        result.AppendLine("You earned " + frequentRenterPoints + " frequent renter points");

        newResult.AppendLine("Amount owed is " + totalAmount);
        newResult.AppendLine("You earned " + frequentRenterPoints + " frequent renter points");

        result.AppendLine(newResult.ToString());

        return result.ToString();
    }

    private string customerName;
    private List<Rental> customerRental = new List<Rental>();
  }
}

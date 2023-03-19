using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VideoRental
{
  public enum MOVETYPE
  {
     REGULAR,
     NEW_RELEASE,
     CHILDRENS,
     EXAMPLE_GENRE,
  }

  public class Movie
  {
    public Movie(string title, MOVETYPE priceCode = MOVETYPE.REGULAR)
    {
        movieTitle = title;
        moviePriceCode = priceCode;
    }

    public MOVETYPE getPriceCode() { return moviePriceCode; }
    public void setPriceCode(MOVETYPE args) { moviePriceCode = args; }
    public string getTitle() { return movieTitle; }

    private string movieTitle;
    MOVETYPE moviePriceCode;
  }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Widly.Models;

namespace Widly.Controllers.API
{
    public class NewRentalsController : ApiController
    {
        private ApplicationDbContext context;
        public NewRentalsController()
        {
            context = new ApplicationDbContext();
        }
        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            var customer = context.Customers.Single(c => c.Id == newRental.CustomerId);
            var movies = context.Movies.Where(m => newRental.MovieIds.Contains(m.Id)).ToList();

            foreach (var movie in movies)
            {
                if (movie.NumberAvailable == 0)
                    return BadRequest("Movie is not available");
                movie.NumberAvailable--;
                var rental = new Rental
                {
                    Movie = movie,
                    Customer = customer,
                    DateRented = DateTime.Now
                };
                context.Rentals.Add(rental);

            }
            context.SaveChanges();
            return Ok();
        }
    }
}


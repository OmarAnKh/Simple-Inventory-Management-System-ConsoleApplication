using System;
using SimpleInventoryManagementSystem.models;

namespace AirportTicketBookingExercise
{
    static class Program
    {
        private static void Main()
        {
            Products products = Products.GetInstance();
            products.Print();
        }
    }
}
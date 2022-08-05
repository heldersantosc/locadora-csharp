using locadora.Entities;

namespace locadora.Services
{
    internal class RentalService
    {
        public double PricePerHour { get; private set; }
        public double PricePerDay { get; private set; }
        private ITaxService _taxService;

        public RentalService(double pricePerHour, double pricePerDay, ITaxService taxService)
        {
            PricePerHour = pricePerHour;
            PricePerDay = pricePerDay;
            _taxService = taxService;
        }

        public void ProcessInvoice(CarRental carRental)
        {
            TimeSpan duration = carRental.Finish.Subtract(carRental.Start);
            double basicPayment = this.getBasicPayment(duration);

            double tax = _taxService.Tax(basicPayment);
            carRental.Invoice = new Invoice(basicPayment, tax);
        }

        public double getBasicPayment(TimeSpan duration)
        {
            return duration.TotalHours <= 12.0 ?
                PricePerHour * Math.Ceiling(duration.TotalHours) : PricePerDay * Math.Ceiling(duration.TotalDays);
        }
    }
}

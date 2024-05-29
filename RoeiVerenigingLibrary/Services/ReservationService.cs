using Innovative.SolarCalculator;
using RoeiVerenigingLibrary.Exceptions;

namespace RoeiVerenigingLibrary
{
    public class ReservationService(IReservationRepository reservationRepository)
    {
        private readonly TimeSpan _maxReservationTime = new (2, 0, 0);

        public bool TimeChecker(DateTime? start, DateTime? end)
        {
            if (start < end)
            {
                return true;
            }

            throw new InvalidTimeException();
        }
        
        public Reservation Create(Member member, int boatId, DateTime startTime, DateTime endTime)
        {
            if (startTime.Date < DateTime.Now.Date)
            {
                string message = "Je mag geen reservering in het verleden maken!";
                throw new Exception(message);
            }

            if (reservationRepository.BoatAlreadyReserved(boatId, startTime, endTime))
            {
                throw new BoatAlreadyReservedException();
            }

            if (!member.Roles.Contains("beheerder"))
            {
                if (endTime - startTime > _maxReservationTime)
                {
                    string message = "Je kan voor maximaal " + _maxReservationTime.Hours + " uur reserveren!";
                    throw new Exception(message);
                }

                if (endTime.Date > DateTime.Now.AddDays(14))
                {
                    string message = "Je mag niet meer dan 2 weken van te voren reserveren!";
                    throw new Exception(message);
                }

                if (!IsReservationInDaylight(startTime, endTime))
                {
                    throw new ReservationNotInDaylightException();
                }

                if (AmountOfBoatsCurrentlyRenting(member.Id) >= 2)
                {
                    throw new MaxAmountOfReservationExceeded();
                }

                // TODO bij niveau --> moet deze niet bij de klik op een boot?
            }

            return reservationRepository.CreateReservation(member, boatId, startTime, endTime);
        }

        public bool IsReservationInDaylight(DateTime startTime, DateTime endTime)
        {
            //todo set lat/long and timezone in config
            TimeZoneInfo cet = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            SolarTimes solarTimes = new SolarTimes(startTime.Date, 52.5125, 6.09444);
            DateTime sunrise = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunrise.ToUniversalTime(), cet);
            DateTime sunset = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunset.ToUniversalTime(), cet);

            return startTime >= sunrise && endTime <= sunset;
        }

        public List<Reservation> GetReservations()
        {
            return reservationRepository.GetReservations();
        }

        public List<Reservation> GetReservations(Member member)
        {
            return reservationRepository.GetReservations(member);
        }

        private int AmountOfBoatsCurrentlyRenting(int id)
        {
            return reservationRepository.GetAmountOfBoatsCurrRenting(id);
        }

        public Reservation ChangeReservation(int reservationId, Member member, int boatId, DateTime start,
            DateTime end)
        {
            return reservationRepository.ChangeReservation(reservationId, member, boatId, start, end);
        }

        public Reservation GetReservation(int reservationId)
        {
            return reservationRepository.GetReservation(reservationId);
        }
    }
}
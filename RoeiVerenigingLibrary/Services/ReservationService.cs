using Innovative.SolarCalculator;
using RoeiVerenigingLibary.Exceptions;

namespace RoeiVerenigingLibary
{
    public class ReservationService
    {
        private readonly TimeSpan maxReservationTime = new TimeSpan(2, 0, 0);
        private readonly IReservationRepository _reservationRepository;

        public ReservationService(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

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

            if (_reservationRepository.BoatAlreadyReserved(boatId, startTime, endTime))
            {
                throw new BoatAlreadyReservedException();
            }

            if (!member.Roles.Contains("beheerder"))
            {
                if (endTime - startTime > maxReservationTime)
                {
                    string message = "Je kan voor maximaal " + maxReservationTime.Hours + " uur reserveren!";
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

            return _reservationRepository.CreateReservation(member, boatId, startTime, endTime);
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
            return _reservationRepository.GetReservations();
        }

        public List<Reservation> GetReservations(Member member)
        {
            return _reservationRepository.GetReservations(member);
        }

        public int AmountOfBoatsCurrentlyRenting(int id)
        {
            return _reservationRepository.GetAmountOfBoatsCurrRenting(id);
        }

        public Reservation ChangeReservation(int reservationdId, Member member, int boatId, DateTime start, DateTime end)
        {
            return _reservationRepository.ChangeReservation(reservationdId, member, boatId, start, end);
        }

        public Reservation GetReservation(int reservationid)
        {
            return _reservationRepository.GetReservation(reservationid);
        }
    }
}
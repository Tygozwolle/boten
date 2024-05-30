using Innovative.SolarCalculator;
using RoeiVerenigingLibrary.Exceptions;

namespace RoeiVerenigingLibrary
{
    public class ReservationService(IReservationRepository reservationRepository)
    {
        // private readonly IReservationRepository _reservationRepository;

        private readonly TimeSpan _maxReservationTime = new(2, 0, 0);


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
            if (ReservationInThePast(startTime))
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

                if (MaxReservationDatePassed(endTime))
                {
                    string message = "Je mag niet meer dan 2 weken van te voren reserveren!";
                    throw new Exception(message);
                }

                if (!IsReservationInDaylight(startTime, endTime))
                {
                    throw new ReservationNotInDaylightException();
                }

                if (MaxReservations(member))
                {
                    throw new MaxAmountOfReservationExceeded();
                }

                // TODO bij niveau --> moet deze niet bij de klik op een boot?
            }

            return reservationRepository.CreateReservation(member, boatId, startTime, endTime);
        }

        /**
         * returns true if the reservation was made in the past
         */
        public bool ReservationInThePast(DateTime startTime)
        {
            return startTime.Date < DateTime.Now.Date;
        }

        /**
         * returns true if the start and endTime are both in daylight
         */
        public bool IsReservationInDaylight(DateTime startTime, DateTime endTime)
        {
            //todo set lat/long and timezone in config
            TimeZoneInfo cet = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            SolarTimes solarTimes = new SolarTimes(startTime.Date, 52.5125, 6.09444);
            DateTime sunrise = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunrise.ToUniversalTime(), cet);
            DateTime sunset = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunset.ToUniversalTime(), cet);

            return startTime >= sunrise && endTime <= sunset;
        }

        public int AmountOfBoatsCurrentlyRenting(int memberId)
        {
            return reservationRepository.GetAmountOfBoatsCurrRenting(memberId);
        }

        /**
         * returns True if the endTime is more than 2 weeks away
         */
        public bool MaxReservationDatePassed(DateTime endTime)
        {
            return endTime.Date > DateTime.Now.AddDays(14);
        }

        /**
         * returns true if the member has the limit of 2 or more reservations
         */
        public bool MaxReservations(Member member)
        {
            return AmountOfBoatsCurrentlyRenting(member.Id) >= 2;
        }

        /**
         * returns true if The reservation is longer than 2 hours
         */
        public bool MaxReservationTimePassed(DateTime startTime, DateTime endTime)
        {
            TimeSpan maxReservationTime = new TimeSpan(2, 0, 0);
            return endTime - startTime > maxReservationTime;
        }

        public List<Reservation> GetReservations()
        {
            return reservationRepository.GetReservations();
        }

        public List<Reservation> GetReservations(Member member)
        {
            return reservationRepository.GetReservations(member);
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

        public DateTime Sunrise(DateTime selectedDate)
        {
            SolarTimes solarTimes = new SolarTimes(selectedDate.Date, 52.5125, 6.09444);
            TimeZoneInfo cet = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime sunrise = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunrise.ToUniversalTime(), cet);

            DateTime roundedSunrise = sunrise.AddMinutes(-sunrise.Minute).AddSeconds(-sunrise.Second)
                .AddMilliseconds(-sunrise.Millisecond)
                .AddMicroseconds(-sunrise.Microsecond);
            if (roundedSunrise != sunrise)
            {
                roundedSunrise = roundedSunrise.AddHours(1);
            }

            return roundedSunrise;
        }

        public DateTime Sunset(DateTime selectedDate)
        {
            SolarTimes solarTimes = new SolarTimes(selectedDate.Date, 52.5125, 6.09444);
            TimeZoneInfo cet = TimeZoneInfo.FindSystemTimeZoneById("W. Europe Standard Time");
            DateTime sunset = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunset.ToUniversalTime(), cet);
            return sunset.AddMinutes(-sunset.Minute).AddSeconds(-sunset.Second).AddMilliseconds(-sunset.Millisecond)
                .AddMicroseconds(-sunset.Microsecond);
        }

        /// <summary>
        /// Evil LINQ magic!
        /// </summary>
        public List<DateTime> GetAvailableTimes(DateTime selectedDate, List<Boat> boatList)
        {
            List<Reservation> reservations = GetReservations();

            // Start and end time bound within sunrise and sunset, rounded to the nearest full hour.
            DateTime startTime = selectedDate.AddHours(Sunrise(selectedDate).Hour);
            DateTime endTime = selectedDate.AddHours(Sunset(selectedDate).Hour);

            // Dictionary with a key of a DateTime and an integer as value.
            // The key is the DateTime of the reservation. The value is the number of reservations for that DateTime.
            // Example: 10/10/2023 16:00 could have a value of 4, meaning there are 4 reservations for that DateTime.
            Dictionary<DateTime, int> reservationsPerTimeBlock = reservations
                .SelectMany(reservation => Enumerable.Range(0, (reservation.EndTime - reservation.StartTime).Hours)
                    .Select(i => reservation.StartTime.AddHours(i)))
                .GroupBy(time => time)
                .ToDictionary(group => group.Key, group => group.Count());

            // List of DateTimes where the time falls between the earlier start and end time, and where the value of the
            // DateTime key in reservationsPerTimeBlock is less than the total amount of boats.
            List<DateTime> timeAvailableList = Enumerable.Range(0, (endTime - startTime).Hours)
                .Select(i => startTime.AddHours(i))
                .Where(time =>
                    !reservationsPerTimeBlock.ContainsKey(time) || reservationsPerTimeBlock[time] < boatList.Count)
                .ToList();

            return timeAvailableList;
        }

        public int GetTotalReservationTime(Member loggedInMember)
        {
            List<Reservation> totalReservationTimeList = GetReservations(loggedInMember);
            TimeSpan totalReservationTime = new TimeSpan();

            foreach (var reservation in totalReservationTimeList)
            {
                totalReservationTime += reservation.EndTime - reservation.StartTime;
            }

            return (int)totalReservationTime.TotalMinutes;
        }
    }
}
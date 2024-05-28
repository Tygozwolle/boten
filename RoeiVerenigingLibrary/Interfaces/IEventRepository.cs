﻿using RoeiVerenigingLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoeiVerenigingLibrary.Interfaces
{
    public interface IEventRepository
    {
       public Event Create(DateTime startTime, DateTime endDate,string descriptions, string name, int maxParticipants,List<Boat> boats , Member member);
    }
}
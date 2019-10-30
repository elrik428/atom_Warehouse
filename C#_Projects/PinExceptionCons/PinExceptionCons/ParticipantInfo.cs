using System;
using System.Collections.Generic;
using System.Text;

namespace PinExceptionCons
{
    public class ParticipantInfo
    {
        private string participantname;
        private string participantfile;
        private string participantsubject;
        private string participantextention;
        private string[] participantexceptiondays;

        public string ParticipantName
        {
            set { participantname = value; }
            get { return participantname; }
        }

        public string ParticipantFile
        {
            set { participantfile = value; }
            get { return participantfile; }
        }

        public string ParticipantSubject
        {
            set { participantsubject = value; }
            get { return participantsubject; }
        }

        public string ParticipantExtention
        {
            set { participantextention = value; }
            get { return participantextention; }
        }

        public string[] ParticipantExceptionDays
        {
            set { participantexceptiondays = value; }
            get { return participantexceptiondays; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ConferenceDTO
{
    public class SpeakersResponse
    {
        public ICollection<SpeakerResponse> Speakers { get; set; } = new List<SpeakerResponse>();
    }
}

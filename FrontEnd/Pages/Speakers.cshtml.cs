using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FrontEnd.Pages
{
    public class SpeakersModel : PageModel
    {
        private readonly IApiClient _apiClient;

        public SpeakersModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public List<SpeakerResponse> Speakers { get; set; }

        public async Task<IActionResult> OnGet()
        {
            var speakersResponse = await _apiClient.GetSpeakersAsync();

            Speakers = speakersResponse.OrderBy(speaker => speaker.Name).ToList();

            if (Speakers == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}

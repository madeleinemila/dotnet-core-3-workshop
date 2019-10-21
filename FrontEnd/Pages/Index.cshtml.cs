using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConferenceDTO;
using FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        protected readonly IApiClient _apiClient;

        public IndexModel(ILogger<IndexModel> logger, IApiClient apiClient)
        {
            _logger = logger;
            _apiClient = apiClient;
        }

        public IEnumerable<IGrouping<DateTimeOffset?, SessionResponse>> Sessions { get; set; }

        public IEnumerable<(int Offset, DayOfWeek? DayofWeek)> DayOffsets { get; set; }

        public int CurrentDayOffset { get; set; }

        public bool IsAdmin { get; set; }

        public List<int> UserSessions { get; set; } = new List<int>();

        [TempData]
        public string Message { get; set; }

        public bool ShowMessage => !string.IsNullOrEmpty(Message);

        public async Task OnGet(int day = 0)
        {
            IsAdmin = User.IsAdmin();

            CurrentDayOffset = day;

            if (User.Identity.IsAuthenticated)
            {
                var userSessions = await _apiClient.GetSessionsByAttendeeAsync(User.Identity.Name);
                UserSessions = userSessions.Select(u => u.Id).ToList();
            }

            var displaySessions = await GetSessionsAsync();
            var allSessions = await _apiClient.GetSessionsAsync();

            var startDate = allSessions.Min(s => s.StartTime?.Date);
            
            var offset = 0;
            DayOffsets = allSessions.Select(s => s.StartTime?.Date)
                                 .Distinct()
                                 .OrderBy(d => d)
                                 .Select(day => (offset++, day?.DayOfWeek));

            var filterDate = startDate?.AddDays(day);

            Sessions = displaySessions.Where(s => s.StartTime?.Date == filterDate)
                               .OrderBy(s => s.TrackId)
                               .GroupBy(s => s.StartTime)
                               .OrderBy(g => g.Key);
        }

        protected virtual Task<List<SessionResponse>> GetSessionsAsync()
        {
            return _apiClient.GetSessionsAsync();
        }

        public async Task<IActionResult> OnPostAsync(int sessionId, int day)
        {
            await _apiClient.AddSessionToAttendeeAsync(User.Identity.Name, sessionId);

            return RedirectToPage("/Index", new { day });
        }

        public async Task<IActionResult> OnPostRemoveAsync(int sessionId, int day)
        {
            await _apiClient.RemoveSessionFromAttendeeAsync(User.Identity.Name, sessionId);

            return RedirectToPage("/Index", new { day });
        }
    }
}

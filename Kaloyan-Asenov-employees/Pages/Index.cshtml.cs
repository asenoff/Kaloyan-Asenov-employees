using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Employees.Core.Employees;
using Employees.Core.Coworking.Interfaces;

namespace Kaloyan_Asenov_employees.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly ITopCoworkers _topCoworkers;

        private readonly IAllCoworkers _allCoworkers;

        private const int FILE_CONTENT_MAX_LENGTH = 500;

        internal string LastErrorMessage { get; set; } = string.Empty;

        internal string FileContent { get; set; } = string.Empty;

        internal int DisplayFileLength { get { return (FileContent.Length > FILE_CONTENT_MAX_LENGTH) ? FILE_CONTENT_MAX_LENGTH : FileContent.Length;  } }

        internal List<TopEmployeePairModel> TopCoworkers { get; private set; } = new List<TopEmployeePairModel>();

        internal List<EmployeesProjectDaysModel> AllCoworkers { get; private set; } = new List<EmployeesProjectDaysModel>();

        public IndexModel(ILogger<IndexModel> logger, ITopCoworkers topCoworkers, IAllCoworkers allCoworkers)
        {
            _logger = logger;
            _topCoworkers = topCoworkers;
            _allCoworkers = allCoworkers;
        }

        public void OnGet()
        {

        }

        public void OnPost(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                try
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    {
                        FileContent = reader.ReadToEnd();
                        TopCoworkers = _topCoworkers.GetTopCoworkers(FileContent);
                        AllCoworkers = _allCoworkers.GetAllCoworkers(FileContent);
                    }
                }
                catch(Exception ex)
                {
                    LastErrorMessage = ex.Message;
                }
            }
        }
    }
}
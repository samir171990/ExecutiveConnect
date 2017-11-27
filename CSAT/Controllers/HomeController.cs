using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CSAT.Models;
namespace CSAT.Controllers
{
    public class HomeController : Controller
    {
        CSATDBEntities db = new CSATDBEntities();
        public ActionResult AjaxHandler(jQueryDataTableParamModel param)
        {
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var data = db.UserAdditionalDatas
                   .Select(p => new
                   {
                       ExecutiveTitle = p.ExecutiveTitle,
                       Name = p.FirstName + " " + p.LastName,
                       DateOfBirth = p.DateOfBirth,
                       Email = db.AspNetUsers.Where(a => a.Id == p.aspnetUserId).Select(q => q.Email).FirstOrDefault()
                   })
                      .ToList().AsEnumerable()
                      .Select(p => new UserListViewModel
                      {
                          ExecutiveTitle = p.ExecutiveTitle,
                          Name = p.Name,
                          DateOfBirth = String.Format("{0:dd/MM/yyyy}", p.DateOfBirth),
                          Email = p.Email
                      }).ToList(); ;
            IEnumerable<UserListViewModel> filteredCompanies;
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredCompanies = (from u in db.UserAdditionalDatas
                                     join a in db.AspNetUsers on u.aspnetUserId equals a.Id
                                     where u.FirstName.Contains(param.sSearch)
                                     || u.LastName.Contains(param.sSearch)
                                     || a.Email.Contains(param.sSearch)
                                     || u.ExecutiveTitle.Contains(param.sSearch)
                                     select new
                                     {
                                         ExecutiveTitle = u.ExecutiveTitle,
                                         Name = u.FirstName + " " + u.LastName,
                                         DateOfBirth = u.DateOfBirth,
                                         Email = a.Email
                                     }).AsEnumerable().Select(p => new UserListViewModel
                                     {
                                         ExecutiveTitle = p.ExecutiveTitle,
                                         Name = p.Name,
                                         DateOfBirth = String.Format("{0:dd/MM/yyyy}", p.DateOfBirth),
                                         Email = p.Email
                                     }).ToList();

            }
            else
            {
                filteredCompanies = data;
            }
            // sorting
            filteredCompanies = this.SortByColumnWithOrder(sortColumnIndex.ToString(), sortDirection.ToUpper(), filteredCompanies.ToList());
            // Apply pagination.   
            //filteredCompanies = filteredCompanies.Skip(param.iDisplayStart).Take(param.iDisplayLength).ToList();
            var displayedCompanies = filteredCompanies;
           var result = from c in displayedCompanies
                         select new[] { Convert.ToString(c.Name), c.DateOfBirth, c.Email, c.ExecutiveTitle };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = data.Count(),
                iTotalDisplayRecords = 10,
                aaData = result
            },
                             JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        private List<UserListViewModel> SortByColumnWithOrder(string order, string orderDir, List<UserListViewModel> data)
        {
            // Initialization.   
            List<UserListViewModel> lst = new List<UserListViewModel>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.DateOfBirth).ToList() : data.OrderBy(p => p.DateOfBirth).ToList();
                        break;
                    case "2":
                        // Setting.   
                         lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Email).ToList() : data.OrderBy(p => p.Email).ToList();
                        break;
                    case "3":
                        // Setting.   
                         lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ExecutiveTitle).ToList() : data.OrderBy(p => p.ExecutiveTitle).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList() : data.OrderBy(p => p.Name).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.   
                Console.Write(ex);
            }
            // info.   
            return lst;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Thankyou()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSAT.Common
{
    public class CommonUtility
    {
        public static List<SelectListItem> TotalDaysInMonth()
        {
            var DaysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            List<SelectListItem>
                            daylist = new List<SelectListItem>
                                ();
            for (int i = 1; i <= DaysInMonth; i++)
            {
                daylist.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
            return daylist;
        }
        public static List<SelectListItem> MonthNames()
        {
            List<SelectListItem> monthNames = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                monthNames.Add(new SelectListItem
                {
                    Text = Convert.ToDateTime(i.ToString() + "/1/1900").ToString("MMMM"),
                    Value = i.ToString()
                });

            }
            return monthNames;
        }

        public static List<SelectListItem> TotalNumberOfYears()
        {
            List<SelectListItem>
                yearList = new List<SelectListItem>
                    ();
            for (int i = DateTime.Now.Year; i >= 1950; i--)
            {
                yearList.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString()
                });
            }
            return yearList;
        }
    }
}
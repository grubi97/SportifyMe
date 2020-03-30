using System;
using System.Collections.Generic;

namespace Domain
{
    public class Activity
    {
        public Guid Id {get; set;}
        public string Title{get; set;}
        public string Description{get; set;}
        public string Category{get; set;}
        public string City{get; set;}
        public DateTime Date{get; set;}
        public string venue{get; set;}

      virtual  public ICollection<UserActivity> UserActivities {get; set;}
      virtual  public ICollection<Comment> Comments {get; set;}
  
    }
}
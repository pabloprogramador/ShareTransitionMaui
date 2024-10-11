using System;
namespace ShareTransitionMaui.Sample.Models
{
    public class Users
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string SummarySmall { get; set; }
        public string Summary { get; set; }
        public string Appointment { get; set; }
        public string Avatar { get; set; }
    }


    public static class UsersMock
    {
        public static List<Users> GetUsers()
        {
            var result = new List<Users>()
{
    new Users(){Avatar = "f1.jpg", Name = "Paul", Position = "CEO", SummarySmall = "Leads the company's vision.", Summary = "Paul oversees the company's vision, manages high-level strategy, and ensures the overall success of the business, guiding all departments to meet long-term objectives.", Appointment = "07:00" },
    new Users(){Avatar = "f2.jpg", Name = "Emily", Position = "COO", SummarySmall = "Manages day-to-day ops.", Summary = "Emily ensures smooth daily operations, implementing strategies to improve efficiency and manage internal processes to support the CEO's vision.", Appointment = "08:00" },
    new Users(){Avatar = "f3.jpg", Name = "Michael", Position = "CTO", SummarySmall = "Heads technology department.", Summary = "Michael leads the tech team, managing IT infrastructure, software development, and technological innovations to ensure the company stays ahead in its industry.", Appointment = "09:00" },
    new Users(){Avatar = "f4.jpg", Name = "Sophia", Position = "CFO", SummarySmall = "Handles financial strategy.", Summary = "Sophia directs financial planning and analysis, managing budgets, investments, and risk management to maintain the company’s financial health.", Appointment = "10:00" },
    new Users(){Avatar = "f5.jpg", Name = "James", Position = "Marketing Manager", SummarySmall = "Leads marketing efforts.", Summary = "James oversees marketing campaigns, brand management, and customer engagement strategies to boost product visibility and attract clients.", Appointment = "11:00" },
    new Users(){Avatar = "f6.jpg", Name = "Olivia", Position = "HR Manager", SummarySmall = "Manages recruitment and HR.", Summary = "Olivia handles all aspects of human resources, from recruitment to employee development, ensuring a positive and productive workplace culture.", Appointment = "12:00" },
    new Users(){Avatar = "f7.jpg", Name = "David", Position = "Product Manager", SummarySmall = "Oversees product lifecycle.", Summary = "David manages the product development process, coordinating with cross-functional teams to deliver high-quality products that meet market demands.", Appointment = "13:00" },
    new Users(){Avatar = "f8.jpg", Name = "Isabella", Position = "Sales Director", SummarySmall = "Heads sales strategies.", Summary = "Isabella leads the sales team, setting sales targets, analyzing market trends, and building relationships with key clients to drive revenue growth.", Appointment = "14:00" },
    new Users(){Avatar = "f9.jpg", Name = "William", Position = "Legal Advisor", SummarySmall = "Handles legal matters.", Summary = "William advises on legal issues, manages contracts, and ensures the company's operations comply with relevant laws and regulations.", Appointment = "15:00" },
    new Users(){Avatar = "f10.jpg", Name = "Ava", Position = "UX/UI Designer", SummarySmall = "Designs user interfaces.", Summary = "Ava designs user-friendly interfaces and experiences, ensuring that the company’s digital products are intuitive and accessible to users.", Appointment = "16:00" },
    new Users(){Avatar = "f11.jpg", Name = "Daniel", Position = "DevOps Engineer", SummarySmall = "Manages deployment processes.", Summary = "Daniel ensures continuous integration and deployment of software, optimizing infrastructure for performance and stability.", Appointment = "17:00" },
    new Users(){Avatar = "f12.jpg", Name = "Mia", Position = "Content Strategist", SummarySmall = "Plans content direction.", Summary = "Mia develops content strategies for the company’s digital presence, ensuring all material aligns with branding and business goals.", Appointment = "18:00" },
    new Users(){Avatar = "f13.jpg", Name = "Henry", Position = "Data Scientist", SummarySmall = "Analyzes company data.", Summary = "Henry uses data analytics to derive actionable insights, guiding business decisions through trends, forecasts, and performance metrics.", Appointment = "19:00" },
    new Users(){Avatar = "f14.jpg", Name = "Grace", Position = "Customer Support Lead", SummarySmall = "Heads customer support.", Summary = "Grace manages the support team, ensuring client issues are resolved promptly and service levels exceed customer expectations.", Appointment = "20:00" },
    new Users(){Avatar = "f15.jpg", Name = "Samuel", Position = "Software Engineer", SummarySmall = "Develops software solutions.", Summary = "Samuel writes and maintains software, collaborating with teams to build reliable and scalable solutions that meet business needs.", Appointment = "21:00" },
    new Users(){Avatar = "f16.jpg", Name = "Lily", Position = "Operations Analyst", SummarySmall = "Improves internal processes.", Summary = "Lily analyzes operational workflows, identifying bottlenecks and proposing solutions to enhance efficiency and streamline processes.", Appointment = "22:00" },
    new Users(){Avatar = "f17.jpg", Name = "Alexander", Position = "Network Administrator", SummarySmall = "Manages IT networks.", Summary = "Alexander ensures the company’s network infrastructure is secure, reliable, and optimized, overseeing maintenance and troubleshooting.", Appointment = "23:00" },
};

            return result;
        }
    }
}

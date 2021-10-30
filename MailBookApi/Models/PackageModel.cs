using System;

namespace MailBookApi.Models
{
    public class PackageModel
    {
        public string Id { get; internal set; }

        public string PackageNumber { get; internal set; }

        public DateTime ArrivedDate { get; internal set; }

        public string DeliverCompany { get; internal set; }
    }
}
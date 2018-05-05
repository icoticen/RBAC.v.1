using System;

namespace Table
{

    public interface IActBase
    {
        int ID { get; set; }
        int AppSource { get; set; }
        int AccountType { get; set; }
        string Mac { get; set; }
        string IP { get; set; }
        string PhoneModel { get; set; }
        int KeyID { get; set; }
        int UserID { get; set; }
        DateTime? ActionDateTime { get; set; }
        DateTime? CancleDateTime { get; set; }
        bool? HasCancle { get; set; }
    }
}
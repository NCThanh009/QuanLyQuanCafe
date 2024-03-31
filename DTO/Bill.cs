using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Bill
    {
        private DateTime? dataCheckIn;
        private DateTime? dataCheckOut;
        private int id;
        private int status;
        private int discount;
        public Bill(int id, DateTime? datein, DateTime? dateout,int sta,int dis = 0) 
        {
            this.id = id;
            this.status = sta;
            this.dataCheckIn = datein;
            this.dataCheckOut = dateout;
            this.discount = dis;
        }
        public Bill(DataRow row)
        {
            this.id = (int)row["id"];
            this.status = (int)row["status"];
            this.dataCheckIn = (DateTime?)row["DateCheckIn"];
            var checkout=row["DateCheckOut"];
            if(checkout.ToString()!= "")
            {
                this.dataCheckOut=(DateTime?)checkout;
            }
            if(row["discount"].ToString() != "")
            {
                this.discount = (int)row["discount"];
            }
            
        }
        public DateTime? DataCheckIn { get => dataCheckIn; set => dataCheckIn = value; }
        public DateTime? DataCheckOut { get => dataCheckOut; set => dataCheckOut = value; }
        public int Id { get => id; set => id = value; }
        public int Status { get => status; set => status = value; }
        public int Discount { get => discount; set => discount = value; }
    }
}

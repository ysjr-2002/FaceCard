using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DYW.ImageReview.Core
{
    /// <summary>
    /// 卡和图像
    /// </summary>
    class CardPhoto
    {
        private static string filePath = "cardphoto.xml";

        public static void GetCardPhotos()
        {
            var doc = XElement.Load(filePath);

            var query = from n in doc.Elements("card")
                        select
                        new Employee
                        {
                            EmpName = n.Element("name").Value,
                            EmpCard = n.Element("cardno").Value,
                            EmpPhoto = n.Element("photo").Value
                        };
            CardPhotos = query.ToList();
        }

        public static Employee FindCardByNo(int cardNo)
        {
            var item = CardPhotos.FirstOrDefault(s => s.EmpCard == cardNo.ToString());
            return item;
        }

        public static ICollection<Employee> CardPhotos
        {
            get;
            private set;
        }

        public static bool IsCardNoExist(string cardno)
        {
            return CardPhotos.Count(s => s.EmpCard == cardno) > 0;
        }

        public static void Add(string cardNo, string name, string photo)
        {
            var doc = XElement.Load(filePath);
            var card = new XElement("card");
            card.Add(new XElement("cardno", cardNo));
            card.Add(new XElement("name", name));
            card.Add(new XElement("photo", photo));
            doc.Add(card);
            doc.Save(filePath);

            GetCardPhotos();
        }

        public static void Delete(string cardNo)
        {
            var doc = XElement.Load(filePath);
            var del = doc.Elements("card").FirstOrDefault(s => s.Element("cardno").Value == cardNo);
            if (del != null)
            {
                del.Remove();
            }
            doc.Save(filePath);

            GetCardPhotos();
        }
    }
}

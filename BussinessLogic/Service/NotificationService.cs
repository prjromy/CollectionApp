using BuisnessObject.ViewModel;
using BussinessLogic.Repository;
using DataAccess.DatabaseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinessLogic.Service
{
    public class NotificationService
    {
        ReturnBaseMessageModel returnMessage = null;
        private GenericUnitOfWork uow = null;


        public NotificationService()
        {
            returnMessage = new ReturnBaseMessageModel();
            uow = new GenericUnitOfWork();

        }

        public NotificationViewModel GetMappedCollector(int? locationId)
        {
            NotificationViewModel notification = new NotificationViewModel();
            var collector = uow.Repository<LocationVsCollector>().FindBy(x => x.LocationId== locationId).ToList();
            var newcollector = (from u in collector
                                select new NotificationViewModel
                                {
                                    collectorId = Convert.ToInt32(u.CollectorId)
                                }).ToList();

            foreach (var item in newcollector)
            {
                notification.collectorName = uow.Repository<User>().FindBy(x => x.UserId == item.collectorId).Select(x => x.UserName).SingleOrDefault();
            }
            notification.notificationList.Add(notification);
            return notification;
        }
    }
}

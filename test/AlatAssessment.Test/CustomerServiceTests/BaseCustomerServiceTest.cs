using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.UnitOfWork;
using AlatAssessment.Services.Implementation;
using AlatAssessment.Services.Interfaces;

namespace AlatAssessment.Test.CustomerServiceTests
{
    public class BaseCustomerServiceTest
    {
        protected internal readonly Mock<IUnitOfWork> unitOfWork;
        protected internal readonly Mock<INotificationService> notificationService;
        protected internal readonly ICustomerService customerService;

        public BaseCustomerServiceTest()
        {
            var myProfile = new CustomerService.CustomerProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            IMapper mapper = new Mapper(configuration);
            unitOfWork = new Mock<IUnitOfWork>();
            notificationService = new Mock<INotificationService>();
            customerService = new CustomerService(unitOfWork.Object, notificationService.Object, mapper);
        }
    }
}

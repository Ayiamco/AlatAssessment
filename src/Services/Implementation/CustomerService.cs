using System;
using System.Linq;
using System.Threading.Tasks;

using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DataAccess.UnitOfWork;
using AlatAssessment.DTOs;
using AlatAssessment.Helpers;
using AlatAssessment.Services.Interfaces;

using AutoMapper;


namespace AlatAssessment.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly INotificationService _notificationService;

        private readonly IMapper _mapper;

        internal readonly IPasswordManager _passwordManager;

        public CustomerService(IUnitOfWork unitOfWork, INotificationService notification,
            IMapper mapper,IPasswordManager passwordManager)
        {
            _notificationService = notification;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordManager = passwordManager;
        }

        public async Task<ServiceResp> AddCustomer(AddCustomerDTO addCustomerDto)
        {
            var customerInDb= _unitOfWork.CustomerRepo.Find(x => x.Email == addCustomerDto.Email.Trim())?.FirstOrDefault();
            if (customerInDb != null && customerInDb.IsVerified)
                return new ServiceResp(ResponseCodes.ClientFailure,"Account with email already exist.");

            if (customerInDb == null)
            {
                var customer = _mapper.Map<Customer>(addCustomerDto);
                customer.Password = _passwordManager.GetHash(customer.Password);
                await _unitOfWork.CustomerRepo.Create(customer);
                await _unitOfWork.SaveAsync();
            }

            _notificationService.SendOtp(addCustomerDto.PhoneNumber);
            return new ServiceResp(ResponseCodes.Success, "Verification code sent to phone."); ;
        }

        public  PaginationHelper.PagedList<CustomerDTO> GetAllCustomer(int pageSize,int page) =>
            _unitOfWork.CustomerRepo.GetAllCustomers(pageSize, page);


        public class CustomerProfile : Profile
        {
            public CustomerProfile()
            {
                CreateMap<AddCustomerDTO, Customer>()
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
            }
        }
    }


   
}

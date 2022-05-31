using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DataAccess.UnitOfWork;
using AlatAssessment.DTOs;
using AlatAssessment.Helpers;
using AlatAssessment.Services.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace AlatAssessment.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly INotificationService _notificationService;

        private readonly IMapper _mapper;
        public CustomerService(IUnitOfWork unitOfWork, INotificationService notification,IMapper mapper)
        {
            _notificationService = notification;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResp> AddCustomer(AddCustomerDTO addCustomerDto)
        {
            var customerInDb= _unitOfWork.CustomerRepo.Find(x => x.Email == addCustomerDto.Email.Trim())?.FirstOrDefault();
            if (customerInDb != null && customerInDb.IsVerified)
                return new ServiceResp(ResponseCodes.ClientFailure,"Account with email already exist.");

            if (customerInDb == null)
            {
                var customer = _mapper.Map<Customer>(addCustomerDto);
                await _unitOfWork.CustomerRepo.Create(customer);
                await _unitOfWork.SaveAsync();
            }

            _notificationService.SendOtp(addCustomerDto.PhoneNumber);
            return new ServiceResp(ResponseCodes.Success, "Verification code sent to phone."); ;
        }

        public  PaginationHelper.PagedList<CustomerDTO> GetAllCustomer(int pageSize,int page)
        {
            return _unitOfWork.CustomerRepo.GetAllCustomers(pageSize, page);
        }

        

        public class CustomerProfile : Profile
        {
            public CustomerProfile()
            {
                CreateMap<AddCustomerDTO, Customer>()
                    .ForMember(dest => dest.Password, opt => opt.MapFrom(src => HashPassword(src.Password)))
                    .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Trim()))
                    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.ModifiedAt, opt => opt.MapFrom(src => DateTime.Now));
            }

            private static string HashPassword(string password)
            {
                using (var sha = new System.Security.Cryptography.SHA256Managed())
                {
                    byte[] textData = System.Text.Encoding.UTF8.GetBytes(password);
                    byte[] hash = sha.ComputeHash(textData);
                    return BitConverter.ToString(hash).Replace("-", string.Empty);
                }
            }
        }
    }


   
}

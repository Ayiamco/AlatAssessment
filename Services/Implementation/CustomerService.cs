using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DataAccess.UnitOfWork;
using AlatAssessment.DTOs;
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

        public async Task<List<CustomerDTO>> GetAllCustomer(int pageSize,int page)
        {
            var customers=await _unitOfWork.CustomerRepo.GetAllCustomers(pageSize, page);

            return customers;
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
                byte[] salt = new byte[128 / 8];
                using (var rngCsp = new RNGCryptoServiceProvider())
                {
                    rngCsp.GetNonZeroBytes(salt);
                }

                return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password.Trim(),
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 100000,
                    numBytesRequested: 256 / 8));
            }
        }
    }


   
}

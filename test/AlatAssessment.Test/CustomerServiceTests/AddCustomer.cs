using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlatAssessment.DataAccess.Entities;
using AlatAssessment.DataAccess.UnitOfWork;
using AlatAssessment.DTOs;
using AlatAssessment.Services.Interfaces;
using AutoMapper;
using Xunit;

namespace AlatAssessment.Test.CustomerServiceTests
{
    public class AddCustomer : BaseCustomerServiceTest
    {
        [Fact]
        public void ShouldAddCustomer()
        {
            //Arrange 
            var customerDto = new AddCustomerDTO
            {
                PhoneNumber = "11111111111",
                Password = "12345678",
                LgaId = 1,
                StateId = 1,
                Email="dummy@dummy.com"
            };
            var customer = new Customer()
            {
                CreatedAt = DateTime.Now,
                Email= customerDto.Email,
            };
            notificationService.Setup(x => x.SendOtp(customerDto.PhoneNumber)).Returns(true);
            unitOfWork.Setup(x => x.CustomerRepo.Find(x => x.Email == customerDto.Email.Trim())).Returns(new List<Customer>());

            //Act
            var actual=customerService.AddCustomer(customerDto);

            //Assert
            Assert.Equal(actual.Result.Code,ResponseCodes.Success);
            unitOfWork.Verify(x=> x.CustomerRepo.Create(It.Is<Customer>(y=>
                y.CreatedAt !=customer.CreatedAt &&
                y.Email==customer.Email &&
                y.Password== "EF797C8118F02DFB649607DD5D3F8C7623048C9C063D532CC95C5ED7A898A64F" &&
                y.Id==new Guid() &&
                y.LgaId==customerDto.LgaId &&
                y.IsVerified == false &&
                y.PhoneNumber== customerDto.PhoneNumber &&
                y.ModifiedAt != customer.ModifiedAt 

            )),Times.Once);
            unitOfWork.Verify(x=> x.SaveAsync(),Times.Once);
        }


        [Fact]
        public void ShouldSendVerificationOtp()
        {

        }

        [Fact]
        public void ShouldNotAddAlreadyExistingEmail()
        {

        }


    }
}
